using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取系统日志
    /// </summary>
    [AppCommand("GetSystemLogItems", "获取系统日志", SecurityLevel = SecurityLevel.Admin)]
    class GetSystemLogItemsAppCommand : ACS<Service>.Func<Tray_GetSystemLogItems_Request, Tray_GetSystemLogItems_Reply>
    {
        protected override Tray_GetSystemLogItems_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_GetSystemLogItems_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            int totalCount;
            SystemLogItem[] logItems = context.Service.SystemLogManager.GetLogs(request.Group, request.Start, request.Count, out totalCount);

            StringTable st = new StringTable();
            StSystemLogItem[] stLogItems = StSystemLogItem.From(logItems, st);

            return new Tray_GetSystemLogItems_Reply {
                LogItems = stLogItems, StringTable = st.ToStringArray(), TotalCount = totalCount,
            };
        }
    }
}
