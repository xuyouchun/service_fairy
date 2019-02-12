using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Service.Cache.Components;
using Common.Utility;
using ServiceFairy.Entities.Cache;

namespace ServiceFairy.Service.Cache
{
    /// <summary>
    /// 获取缓存项
    /// </summary>
    [AppCommand("Get", "获取缓存项", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class GetAppCommand : ACS<Service>.Func<Cache_Get_Request, Cache_Get_Reply>
    {
        protected override Cache_Get_Reply OnExecute(AppCommandExecuteContext<Service> context, Cache_Get_Request request, ref ServiceResult sr)
        {
            RoutableCacheManager rm = context.Service.RouteManager;

            CacheKeyValuePair[] pairs = rm.Get(new[] { request.Key }, request.Remove, request.EnableRoute);
            return new Cache_Get_Reply() {
                Data = pairs.IsNullOrEmpty() ? null : pairs[0].Data
            };
        }
    }
}
