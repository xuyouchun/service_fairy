using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 分布式控制器
    /// </summary>
    [AppComponent("分布式控制器", "控制多个服务的协调工作", AppComponentCategory.System, "Sys_DistributedController")]
    public class DistributedControllerAppComponent : TimerAppComponentBase
    {
        public DistributedControllerAppComponent(SystemAppServiceBase service)
            : base(service, TimeSpan.FromSeconds(5))
        {

        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }
    }
}
