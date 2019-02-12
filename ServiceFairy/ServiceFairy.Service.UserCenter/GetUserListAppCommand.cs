using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 获取用户列表
    /// </summary>
    [AppCommand("GetUserList", "获取用户列表", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetUserListAppCommand : ACS<Service>.Func<UserCenter_GetUserList_Request, UserCenter_GetUserList_Reply>
    {
        protected override UserCenter_GetUserList_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_GetUserList_Request req, ref ServiceResult sr)
        {
            UserIdName[] userInfos = context.Service.UserListManager.GetUserList(req.Start, req.Count, req.Order);

            return new UserCenter_GetUserList_Reply { Users = userInfos };
        }
    }
}
