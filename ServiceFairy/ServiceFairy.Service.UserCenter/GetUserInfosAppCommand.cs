using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    [AppCommand("GetUserInfos", "获取用户的信息", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetUserInfosAppCommand : ACS<Service>.Func<UserCenter_GetUserInfos_Request, UserCenter_GetUserInfos_Reply>
    {
        protected override UserCenter_GetUserInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_GetUserInfos_Request req, ref ServiceResult sr)
        {
            UserInfos[] infos = context.Service.UserInfoManager.GetUserInfos(req.Mask, req.Users, req.Refresh);
            return new UserCenter_GetUserInfos_Reply() { Infos = infos };
        }
    }
}
