using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 获取所有的在线用户
    /// </summary>
    [AppCommand("GetAllOnlineUsers", "获取所有的在线用户", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetAllOnlineUsersAppCommand : ACS<Service>.Func<UserCenter_GetAllOnlineUsers_Request, UserCenter_GetAllOnlineUsers_Reply>
    {
        protected override UserCenter_GetAllOnlineUsers_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_GetAllOnlineUsers_Request request, ref ServiceResult sr)
        {
            int[] userIds = context.Service.RoutableUserConnectionManager.GetAllOnlineUsers(request.EnableRoute);

            return new UserCenter_GetAllOnlineUsers_Reply() { UserIds = userIds };
        }
    }
}
