using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Cache;
using ServiceFairy.Service.Cache.Components;

namespace ServiceFairy.Service.Cache
{
    /// <summary>
    /// 批量获取缓存项
    /// </summary>
    [AppCommand("GetRange", "批量获取缓存项", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class GetRangeAppCommand : ACS<Service>.Func<Cache_GetRange_Request, Cache_GetRange_Reply>
    {
        protected override Cache_GetRange_Reply OnExecute(AppCommandExecuteContext<Service> context, Cache_GetRange_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            RoutableCacheManager rm = context.Service.RouteManager;

            return new Cache_GetRange_Reply() {
                Datas = rm.Get(request.Keys, request.Remove, request.EnableRoute)
            };
        }
    }
}
