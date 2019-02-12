using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Log;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取指定时间范围之内的日志
    /// </summary>
    [AppCommand("GetLocalLogItemsByTime", "获取指定时间范围之内的日志", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetLocalLogItemsByTimeAppCommand : ACS<Service>.Func<Tray_GetLocalLogItemsByTime_Request, Tray_GetLocalLogItemsByTime_Reply>
    {
        protected override Tray_GetLocalLogItemsByTime_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_GetLocalLogItemsByTime_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            LogItem[] items = context.Service.LocalLogManager.GetLocalLogItemsByTime(request.StartTime, request.EndTime, request.MaxCount);

            StringTable st = new StringTable();
            StLogItem[] stLogItems = StLogItem.From(items, st);

            return new Tray_GetLocalLogItemsByTime_Reply { LogItems = stLogItems, StringTable = st.ToStringArray() };
        }
    }
}
