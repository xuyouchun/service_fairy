using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 获取组信息
    /// </summary>
    [AppCommand("GetGroupInfos", "获取组信息", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetGroupInfosAppCommand : ACS<Service>.Func<UserCenter_GetGroupInfos_Request, UserCenter_GetGroupInfos_Reply>
    {
        protected override UserCenter_GetGroupInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_GetGroupInfos_Request req, ref ServiceResult sr)
        {
            GroupInfos[] infos = context.Service.GroupInfoManager.GetGroupInfos(req.GroupIds, req.Mask, req.Refresh);

            return new UserCenter_GetGroupInfos_Reply() { Infos = infos };
        }
    }
}
