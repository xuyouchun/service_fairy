using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.UserCenter;
using Common.Contracts.Service;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 用户连接断开通知
    /// </summary>
    [AppCommand("UserDisconnectedNotify", "用户连接断开通知", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class UserDisconnectedNotifyAppCommand : ACS<Service>.Action<UserCenter_UserDisconnectedNotify_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, UserCenter_UserDisconnectedNotify_Request req, ref ServiceResult sr)
        {
            context.Service.RoutableUserConnectionManager.UserDisconnectedNotify(req.DisconnectionInfos, req.EnableRoute);
        }
    }
}
