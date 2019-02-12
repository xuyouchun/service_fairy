using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.UserCenter;
using ServiceFairy.DbEntities.User;
using Common.Data;
using System.Diagnostics.Contracts;
using Common.Utility;
using ServiceFairy.Entities;
using Common.Data.UnionTable;
using ServiceFairy.DbEntities;
using Common;
using ServiceFairy.Entities.User;
using ServiceFairy.Components;

namespace ServiceFairy.Service.UserCenter.Components
{
    /// <summary>
    /// 用户信息管理器
    /// </summary>
    [AppComponent("用户信息管理器", "加载并缓存用户的信息")]
    class UserInfoManagerAppComponent : TimerAppComponentBase
    {
        public UserInfoManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;

            _cacheChain = new CacheChain<UserCacheKey, object>(new ICacheChainNode<UserCacheKey, object>[]{
                CacheChain<UserCacheKey, object>.CreateMemoryCacheNode(TimeSpan.FromMinutes(1)),
                new DistribuedCacheChainNode<UserCacheKey, object>(service.Invoker, TimeSpan.FromMinutes(30), "s_u_"),
                CacheChain<UserCacheKey, object>.CreateLoaderNode(_LoadUserInfos)
            });

            _utConProvider = new RemoteUtConnectionProvider(_service.Invoker, DbEntityUtility.LoadReviseInfo());
        }

        private readonly Service _service;
        private readonly IUtConnectionProvider _utConProvider;

        private readonly CacheChain<UserCacheKey, object> _cacheChain;

        private static KeyValuePair<UserCacheKey, object>[] _GetEntities(DbUser dbUser, UserInfoMask mask)
        {
            List<KeyValuePair<UserCacheKey, object>> objs = new List<KeyValuePair<UserCacheKey, object>>();

            foreach (UserInfoMask m in _masks.Keys.Where(m => mask.HasFlag(m)))
            {
                object obj;
                switch (m)
                {
                    case UserInfoMask.Basic:
                        obj = new UserBasicInfo() {
                            UserId = dbUser.UserId, UserName = dbUser.UserName, Sid = Sid.Parse(dbUser.Sid),
                            Name = dbUser.Name, CreationTime = dbUser.CreateTime, Enable = dbUser.Enable,
                        };
                        break;

                    case UserInfoMask.Detail:
                        obj = new UserDetailInfo() {
                            UserId = dbUser.UserId, Items = DbUser.DetailToDict(dbUser.Detail), ChangedTime = dbUser.DetailChangedTime,
                        };
                        break;

                    case UserInfoMask.Status:
                        obj = new UserStatusInfo() {
                            UserId = dbUser.UserId, Status = dbUser.Status, ChangedTime = dbUser.StatusChangedTime,
                        };
                        break;

                    default:
                        continue;
                }

                objs.Add(new KeyValuePair<UserCacheKey, object>(new UserCacheKey { Mask = m, UserId = dbUser.UserId }, obj));
            }

            return objs.ToArray();
        }

        private KeyValuePair<UserCacheKey, object>[] _LoadUserInfos(UserCacheKey[] keys, bool refresh)
        {
            if (keys.IsNullOrEmpty())
                return Array<KeyValuePair<UserCacheKey, object>>.Empty;

            List<KeyValuePair<UserCacheKey, object>> list = new List<KeyValuePair<UserCacheKey, object>>();
            foreach (var g in keys.GroupBy(key => key.Mask))
            {
                UserInfoMask mask = g.Key;
                DbUser[] users = DbUser.Select(_utConProvider, g.ToArray(v => v.UserId), _GetColumnNames(mask));
                foreach (DbUser user in users ?? Array<DbUser>.Empty)
                {
                    list.AddRange(_GetEntities(user, mask));
                }
            }

            return list.ToArray();
        }

