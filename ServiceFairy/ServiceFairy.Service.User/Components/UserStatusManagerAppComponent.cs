using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.User;
using Common.Utility;
using Common.Package;
using ServiceFairy.Entities.Message;
using Common.Contracts;
using ServiceFairy.Entities;
using ServiceFairy.SystemInvoke;
using Common.Framework.TrayPlatform;
using ServiceFairy.DbEntities.User;
using Common.Data;
using Common.Data.UnionTable;
using Common;

namespace ServiceFairy.Service.User.Components
{
    /// <summary>
    /// 用户状态管理器
    /// </summary>
    [AppComponent("用户状态管理器", "管理并分发用户的状态")]
    class UserStatusManagerAppComponent : TimerAppComponentBase
    {
        public UserStatusManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(1))
        {
            _service = service;
            _utConProvider = new RemoteUtConnectionProvider(_service.Invoker);
            _invoker = service.Invoker;
        }

        private readonly Service _service;
        private readonly SystemInvoker _invoker;
        private readonly IUtConnectionProvider _utConProvider;
        private readonly HashSet<int> _changedUserIds = new HashSet<int>();

        // 更新联系人的状态
        private void _UpdateUserStatus(UserSessionState uss, string status, string statusUrl)
        {
            DbUser user = new DbUser { UserId = uss.BasicInfo.UserId, Status = status, StatusUrl = statusUrl, StatusChangedTime = DateTime.UtcNow };
            user.Update(_utConProvider, new[] { DbUser.F_Status, DbUser.F_StatusUrl, DbUser.F_StatusChangedTime });
        }

        /// <summary>
        /// 设置用户的状态
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="status"></param>
        /// <param name="statusUrl"></param>
        public void SetStatus(UserSessionState uss, string status, string statusUrl)
        {
            // 发送通知
            try
            {
                int userId = uss.BasicInfo.UserId;
                _UpdateUserStatus(uss, status, statusUrl);
                _changedUserIds.SafeAdd(userId);
                _SendStatusChangedMessages(uss);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        // 将状态变化通知发送给我的粉丝
        private void _SendStatusChangedMessages(UserSessionState uss)
        {
            int sender = uss.BasicInfo.UserId;
            var entity = User_StatusChanged_Message.Create(sender);
            _service.MessageSender.SendToFollowers<User_StatusChanged_Message>(entity, sender, property: MsgProperty.Override);
        }

        protected override void OnExecuteTask(string taskName)
        {
            if (_changedUserIds.Count == 0)
                return;

            int[] userIds = _changedUserIds.SafeToArray(clear: true, trimExcess: true);
            _service.ServiceEvent.Raise(new User_StatusChanged_Event() { UserIds = userIds });
        }

        /// <summary>
        /// 获取所关注用户的状态
        /// </summary>
        /// <param name="uss">当前用户会话状态</param>
        /// <param name="userIds">用户ID</param>
        /// <returns></returns>
        public UserStatusInfo[] GetStatus(UserSessionState uss, int[] userIds)
        {
            if (userIds.IsNullOrEmpty())
                return Array<UserStatusInfo>.Empty;

            int userId = uss.BasicInfo.UserId;
#warning 查询用户状态的代码移到用户服务中，使用CacheChain
            return _invoker.UserCenter.GetUserStatusInfos(userIds);
        }
    }
}
