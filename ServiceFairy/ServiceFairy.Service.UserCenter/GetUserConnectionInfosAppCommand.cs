using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.UserCenter;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 获取用户的连接信息
    /// </summary>
    [AppCommand("GetUserConnectionInfos", "获取用户的连接信息", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetUserConnectionInfosAppCommand : ACS<Service>.Func<UserCenter_GetUserConnectionInfos_Request, UserCenter_GetUserConnectionInfos_Reply>
    {
        protected override UserCenter_GetUserConnectionInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_GetUserConnectionInfos_Request req, ref ServiceResult sr)
        {
            UserConnectionInfo[] conInfos = context.Service.RoutableUserConnectionManager.GetUserConnectionInfos(req.UserIds, req.EnableRoute);
            return new UserCenter_GetUserConnectionInfos_Reply() { Infos = conInfos };
        }
    }
}