        private static readonly Dictionary<UserInfoMask, string[]> _masks = new Dictionary<UserInfoMask, string[]> {
            { UserInfoMask.Basic, new [] { DbUser.F_UserName, DbUser.F_Name, DbUser.F_Sid, DbUser.F_CreateTime, DbUser.F_Enable } },
            { UserInfoMask.Detail, new [] { DbUser.F_Detail, DbUser.F_DetailChangedTime } },
            { UserInfoMask.Status, new [] { DbUser.F_Status, DbUser.F_StatusUrl, DbUser.F_StatusChangedTime } },
        };

        private UserCacheKey[] _ToCacheKeys(UserInfoMask mask, int[] userIds)
        {
            List<UserCacheKey> keys = new List<UserCacheKey>();

            foreach (UserInfoMask m in _masks.Keys)
            {
                if (mask.HasFlag(m))
                    keys.AddRange(userIds.Select(userId => new UserCacheKey() { UserId = userId, Mask = mask }));
            }

            return keys.ToArray();
        }

        private string[] _GetColumnNames(UserInfoMask mask)
        {
            HashSet<string> hs = new HashSet<string> { DbUser.F_UserId };
            foreach (KeyValuePair<UserInfoMask, string[]> item in _masks)
            {
                if (mask.HasFlag(item.Key))
                    hs.AddRange(item.Value);
            }

            return hs.ToArray();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="users">用户</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>用户信息</returns>
        public UserInfos[] GetUserInfos(UserInfoMask mask, Users users, bool refresh = false)
        {
            Contract.Requires(users != null);

            int[] userIds = _service.UserCollectionParser.Parse(users);
            KeyValuePair<UserCacheKey, object>[] values = _cacheChain.GetRange(_ToCacheKeys(mask, userIds), refresh);
            Dictionary<int, UserInfos> dict = new Dictionary<int, UserInfos>();

            foreach (KeyValuePair<UserCacheKey, object> value in values)
            {
                UserInfos info = dict.GetOrSet(value.Key.UserId, (userId) => new UserInfos() { UserId = userId });
                _AssignValue(info, value.Value, value.Key.Mask);
            }

            return dict.Values.ToArray();
        }

        private void _AssignValue(UserInfos info, object value, UserInfoMask mask)
        {
            switch (mask)
            {
                case UserInfoMask.Basic:
                    info.BasicInfo = value as UserBasicInfo;
                    break;

                case UserInfoMask.Detail:
                    info.DetailInfo = value as UserDetailInfo;
                    break;

                case UserInfoMask.Status:
                    info.StatusInfo = value as UserStatusInfo;
                    break;
            }
        }

        /// <summary>
        /// 获取用户的基础信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public UserBasicInfo GetUserBasicInfo(int userId, bool refresh = false)
        {
            return GetUserBasicInfos(Users.FromUserId(userId), refresh).FirstOrDefault();
        }

        /// <summary>
        /// 批量获取用户的基础信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public UserBasicInfo[] GetUserBasicInfos(int[] userIds, bool refresh = false)
        {
            return GetUserBasicInfos(Users.FromUserIds(userIds), refresh);
        }

        /// <summary>
        /// 获取指定用户的基础信息
        /// </summary>
        /// <param name="users"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public UserBasicInfo[] GetUserBasicInfos(Users users, bool refresh = false)
        {
            if (Users.IsNullOrEmpty(users))
                return Array<UserBasicInfo>.Empty;

            return GetUserInfos(UserInfoMask.Basic, users, refresh).ToArray(uItem => uItem.BasicInfo);
        }

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            base.OnStatusChanged(status);

            _service.ServiceEvent.Switch<User_InfoChanged_Event>(_OnUser_InfoChanged, status);
        }

        // 用户信息变化
        private void _OnUser_InfoChanged(object sender, ServiceEventArgs<User_InfoChanged_Event> e)
        {
            int[] userIds;
            if (e.Entity != null && !(userIds = e.Entity.UserIds).IsNullOrEmpty())
            {
                _ClearCache(e.Entity.UserIds);
            }
        }

        private void _ClearCache(int[] userIds)
        {
            _cacheChain.RemoveRange(UserCacheKey.CreateCacheKeys(userIds));
        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }
    }
}
