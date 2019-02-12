using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Group;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 用户退出群组
    /// </summary>
    [AppCommand("ExitGroup", "退出群组", SecurityLevel = SecurityLevel.User), Remarks(Remarks), NewCommand]
    class ExitGroupAppCommand : ACS<Service>.Action<Group_ExitGroup_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Group_ExitGroup_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            int userId = context.GetUserId();
            int groupId = request.GroupId;

#warning 如果当前登录用户为群主，则按解散群组来处理
            context.Service.EnsureMemberOfGroup(userId, groupId, errorMsg: "退出群组的用户，必须为该群组的成员");
            context.Service.EnsureNotCreatorOfGroup(userId, groupId, errorMsg: "退出群组的用户，不能为该群组的创建者");

            context.Service.GroupAccountManager.ExitGroup(groupId, userId);
        }

        const string Remarks = @"要求当前登录用户为群组的成员，并且不能为创建者。";
    }
}
