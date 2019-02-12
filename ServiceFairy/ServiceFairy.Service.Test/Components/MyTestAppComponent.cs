using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using System.Threading;
using Common.Package;

namespace ServiceFairy.Service.Test.Components
{
    [AppComponent("临时测试组件", "临时测试用")]
    class MyTestAppComponent : TimerAppComponentBase
    {
        public MyTestAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;

        protected override void OnExecuteTask(string taskName)
        {
            /*
            while (true)
            {
                Thread.Sleep(1000);

                LogManager.LogMessage("YEAH! - " + DateTime.UtcNow);
            }*/
        }
    }
}
