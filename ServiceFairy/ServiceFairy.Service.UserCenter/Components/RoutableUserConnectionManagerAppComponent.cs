using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.UserCenter;
using System.Diagnostics.Contracts;
using Common.Framework.TrayPlatform;
using Common.Utility;

namespace ServiceFairy.Service.UserCenter.Components
{
    [AppComponent("支持路由功能的用户在线状态管理器")]
    class RoutableUserConnectionManagerAppComponent : AppComponent
    {
        public RoutableUserConnectionManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 保持用户的订阅状态
        /// </summary>
        /// <param name="conInfos"></param>
        /// <param name="enableRoute"></param>
        public void KeepUserConnection(UserConnectionInfo[] conInfos, bool enableRoute)
        {
            Contract.Requires(conInfos != null);

            if (!enableRoute)
            {
                _service.UserConnectionManager.KeepUserConnection(conInfos);
            }
            else
            {
                _service.UserRouter.Apply(conInfos,
                    localConInfos => _service.UserConnectionManager.KeepUserConnection(conInfos),
                    (remoteClientId, remoteConInfos) => _service.Invoker.UserCenter.KeepUserConnection(remoteConInfos, false, 
                        CallingSettings.FromTarget(remoteClientId, CommunicateInvokeType.OneWay)),
                    conInfo => conInfo.UserId
                );
            }
        }

        /// <summary>
        /// 用户断开连接通知
        /// </summary>
        /// <param name="conInfos"></param>
        /// <param name="enableRoute"></param>
        public void UserDisconnectedNotify(UserDisconnectedInfo[] conInfos, bool enableRoute)
        {
            Contract.Requires(conInfos != null);

            if (!enableRoute)
            {
                _service.UserConnectionManager.UserDisconnectedNotify(conInfos);
            }
            else
            {
                _service.UserRouter.Apply(conInfos,
                    localConInfos => _service.UserConnectionManager.UserDisconnectedNotify(conInfos),
                    (remoteClientId, remoteConInfos) => _service.Invoker.UserCenter.UserDisconnectedNotify(remoteConInfos, false,
                        CallingSettings.FromTarget(remoteClientId, CommunicateInvokeType.OneWay)),
                    conInfo => conInfo.UserId
                );
            }
        }

        /// <summary>
        /// 获取用户的连接信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="enableRoute"></param>
        /// <returns></returns>
        public UserConnectionInfo[] GetUserConnectionInfos(int[] userIds, bool enableRoute)
        {
            Contract.Requires(userIds != null);

            if (!enableRoute)
            {
                return _service.UserConnectionManager.GetUserConnectionInfos(userIds);
            }
            else
            {
                return _service.UserRouter.Collect(userIds,
                    localUserIds => _service.UserConnectionManager.GetUserConnectionInfos(localUserIds),
                    (remoteClientId, remoteUserIds) => _service.Invoker.UserCenter.GetUserConnectionInfos(remoteUserIds, false, CallingSettings.FromTarget(remoteClientId, CommunicateInvokeType.OneWay))
                );
            }
        }

        /// <summary>
        /// 获取所有的在线用户
        /// </summary>
        /// <returns></returns>
        public int[] GetAllOnlineUsers(bool enableRoute)
        {
            if (!enableRoute)
            {
                return _service.UserConnectionManager.GetAllOnlineUsers();
            }
            else
            {
                return _service.UserRouter.Collect<int>(
                    localLoader: _service.UserConnectionManager.GetAllOnlineUsers,
                    remoteLoader: (clientId) => _service.Invoker.UserCenter.GetAllOnlineUsers(false, CallingSettings.FromTarget(clientId))
                );
            }
        }

        /// <summary>
        /// 获取指定用户所在的终端
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="enableRoute"></param>
        /// <returns></returns>
        public UserPosition[] GetUserPositions(int[] userIds, bool enableRoute)
        {
            Contract.Requires(userIds != null);

            if (!enableRoute)
            {
                var list = from uc in _service.UserConnectionManager.GetUserConnectionInfos(userIds)
                           group uc.UserId by uc.ClientId into g
                           select new UserPosition { ClientId = g.Key, UserIds = g.ToArray() };

                return list.ToArray();
            }
            else
            {
                IDictionary<Guid, UserPosition[]> dict = _service.UserRouter.CollectDict<int, UserPosition>(userIds,
                    localLoader: localUserIds => GetUserPositions(localUserIds, false),
                    remoteLoader: (remoteClientId, remoteUserIds) => _service.Invoker.UserCenter.GetUserPositions(remoteUserIds, false, CallingSettings.FromTarget(remoteClientId))
                );

                var list = from up in dict.Values.SelectMany()
                           group up.UserIds by up.ClientId into g
                           select new UserPosition { ClientId = g.Key, UserIds = g.SelectMany().ToArray() };

                return list.ToArray();
            }
        }

        /// <summary>
        /// 选取在线用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int[] SelectOnlineUsers(int[] userIds)
        {
            Contract.Requires(userIds != null);

            return GetUserPositions(userIds, true).SelectMany(up => up.UserIds).ToArray();
        }

        /// <summary>
        /// 判断用户是否在线
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsOnline(int userId)
        {
            return !SelectOnlineUsers(new[] { userId }).IsNullOrEmpty();
        }
    }
}
