using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Components;
using Common.Contracts.Service;
using ServiceFairy.Service.Queue.Components;

namespace ServiceFairy.Service.Queue
{
    /// <summary>
    /// 队列服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Queue, "1.0", "队列服务",
        category: AppServiceCategory.System, desc:"对实时性要求不高的大量并发请求提供缓冲的时间")]
    class Service : SystemAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                TaskManager = new TaskManager(this),
            });
        }

        /// <summary>
        /// 任务管理器　
        /// </summary>
        public TaskManager TaskManager { get; private set; }
    }
}
