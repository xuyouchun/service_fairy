using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 获取用户所属的组
    /// </summary>
    [AppCommand("GetUserGroups", "获取用户所属的组", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetUserGroupsAppCommand : ACS<Service>.Func<UserCenter_GetUserGroups_Request, UserCenter_GetUserGroups_Reply>
    {
        protected override UserCenter_GetUserGroups_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_GetUserGroups_Request req, ref ServiceResult sr)
        {
            int[] userIds = context.Service.UserCollectionParser.Parse(req.Users);
            UserGroupItem[] items = context.Service.GroupInfoManager.GetUserGroups(userIds, req.Refresh);

            return new UserCenter_GetUserGroups_Reply() { Items = items };
        }
    }
}
