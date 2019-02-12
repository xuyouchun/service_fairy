using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Group;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Utility;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 批量获取群组信息
    /// </summary>
    [AppCommand("GetGroupInfos", "批量获取群组信息", SecurityLevel = SecurityLevel.User), Remarks(Remarks), NewCommand]
    class GetGroupInfosAppCommand : ACS<Service>.Func<Group_GetGroupInfos_Request, Group_GetGroupInfos_Reply>
    {
        protected override Group_GetGroupInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, Group_GetGroupInfos_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            GroupBasicInfo[] infos = context.Service.GroupAccountManager.GetGroupBasicInfos(request.GroupIds);
            return new Group_GetGroupInfos_Reply { Infos = infos.ToArray(info => GroupInfo.FromBasicInfo(info)) };
        }

        const string Remarks = @"要求用户必须为该群组的创建者，否则没有权限获取群组信息。";
    }
}
