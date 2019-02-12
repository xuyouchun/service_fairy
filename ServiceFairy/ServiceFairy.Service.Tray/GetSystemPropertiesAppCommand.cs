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
    /// 获取系统属性
    /// </summary>
    [AppCommand("GetSystemProperties", "获取系统属性", SecurityLevel = SecurityLevel.Admin)]
    class GetSystemPropertiesAppCommand : ACS<Service>.Func<Tray_GetSystemProperties_Request, Tray_GetSystemProperties_Reply>
    {
        protected override Tray_GetSystemProperties_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_GetSystemProperties_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            var sysInfoMgr = context.Service.SystemInformationManager;
            SystemProperty[] properties;
            if (request.Names == null)
            {
                properties = sysInfoMgr.GetAllSystemProperties();
            }
            else
            {
                properties = request.Names.WhereNotNull(name => sysInfoMgr.GetSystemProperty(name)).ToArray();
            }

            return new Tray_GetSystemProperties_Reply { Properties = properties };
        }
    }
}
