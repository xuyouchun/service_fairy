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
    /// 添加群组成员
    /// </summary>
    [AppCommand("AddMember", "添加群组成员", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class AddMemberAppCommand : ACS<Service>.Action<Group_AddMember_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Group_AddMember_Request request, ref ServiceResult sr)
        {
            int userId = context.GetSessionState().BasicInfo.UserId, groupId = request.GroupId;
            context.Service.EnsureCreatorOfGroup(userId, groupId);

            context.Service.GroupAccountManager.AddMember(groupId, request.Member);
        }

        private const string Remarks = @"向已经建好的群组中添加成员，要求当前登录用户为群组的创建者，所添加的群组成员为与创建者为互粉关系。
所有本群组中的在线成员将会收到System.Group/GroupChanged消息";
    }
}
