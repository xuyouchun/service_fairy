using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Cache;
using ServiceFairy.Service.Cache.Components;
using Common.Utility;
using Common;

namespace ServiceFairy.Service.Cache
{
    /// <summary>
    /// 将缓存值赋予一个增量
    /// </summary>
    [AppCommand("Increase", "将缓存值赋予一个增量", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class IncreaseAppCommand : ACS<Service>.Func<Cache_Increase_Request, Cache_Increase_Reply>
    {
        protected override Cache_Increase_Reply OnExecute(AppCommandExecuteContext<Service> context, Cache_Increase_Request req, ref ServiceResult sr)
        {
            RoutableCacheManager rm = context.Service.RouteManager;

            if (req.Items.IsNullOrEmpty())
                return new Cache_Increase_Reply() { Results = Array<CacheIncreaseResult>.Empty };

            return new Cache_Increase_Reply() { Results = rm.Increase(req.Items, req.EnableRoute) };
        }
    }
}
