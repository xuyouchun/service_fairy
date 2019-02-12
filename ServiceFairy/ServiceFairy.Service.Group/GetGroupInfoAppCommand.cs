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
    /// 获取群组信息
    /// </summary>
    [AppCommand("GetGroupInfo", "获取群组信息", SecurityLevel = SecurityLevel.User), Remarks(Remarks), NewCommand]
    class GetGroupInfoAppCommand : ACS<Service>.Func<Group_GetGroupInfo_Request, Group_GetGroupInfo_Reply>
    {
        protected override Group_GetGroupInfo_Reply OnExecute(AppCommandExecuteContext<Service> context, Group_GetGroupInfo_Request request, ref ServiceResult sr)
        {
            GroupBasicInfo info = context.Service.GroupAccountManager.GetGroupBasicInfo(request.GroupId);
            return new Group_GetGroupInfo_Reply { Info = GroupInfo.FromBasicInfo(info) };
        }

        const string Remarks = @"要求用户必须为该群组的创建者，否则没有权限获取群组信息。";
    }
}
