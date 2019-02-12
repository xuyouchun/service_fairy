using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取所有服务
    /// </summary>
    [AppCommand("GetAllServices", "获取所有服务", SecurityLevel = SecurityLevel.Admin)]
    class GetAllServicesAppCommand : ACS<Service>.Func<Tray_GetAllServices_Reply>
    {
        protected override Tray_GetAllServices_Reply OnExecute(AppCommandExecuteContext<Service> context, ref Common.Contracts.Service.ServiceResult sr)
        {
            ServiceDesc[] services = context.Service.Context.Platform.GetAllServices();
            return new Tray_GetAllServices_Reply { Services = services };
        }
    }
}
