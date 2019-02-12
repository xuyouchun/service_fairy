using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;
using ServiceFairy.Entities.Group;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 获取我的所有群组ID
    /// </summary>
    [AppCommand("GetMyGroups", "获取我的所有群组ID", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class GetMyGroupsAppCommand : ACS<Service>.Func<Group_GetMyGroups_Reply>
    {
        protected override Group_GetMyGroups_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            int userId = context.GetSessionState().BasicInfo.UserId;
            int[] groupIds = context.Service.GroupManager.GetUserGroups(userId);

            return new Group_GetMyGroups_Reply { GroupIds = groupIds };
        }

        const string Remarks = @"仅返回群组ID，如果要查看群组信息，需要调用GetGroupInfo或GetGroupInfos接口";
    }
}
