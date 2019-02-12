using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Utility;
using System.Diagnostics.Contracts;
using ServiceFairy.DbEntities.User;
using Common.Data.SqlExpressions;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Service.User.Components
{
    /// <summary>
    /// 用户关系管理器
    /// </summary>
    [AppComponent("用户关系管理器", "创建并维护用户之间的关系")]
    class UserRelationManagerAppComponent : TimerAppComponentBase
    {
        public UserRelationManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(1))
        {
            _service = service;
        }

        private readonly Service _service;
        private readonly List<FollowerRelationItem> _items = new List<FollowerRelationItem>();
        private readonly HashSet<int> _changedUserIds = new HashSet<int>();

        /// <summary>
        /// 添加粉丝
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="followerId">粉丝ID</param>
        /// <param name="time">创建时间</param>
        /// <param name="async">是否异步</param>
        public void AddFollower(int userId, int followerId, DateTime time = default(DateTime), bool async = false)
        {
            AddFollowers(userId, new[] { followerId }, time, async);
        }

        /// <summary>
        /// 批量添加粉丝
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="followerIds">粉丝ID</param>
        /// <param name="time">创建时间</param>
        /// <param name="async">是否异步</param>
        public void AddFollowers(int userId, int[] followerIds, DateTime time = default(DateTime), bool async = false)
        {
            Contract.Requires(userId != 0);

            if (followerIds.IsNullOrEmpty())
                return;

            AddFollowers(followerIds.ToArray(fid => new FollowerRelationItem {
                UserId = userId, FollowerId = fid, Time = time
            }), async);
        }

        /// <summary>
        /// 批量添加粉丝
        /// </summary>
        /// <param name="items"></param>
        /// <param name="async">是否异步添加</param>
        public void AddFollowers(FollowerRelationItem[] items, bool async = false)
        {
            if (items.IsNullOrEmpty())
                return;

            if (async)
            {
                _items.SafeAddRange(items);
                return;
            }

            List<DbFollower> dbFollowers = new List<DbFollower>();
            List<DbFollowing> dbFollowings = new List<DbFollowing>();

            DateTime now = DateTime.UtcNow;
            HashSet<int> userIds = new HashSet<int>();
            foreach (FollowerRelationItem item in items.Distinct())
            {
                if (item.IsAvaliable())
                {
                    dbFollowers.Add(new DbFollower {
                        UserId = item.UserId,
                        FollowerId = item.FollowerId,
                        CreationTime = (item.Time == default(DateTime)) ? now : item.Time
                    });

                    dbFollowings.Add(new DbFollowing {
                        UserId = item.FollowerId,
                        FollowingId = item.UserId,
                        CreationTime = (item.Time == default(DateTime)) ? now : item.Time
                    });

                    userIds.Add(item.UserId);
                    userIds.Add(item.FollowerId);
                }
            }

            if (dbFollowers.Count > 0)
            {
                DbFollower.Insert(_service.DbConnectionManager.Provider, dbFollowers);
                DbFollowing.Insert(_service.DbConnectionManager.Provider, dbFollowings);

                _changedUserIds.SafeAddRange(userIds);
            }
        }

        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="followingId">关注ID</param>
        /// <param name="time">时间</param>
        /// <param name="async">是否异步方式</param>
        public void AddFollowing(int userId, int followingId, DateTime time = default(DateTime), bool async = false)
        {
            AddFollowings(userId, new[] { followingId }, time, async);
        }

        /// <summary>
        /// 批量添加关注
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="followingIds">关注ID</param>
        /// <param name="time">时间</param>
        /// <param name="async">是否异步方式</param>
        public void AddFollowings(int userId, int[] followingIds, DateTime time = default(DateTime), bool async = false)
        {
            if (followingIds.IsNullOrEmpty())
                return;

            AddFollowings(followingIds.ToArray(fid => new FollowingRelationItem {
                UserId = userId, FollowingId = fid, Time = time
            }), async);
        }

        /// <summary>
        /// 批量添加关注
        /// </summary>
        /// <param name="items"></param>
        /// <param name="async">是否异步添加</param>
        public void AddFollowings(FollowingRelationItem[] items, bool async = false)
        {
            if (items.IsNullOrEmpty())
                return;

            AddFollowers(items.ToArray(item => item.ToFollower()), async);
        }

        protected override void OnExecuteTask(string taskName)
        {
            if (_items.Count == 0)
                return;

            FollowerRelationItem[] items = _items.SafeToArray(clear: true);
            InvokeNoThrow(() => AddFollowers(items, false));

            int[] changedUserIds = _changedUserIds.SafeToArray(clear: true, trimExcess: true);
            if (changedUserIds.Length > 0)
                _service.ServiceEvent.Raise(new User_RelationChanged_Event { UserIds = changedUserIds });
        }

        // 从已有的关系表中寻找关系
        public void CreateRelations(string username, int userId, DateTime time = default(DateTime))
        {
            if (time == default(DateTime))
                time = DateTime.UtcNow;

            var provider = _service.DbConnectionManager.Provider;

            // 将自己添加到关系表中
            SqlExpression where = SqlExpression.Equals(DbUserRelation.F_FollowerId, 0);
            DbUserRelation.DeleteByRouteKey(provider, username, (string)where);
            DbUserRelation relation = new DbUserRelation { UserId = userId, UserName = username, FollowerId = 0 };
            relation.Insert(provider);

            // 寻找关系
            SqlExpression where2 = SqlExpression.NotEquals(DbUserRelation.F_FollowerId, 0);
            DbUserRelation[] relations = DbUserRelation.Select(provider,
                username, new[] { DbUserRelation.F_UserId, DbUserRelation.F_FollowerId }, (string)where2);

            if (!relations.IsNullOrEmpty())
            {
                // 添加粉丝与关注
                int[] followerIds = relations.Select(r => r.FollowerId).ToArray();
                AddFollowers(userId, followerIds, async: true);

                // 更新关注
                DbUserRelation rel = new DbUserRelation { UserName = username, UserId = userId };
                rel.Update(provider, DbUserRelation.F_UserId);
            }
        }
    }

    #region Struct FollowerRelationItem ...

    public struct FollowerRelationItem
    {
        public int UserId, FollowerId;
        public DateTime Time;

        public bool IsAvaliable()
        {
            return UserId != 0 && FollowerId != 0;
        }

        public override int GetHashCode()
        {
            return UserId ^ FollowerId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(FollowerRelationItem))
                return false;

            FollowerRelationItem obj2 = (FollowerRelationItem)obj;
            return obj2.UserId == UserId && obj2.FollowerId == FollowerId;
        }

        public FollowingRelationItem ToFollowing()
        {
            return new FollowingRelationItem { UserId = FollowerId, FollowingId = UserId, Time = Time };
        }
    }

    #endregion

    #region Struct FollowingRelationItem ...

    public struct FollowingRelationItem
    {
        public int UserId, FollowingId;
        public DateTime Time;

        public bool IsAvaliable()
        {
            return UserId != 0 && FollowingId != 0;
        }

        public override int GetHashCode()
        {
            return UserId ^ FollowingId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(FollowingRelationItem))
                return false;

            FollowingRelationItem obj2 = (FollowingRelationItem)obj;
            return obj2.UserId == UserId && obj2.FollowingId == FollowingId;
        }

        public FollowerRelationItem ToFollower()
        {
            return new FollowerRelationItem { FollowerId = UserId, UserId = FollowingId, Time = Time };
        }
    }

    #endregion
}
