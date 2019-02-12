using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取系统日志的分组
    /// </summary>
    [AppCommand("GetSystemLogGroups", "获取系统日志的分组", SecurityLevel = SecurityLevel.Admin)]
    class GetSystemLogGroupsAppCommand : ACS<Service>.Func<Tray_GetSystemLogGroups_Reply>
    {
        protected override Tray_GetSystemLogGroups_Reply OnExecute(AppCommandExecuteContext<Service> context, ref Common.Contracts.Service.ServiceResult sr)
        {
            return new Tray_GetSystemLogGroups_Reply {
                Groups = context.Service.SystemLogManager.GetGroups(),
            };
        }
    }
}
