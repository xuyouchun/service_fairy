using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.Watch.Components;

namespace ServiceFairy.Service.Watch
{
    /// <summary>
    /// 监控服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Watch, "1.0", "监控服务",
        category: AppServiceCategory.System, desc:"监控各个服务的运行情况，并及时报告集群的故障")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                ServiceWatchManager = new ServiceWatchManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 服务监控器
        /// </summary>
        public ServiceWatchManagerAppComponent ServiceWatchManager { get; private set; }
    }
}
