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
    /// 修改群组信息
    /// </summary>
    [AppCommand("ModifyGroupInfo", "修改群组信息", SecurityLevel = SecurityLevel.User), Remarks(Remarks), NewCommand]
    class ModifyGroupInfoAppCommand : ACS<Service>.Action<Group_ModifyGroupInfo_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Group_ModifyGroupInfo_Request request, ref ServiceResult sr)
        {
            int userId = context.GetSessionState().BasicInfo.UserId, groupId = request.GroupId;
            context.Service.EnsureCreatorOfGroup(userId, groupId, errorMsg: "群组的创建者才有权限修改群组信息");

            context.Service.GroupAccountManager.ModifyGroupInfo(groupId, request.Items);
        }

        private const string Remarks = @"要求当前登录用户必须为该群组的创建者。";
    }
}
