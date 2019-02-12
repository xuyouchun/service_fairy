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
    /// 批量删除群组
    /// </summary>
    [AppCommand("RemoveGroups", "批量删除群组", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class RemoveGroupsAppCommand : ACS<Service>.Action<Group_RemoveGroups_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Group_RemoveGroups_Request request, ref ServiceResult sr)
        {
            int userId = context.GetSessionState().BasicInfo.UserId;
            int[] groupIds = request.GroupIds;

            context.Service.EnsureCreatorOfGroups(userId, groupIds);
            context.Service.GroupAccountManager.RemoveGroups(groupIds);
        }

        const string Remarks = @"要求当前登录用户为该群组的创建者，否则无权限删除。";
    }
}
