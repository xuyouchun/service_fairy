using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;
using Common.Contracts.Log;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取本地日志组信息
    /// </summary>
    [AppCommand("GetLocalLogGroups", "获取本地日志组信息", SecurityLevel = SecurityLevel.Admin)]
    class GetLocalLogGroupsAppCommand : ACS<Service>.Func<Tray_GetLocalLogGroups_Request, Tray_GetLocalLogGroups_Reply>
    {
        protected override Tray_GetLocalLogGroups_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_GetLocalLogGroups_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            LogItemGroup[] groups = srv.LocalLogManager.GetLocalLogGroups(req.ParentGroup);

            return new Tray_GetLocalLogGroups_Reply() { Groups = groups };
        }
    }
}
