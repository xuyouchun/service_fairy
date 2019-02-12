using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 获取用户的状态信息
    /// </summary>
    [AppCommand("GetUserStatusInfos", "获取用户的状态信息", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetUserStatusInfosAppCommand : ACS<Service>.Func<UserCenter_GetUserStatusInfos_Request, UserCenter_GetUserStatusInfos_Reply>
    {
        protected override UserCenter_GetUserStatusInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_GetUserStatusInfos_Request req, ref ServiceResult sr)
        {
            int[] userIds = context.Service.UserCollectionParser.Parse(req.Users);

            UserStatusInfo[] infos = context.Service.UserStatusManager.GetUserStatus(userIds);
            return new UserCenter_GetUserStatusInfos_Reply() { Infos = infos };
        }
    }
}
