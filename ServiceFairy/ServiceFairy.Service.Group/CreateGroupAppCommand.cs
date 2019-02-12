using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Contracts.Service;
using ServiceFairy.Entities.Group;
using Common.Contracts;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 创建群组
    /// </summary>
    [AppCommand("CreateGroup", "创建群组", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class CreateGroupAppCommand : ACS<Service>.Func<Group_CreateGroup_Request, Group_CreateGroup_Reply>
    {
        protected override Group_CreateGroup_Reply OnExecute(AppCommandExecuteContext<Service> context, Group_CreateGroup_Request req, ref ServiceResult sr)
        {
            int groupId = context.Service.GroupAccountManager.CreateGroup(
                context.GetSessionState().BasicInfo.UserId,
                req.Name, req.Details);

            return new Group_CreateGroup_Reply() { GroupId = groupId };
        }

        const string Remarks = @"创建群组，每个群组都有一个整型的唯一标识，创建成功之后将会返回该标识。";
    }
}
