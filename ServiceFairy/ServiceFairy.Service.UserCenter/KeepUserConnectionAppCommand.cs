using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 保持用户的消息订阅状态
    /// </summary>
    [AppCommand("KeepUserConnection", "保持用户连接状态", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class KeepUserConnectionAppCommand : ACS<Service>.Action<UserCenter_KeepUserConnection_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, UserCenter_KeepUserConnection_Request req, ref ServiceResult sr)
        {
            context.Service.RoutableUserConnectionManager.KeepUserConnection(req.ConnectionInfos, req.EnableRoute);
        }
    }
}
