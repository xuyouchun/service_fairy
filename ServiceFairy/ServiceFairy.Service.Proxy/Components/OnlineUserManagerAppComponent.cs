using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using Common.Framework.TrayPlatform;
using Common.Utility;
using Common;

namespace ServiceFairy.Service.Proxy.Components
{
    /// <summary>
    /// 在线用户管理器
    /// </summary>
    [AppComponent("在线用户管理器", "向用户中心保持在线用户的状态")]
    class OnlineUserManagerAppComponent : TimerAppComponentBase
    {
        public OnlineUserManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(1))
        {
            _service = service;
        }

        private readonly Service _service;
        private DateTime _lastFullUpdate;

        /// <summary>
        /// 获取所有的在线用户
        /// </summary>
        /// <returns></returns>
        public UserConnectionInfo[] GetAllOnlineUsers()
        {
            return _service.Context.SessionStateManager.GetAllConnectedUsers() ?? Array<UserConnectionInfo>.Empty;
        }

        protected override void OnExecuteTask(string taskName)
        {
            // 所有在线用户
            DateTime now = DateTime.UtcNow;
            UserConnectionInfo[] conInfos;
            if (now - _lastFullUpdate >= TimeSpan.FromSeconds(10))  // 超过10秒钟便全部同步
            {
                conInfos = GetAllOnlineUsers();
                _lastFullUpdate = now;
            }
            else
            {
                conInfos = _service.Context.SessionStateManager.GetConnectionUsersByLastAccessTime(now - TimeSpan.FromSeconds(10), now);
            }

            if (conInfos.Length > 0)
            {
                conInfos.Split(500).ForEach(infos => {
                    InvokeNoThrow(() => _service.Invoker.UserCenter.KeepUserConnection(infos.ToArray()));
                });
            }

            // 所有断开连接的用户
            UserDisconnectedInfo[] disconnectedConInfos = _service.Context.SessionStateManager.GetDisconnectedInfos();
            if (!disconnectedConInfos.IsNullOrEmpty())
            {
                disconnectedConInfos.Split(500).ForEach(infos => {
                    InvokeNoThrow(() => _service.Invoker.UserCenter.UserDisconnectedNotify(infos.ToArray()));
                });
            }
        }
    }
}
