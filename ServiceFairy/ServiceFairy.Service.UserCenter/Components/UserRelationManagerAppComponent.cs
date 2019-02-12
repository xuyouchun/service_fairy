using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities;
using ServiceFairy.Components;
using ServiceFairy.Entities.User;
using Common.Utility;
using System.Threading;
using System.Threading.Tasks;
using Common.Package;
using Common.Data;
using ServiceFairy.SystemInvoke;
using ServiceFairy.DbEntities.User;
using Common;
using Common.Data.SqlExpressions;
using CacheChain = Common.Package.CacheChain<int, int[]>;
using ICacheChainNode = Common.Package.ICacheChainNode<int, int[]>;
using DistribuedCacheChainNode = ServiceFairy.SystemInvoke.DistribuedCacheChainNode<int, int[]>;
using Kp = System.Collections.Generic.KeyValuePair<int, int[]>;
using Common.Data.UnionTable;
using ServiceFairy.DbEntities;

namespace ServiceFairy.Service.UserCenter.Components
{
    /// <summary>
    /// 用户关系管理器
    /// </summary>
    [AppComponent("用户关系管理器", "生成并管理用户之间的关系")]
    class UserRelationManagerAppComponent : TimerAppComponentBase
    {
        public UserRelationManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;

            _utConProvider = new RemoteUtConnectionProvider(_service.Invoker, DbEntityUtility.LoadReviseInfo());

            _followerCache = new CacheChain(new ICacheChainNode[] {
                CacheChain.CreateMemoryCacheNode(TimeSpan.FromSeconds(10)),
                new DistribuedCacheChainNode(_service.Invoker, TimeSpan.FromHours(1), "s_fr_"),
                CacheChain.CreateLoaderNode(_LoadFollower)
            });

            _followingCache = new CacheChain(new ICacheChainNode[] {
                CacheChain.CreateMemoryCacheNode(TimeSpan.FromSeconds(10)),
                new DistribuedCacheChainNode(_service.Invoker, TimeSpan.FromHours(1), "s_fg_"),
                CacheChain.CreateLoaderNode(_LoadFollowing)
            });
        }

        private readonly Service _service;
        private readonly IUtConnectionProvider _utConProvider;
        private readonly CacheChain _followerCache, _followingCache;

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            base.OnStatusChanged(status);

            _service.ServiceEvent.Switch<User_Login_Event>(_OnUserLogin, status);
            _service.ServiceEvent.Switch<User_Register_Event>(_OnUserRegister, status);
            _service.ServiceEvent.Switch<User_RelationChanged_Event>(_OnContactListChanged, status);
        }

        // 用户登录
        private void _OnUserLogin(object sender, ServiceEventArgs<User_Login_Event> e)
        {
            
        }

        // 新用户注册
        private void _OnUserRegister(object sender, ServiceEventArgs<User_Register_Event> e)
        {
            
        }

        // 联系人列表变化
        private void _OnContactListChanged(object sender, ServiceEventArgs<User_RelationChanged_Event> e)
        {
            User_RelationChanged_Event entity = e.Entity;
            if (entity != null)
            {
                _followingCache.RemoveRange(entity.UserIds);
                _followerCache.RemoveRange(entity.UserIds);
            }
        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }

        // 加载粉丝
        private Kp[] _LoadFollower(int[] userIds, bool refresh)
        {
            DbFollower[] followers = DbFollower.Select(_utConProvider, userIds, new[] { DbFollower.F_UserId, DbFollower.F_FollowerId });
            return followers.GroupBy(f => f.UserId, f => f.FollowerId).ToArray(g => new Kp(g.Key, g.ToArray()));
        }

        // 加载关注
        private Kp[] _LoadFollowing(int[] userIds, bool refresh)
        {
            DbFollowing[] followings = DbFollowing.Select(_utConProvider, userIds, new[] { DbFollowing.F_UserId, DbFollowing.F_FollowingId });
            return followings.GroupBy(f => f.UserId, f => f.FollowingId).ToArray(g => new Kp(g.Key, g.ToArray()));
        }

        private static Kp[] _GroupUserIds<T>(int[] userIds, T[] items, Func<T, int> keySelector, Func<T, int> valueSelector)
        {
            if (userIds.Length == 0)
                return Array<KeyValuePair<int, int[]>>.Empty;

            if (userIds.Length == 1)
                return new[] { new Kp(userIds[0], items.ToArray(valueSelector)) };

            return items.GroupBy(keySelector).ToArray(g => new Kp(g.Key, g.Select(valueSelector).ToArray()));
        }

        /// <summary>
        /// 获取用户粉丝表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int[] GetFollowers(int userId)
        {
            return _followerCache.Get(userId);
        }

        /// <summary>
        /// 获取用户粉丝表
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int[] GetFollowers(int[] userIds)
        {
            if (userIds.IsNullOrEmpty())
                return Array<int>.Empty;

            return _followerCache.GetRange(userIds).SelectMany(v => v.Value).Distinct().ToArray();
        }

        /// <summary>
        /// 获取用户关注表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int[] GetFollowings(int userId)
        {
            return _followingCache.Get(userId);
        }

        /// <summary>
        /// 获取用户关注表
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int[] GetFollowings(int[] userIds)
        {
            if (userIds.IsNullOrEmpty())
                return Array<int>.Empty;

            return _followingCache.GetRange(userIds).SelectMany(v => v.Value).Distinct().ToArray();
        }
    }
}
