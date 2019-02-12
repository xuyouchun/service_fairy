using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Service.Cache.Components;
using ServiceFairy.Entities.Cache;

namespace ServiceFairy.Service.Cache
{
    /// <summary>
    /// 删除缓存项
    /// </summary>
    [AppCommand("Remove", "删除缓存项", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class RemoveAppCommand : ACS<Service>.Action<Cache_Remove_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Cache_Remove_Request req, ref ServiceResult sr)
        {
            RoutableCacheManager rm = context.Service.RouteManager;

            rm.Remove(req.Keys, req.EnableRoute);
        }
    }
}
