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
    /// 获取我的所有群组信息
    /// </summary>
    [AppCommand("GetMyGroupInfos", "获取我的所有群组信息", SecurityLevel = SecurityLevel.User), Remarks(Remarks), NewCommand]
    class GetMyGroupInfosAppCommand : ACS<Service>.Func<Group_GetMyGroupInfos_Reply>
    {
        protected override Group_GetMyGroupInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            GroupBasicInfo[] infos = context.Service.GroupAccountManager.SafeGetMyGroupBasicInfos(context.GetSessionState());
            return new Group_GetMyGroupInfos_Reply { Infos = infos.Where(info => info.Enable).ToArray(info => GroupInfo.FromBasicInfo(info)) };
        }

        const string Remarks = @"";
    }
}
