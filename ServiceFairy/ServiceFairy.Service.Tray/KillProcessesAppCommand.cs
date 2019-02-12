using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Tray;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 结束进程
    /// </summary>
    [AppCommand("KillProcesses", "结束进程", SecurityLevel = SecurityLevel.Admin)]
    class KillProcessesAppCommand : ACS<Service>.Action<Tray_KillProcesses_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Tray_KillProcesses_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            context.Service.ProcessManager.KillProcesses(request.ProcessIds);
        }
    }
}
