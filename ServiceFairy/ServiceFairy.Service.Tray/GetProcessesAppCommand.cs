using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取进程列表
    /// </summary>
    [AppCommand("GetProcesses", title: "获取进程列表", SecurityLevel = SecurityLevel.Admin)]
    class GetProcessesAppCommand : ACS<Service>.Func<Tray_GetProcesses_Reply>
    {
        protected override Tray_GetProcesses_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            Service srv = context.Service;
            ProcessInfo[] infos = srv.ProcessManager.GetAllProcessInfos();
            return new Tray_GetProcesses_Reply() { ProcessInfos = infos };
        }
    }
}
