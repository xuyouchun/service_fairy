using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;
using Common.Contracts.Log;
using Common.Contracts;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取指定组的日志
    /// </summary>
    [AppCommand("GetLocalLogItems", "获取指定组的日志", SecurityLevel = SecurityLevel.Admin)]
    class GetLocalLogItemsAppCommand : ACS<Service>.Func<Tray_GetLocalLogItems_Request, Tray_GetLocalLogItems_Reply>
    {
        protected override Tray_GetLocalLogItems_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_GetLocalLogItems_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            LogItem[] logItems = srv.LocalLogManager.GetLocalLogItems(req.Group);

            StringTable st = new StringTable();
            StLogItem[] stLogItems = StLogItem.From(logItems, st);

            return new Tray_GetLocalLogItems_Reply() { LogItems = stLogItems, StringTable = st.ToStringArray() };
        }
    }
}
