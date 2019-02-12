using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Tray;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 启动新进程
    /// </summary>
    [AppCommand("StartProcesses", "启动新进程", SecurityLevel = SecurityLevel.Admin)]
    class StartProcessesAppCommand : ACS<Service>.Action<Tray_StartProcesses_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Tray_StartProcesses_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            context.Service.ProcessManager.StartProcesses(request.StartInfos);
        }
    }
}
