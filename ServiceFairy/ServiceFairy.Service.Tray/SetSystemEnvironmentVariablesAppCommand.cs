using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 设置环境变量
    /// </summary>
    [AppCommand("SetSystemEnvironmentVariables", "设置环境变量", SecurityLevel = SecurityLevel.Admin)]
    class SetSystemEnvironmentVariablesAppCommand : ACS<Service>.Action<Tray_SetSystemEnvironmentVariables_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Tray_SetSystemEnvironmentVariables_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            foreach (SystemEnvironmentVariable variable in request.Variables)
            {
                context.Service.SystemInformationManager.SetEnvironmentVariable(variable.Name, variable.Value);
            }
        }
    }
}
