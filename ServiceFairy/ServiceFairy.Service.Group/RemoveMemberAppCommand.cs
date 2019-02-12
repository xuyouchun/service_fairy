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
    /// 删除群组成员
    /// </summary>
    [AppCommand("RemoveMember", "删除群组成员", SecurityLevel = SecurityLevel.User), Remarks(Remarks), NewCommand]
    class RemoveMemberAppCommand : ACS<Service>.Action<Group_RemoveMember_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Group_RemoveMember_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            int userId = context.GetUserId(), groupId = request.GroupId;
            context.Service.EnsureCreatorOfGroup(userId, groupId, errorMsg: "当前用户必须为该群组的成员");
            context.Service.EnsureMemberOfGroup(userId, groupId, errorMsg: "要删除的成员必须为该群组的成员");

            if (userId == request.MemberId)
            {
                context.Service.GroupAccountManager.ExitGroup(groupId, userId);
            }
            else
            {
                context.Service.GroupAccountManager.RemoveMember(groupId, request.MemberId);
            }
        }

        const string Remarks = @"所要删除的成员不允许为群组创建者，如果要该成员为当前登录用户，按“退出群组”来处理。
群组成员将会收到“Sys.Group/GroupChanged”消息，被删除的成员收到的消息Type为CurrentUserRemoved，其它成员收到的消息Type为MemberRemoved";
    }
}
