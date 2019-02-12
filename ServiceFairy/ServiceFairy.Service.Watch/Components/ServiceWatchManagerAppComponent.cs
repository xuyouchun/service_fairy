using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;

namespace ServiceFairy.Service.Watch.Components
{
    /// <summary>
    /// 服务监控器
    /// </summary>
    [AppComponent(title: "服务监控器", desc: "收集各个服务的监控信息")]
    class ServiceWatchManagerAppComponent : TimerAppComponentBase
    {
        public ServiceWatchManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;

        protected override void OnExecuteTask(string taskName)
        {
            
        }
    }
}
