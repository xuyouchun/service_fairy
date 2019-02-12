using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Cache;

namespace ServiceFairy.Service.Cache
{
    /// <summary>
    /// 批量设置缓存项
    /// </summary>
    [AppCommand("SetRange", "批量设置缓存项", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class SetRangeAppCommand : ACS<Service>.Action<Cache_SetRange_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Cache_SetRange_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            srv.RouteManager.Set(request.Items, request.EnableRoute);
        }
    }
}
