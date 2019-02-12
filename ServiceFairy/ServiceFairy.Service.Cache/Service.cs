using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Service.Cache.Components;
using Common.Package.Service;
using ServiceFairy.Components;
using System.IO;

namespace ServiceFairy.Service.Cache
{
    /// <summary>
    /// 分布式缓存
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Cache, "1.0", "缓存服务",
        category: AppServiceCategory.System, desc: "分布式缓存系统")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            AppComponentManager.AddRange(new IAppComponent[] {
                CacheManager = new CacheManagerAppComponent(this),
                ServiceConsistentNodeManager = new ServiceConsistentNodeManagerAppComponent(this),
                RouteManager = new RoutableCacheManager(this),
            });
        }

        protected override void OnStart()
        {
            base.OnStart();

            Settings.CacheBasePath = Context.ConfigReader.Get("cache_base_path", Path.Combine(ServiceDataPath, "cache_value"));
        }

        /// <summary>
        /// 缓存管理器
        /// </summary>
        public CacheManagerAppComponent CacheManager { get; private set; }

        /// <summary>
        /// 基于一致性哈希算法的数据路由管理器
        /// </summary>
        public ServiceConsistentNodeManagerAppComponent ServiceConsistentNodeManager { get; private set; }

        /// <summary>
        /// 路由管理器
        /// </summary>
        public RoutableCacheManager RouteManager { get; private set; }
    }
}
