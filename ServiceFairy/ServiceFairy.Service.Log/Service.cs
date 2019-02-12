using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using ServiceFairy.Components;
using ServiceFairy.Service.Log.Components;

namespace ServiceFairy.Service.Log
{
    /// <summary>
    /// Service
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Log, "1.0", "日志服务",
        category: AppServiceCategory.System, weight:40, desc:"收集并分析各个服务产生的日志")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[]{
                LogAnalyzer = new LogAnalyzerAppComponent(this),
                LogStorage = new LogStorageAppComponent(this),
                LogCollector = new LogCollectorAppComponent(this),
            });
        }

        /// <summary>
        /// 日志分析器
        /// </summary>
        public LogAnalyzerAppComponent LogAnalyzer { get; private set; }

        /// <summary>
        /// 日志存储器
        /// </summary>
        public LogStorageAppComponent LogStorage { get; private set; }

        /// <summary>
        /// 日志收集器
        /// </summary>
        public LogCollectorAppComponent LogCollector { get; private set; }
    }
}
