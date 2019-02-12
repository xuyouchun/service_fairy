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
    /// 添加群组成员
    /// </summary>
    [AppCommand("AddMembers", "批量添加群组成员", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class AddMembersAppCommand : ACS<Service>.Action<Group_AddMembers_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Group_AddMembers_Request request, ref ServiceResult sr)
        {
            UserSessionState uss = context.GetSessionState();
            int userId = uss.BasicInfo.UserId, groupId = request.GroupId;
            context.Service.EnsureCreatorOfGroup(userId, groupId);

            context.Service.GroupAccountManager.AddMembers(request.GroupId, new Users(request.Members), userId);
        }

        private const string Remarks = @"向已经建好的群组中添加成员，要求当前登录用户为群组的创建者，所添加的群组成员为与创建者为互粉关系。
所有本群组中的在线成员将会收到System.Group/GroupChanged消息";
    }
}
