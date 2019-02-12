using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Group;
using Common.Contracts.Service;
using ServiceFairy.Service.Group.Components;
using Common.Contracts;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 获取群组成员
    /// </summary>
    [AppCommand("GetMembers", "获取群组成员", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class GetMembersAppCommand : ACS<Service>.Func<Group_GetMembers_Request, Group_GetMembers_Reply>
    {
        protected override Group_GetMembers_Reply OnExecute(AppCommandExecuteContext<Service> context, Group_GetMembers_Request request, ref ServiceResult sr)
        {
            // 确认身份
            GroupAccountManagerAppComponent gMgr = context.Service.GroupAccountManager;
            UserSessionState uss = context.GetSessionState();
            context.Service.EnsureMemberOfGroup(uss.BasicInfo.UserId, request.GroupId);

            GroupBasicInfo gInfo = gMgr.GetGroupBasicInfo(request.GroupId);
            if (gInfo.ChangedTime.Ticks == request.Version)
                return new Group_GetMembers_Reply();

            // 获取群组成员
            int[] members = gMgr.GetMembers(request.GroupId);

            // 转换
            return new Group_GetMembers_Reply {
                MemberIds = members, Version = gInfo.ChangedTime.Ticks,
            };
        }

        const string Remarks = @"获取指定群组的成员。
要求用户必须为该群组的成员，才会有权限进行该操作";
    }
}
