using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Group;
using Common.Contracts;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 批量删除群组成员
    /// </summary>
    [AppCommand("RemoveMembers", "批量删除群组成员", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class RemoveMembersAppCommand : ACS<Service>.Action<Group_RemoveMembers_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Group_RemoveMembers_Request request, ref ServiceResult sr)
        {
            int userId = context.GetSessionState().BasicInfo.UserId, groupId = request.GroupId;
            context.Service.EnsureCreatorOfGroup(userId, groupId);
            context.Service.GroupAccountManager.RemoveMembers(request.GroupId, new Users(request.Members));

#warning 如果要删除的成员中有群主，则按解散群来处理。
        }

        const string Remarks = @"删除群组成员，要求用户必须为该群组的成员。
群组成员将会收到Sys.Group/GroupChanged消息，被删除的成员收到的消息Type为CurrentUserRemoved，其它收到的消息其Type为MemberRemoved。";
    }
}
