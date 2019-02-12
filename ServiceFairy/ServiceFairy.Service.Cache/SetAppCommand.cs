using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Service.Cache.Components;
using Common.Utility;
using ServiceFairy.Entities.Cache;

namespace ServiceFairy.Service.Cache
{
    /// <summary>
    /// 设置缓存
    /// </summary>
    [AppCommand("Set", "设置缓存项", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class SetAppCommand : ACS<Service>.Action<Cache_Set_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Cache_Set_Request request, ref ServiceResult sr)
        {
            CacheItem item = new CacheItem { Key = request.Key, Data = request.Data, Expired = request.Expired };
            context.Service.RouteManager.Set(new[] { item }, request.EnableRoute);
        }
    }
}
