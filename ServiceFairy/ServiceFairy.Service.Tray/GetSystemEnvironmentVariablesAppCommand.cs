using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;
using Common.Utility;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 读取环境变量
    /// </summary>
    [AppCommand("GetSystemEnvironmentVariables", "读取环境变量", SecurityLevel = SecurityLevel.Admin)]
    class GetSystemEnvironmentVariablesAppCommand : ACS<Service>.Func<Tray_GetSystemEnvironmentVariables_Request, Tray_GetSystemEnvironmentVariables_Reply>
    {
        protected override Tray_GetSystemEnvironmentVariables_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_GetSystemEnvironmentVariables_Request request, ref ServiceResult sr)
        {
            var sysInfoMgr = context.Service.SystemInformationManager;
            SystemEnvironmentVariable[] values;
            if (request.Names == null)
            {
                values = sysInfoMgr.GetAllEnvironmentVariables();
            }
            else
            {
                values = request.Names.WhereNotNull(name => sysInfoMgr.GetEnvironmentVariable(name)).ToArray();
            }

            return new Tray_GetSystemEnvironmentVariables_Reply { Variables = values };
        }
    }
}
