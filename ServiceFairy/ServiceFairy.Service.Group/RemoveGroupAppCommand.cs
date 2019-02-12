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
    /// 删除群组
    /// </summary>
    [AppCommand("RemoveGroup", "删除群组", SecurityLevel = SecurityLevel.User), Remarks(Remarks), NewCommand]
    class RemoveGroupAppCommand : ACS<Service>.Action<Group_RemoveGroup_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Group_RemoveGroup_Request request, ref ServiceResult sr)
        {
            int userId = context.GetSessionState().BasicInfo.UserId, groupId = request.GroupId;
            context.Service.EnsureCreatorOfGroup(userId, groupId);
            context.Service.GroupAccountManager.RemoveGroup(request.GroupId);
        }

        const string Remarks = @"要求当前登录用户为该群组的创建者，否则无权限删除。";
    }
}
