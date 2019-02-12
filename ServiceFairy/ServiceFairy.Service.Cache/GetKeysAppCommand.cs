using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Service.Cache.Components;
using ServiceFairy.Entities.Cache;

namespace ServiceFairy.Service.Cache
{
    /// <summary>
    /// 获取缓存键，用于确定哪些键存在
    /// </summary>
    [AppCommand("GetKeys", "获取缓存键，用于确定哪些键存在", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class GetKeysAppCommand : ACS<Service>.Func<Cache_GetKeys_Request, Cache_GetKeys_Reply>
    {
        protected override Cache_GetKeys_Reply OnExecute(AppCommandExecuteContext<Service> context, Cache_GetKeys_Request req, ref ServiceResult sr)
        {
            RoutableCacheManager rm = context.Service.RouteManager;

            if (req.Keys.IsNullOrEmpty())
                return new Cache_GetKeys_Reply() { Keys = new string[0] };

            return new Cache_GetKeys_Reply() { Keys = rm.GetKeys(req.Keys) };
        }
    }
}
