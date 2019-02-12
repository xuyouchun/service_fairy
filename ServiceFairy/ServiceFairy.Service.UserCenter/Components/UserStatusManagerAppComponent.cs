using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Package;
using Common.Utility;
using ServiceFairy.DbEntities.User;
using Common.Data;
using ServiceFairy.SystemInvoke;
using CacheChain = Common.Package.CacheChain<int, Common.Contracts.Service.UserStatusInfo>;
using ServiceFairy.Entities.UserCenter;
using ServiceFairy.Entities.User;
using ServiceFairy.Components;
using Common.Data.UnionTable;
using ServiceFairy.DbEntities;

namespace ServiceFairy.Service.UserCenter.Components
{
    /// <summary>
    /// 维护用户的状态信息
    /// </summary>
    [AppComponent("用户状态信息管理器", "维护用户的状态信息")]
    class UserStatusManagerAppComponent : TimerAppComponentBase
    {
        public UserStatusManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;

            _utConProvider = new RemoteUtConnectionProvider(_service.Invoker, DbEntityUtility.LoadReviseInfo());
            _cache = new CacheChain(new ICacheChainNode<int, UserStatusInfo>[] {
                CacheChain.CreateMemoryCacheNode(TimeSpan.FromSeconds(5)),
                new DistribuedCacheChainNode<int, UserStatusInfo>(_service.Invoker, TimeSpan.FromHours(1), "s_st_"),
                CacheChain.CreateLoaderNode(_LoadUserStatus),
            });
        }

        private readonly Service _service;
        private readonly IUtConnectionProvider _utConProvider;
        private readonly CacheChain _cache;

        private KeyValuePair<int, UserStatusInfo>[] _LoadUserStatus(int[] userIds, bool refresh)
        {
            DbUser[] users = DbUser.SelectIn(_utConProvider, DbUser.F_UserId, userIds,
                null, new[] { DbUser.F_UserId, DbUser.F_Status, DbUser.F_StatusUrl, DbUser.F_StatusChangedTime });

            return users.ToArray(user => new KeyValuePair<int, UserStatusInfo>(
                user.UserId, new UserStatusInfo { UserId = user.UserId, Status = user.Status, StatusUrl = user.StatusUrl, ChangedTime = user.StatusChangedTime }
            ));
        }

        /// <summary>
        /// 获取用户的状态
        /// </summary>
        /// <param name="userIds">用户ID</param>
        /// <returns></returns>
        public UserStatusInfo[] GetUserStatus(int[] userIds)
        {
            Contract.Requires(userIds != null);

            //UserPosition[] positions = _service.RoutableUserConnectionManager.GetUserPositions(userIds);
            HashSet<int> onlineUsers = _service.RoutableUserConnectionManager.SelectOnlineUsers(userIds).ToHashSet();

            var list = from item in _cache.GetRange(userIds)
                       let userId = item.Key
                       let status = item.Value
                       let online = onlineUsers.Contains(userId)
                       select new UserStatusInfo {
                           UserId = userId, ChangedTime = status.ChangedTime,
                           StatusUrl = status.StatusUrl, Status = status.Status, Online = online
                       };

            return list.ToArray();
        }

        /// <summary>
        /// 获取用户的状态
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public UserStatusInfo[] GetUserStatus(Users users)
        {
            Contract.Requires(users != null);

            return GetUserStatus(_service.UserCollectionParser.Parse(users));
        }

        /// <summary>
        /// 获取用户的状态
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public UserStatusInfo[] GetUserStatus(string users)
        {
            Contract.Requires(users != null);

            return GetUserStatus(new Users(users));
        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            base.OnStatusChanged(status);

            _service.ServiceEvent.Switch<User_StatusChanged_Event>(_OnUserStatusChanged, status);
        }

        // 用户状态变化时清除缓存
        private void _OnUserStatusChanged(object sender, ServiceEventArgs<User_StatusChanged_Event> e)
        {
            User_StatusChanged_Event entity = e.Entity;

            if (!entity.UserIds.IsNullOrEmpty())
            {
                _cache.RemoveRange(entity.UserIds);
            }
        }
    }
}
