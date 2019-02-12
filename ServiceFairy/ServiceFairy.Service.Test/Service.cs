using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Service.Test.Components;
using ServiceFairy.Components;
using ServiceFairy.Service.Test.Addins;

namespace ServiceFairy.Service.Test
{
    /// <summary>
    /// 用于场景模拟的测试服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Test, "1.0", "测试服务",
        category: AppServiceCategory.System, desc: "用于集群功能及性能测试")]
    class Service : SystemAppServiceBase
    {
        public Service()
        {

        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                TestTaskManager = new TestTaskManagerAppComponent(this),
                MyTest = new MyTestAppComponent(this),
            });

            ServiceAddIn.Register(SFNames.ServiceNames.Security, new MyTestAddin(this));
        }

        /// <summary>
        /// 测试管理器
        /// </summary>
        public TestTaskManagerAppComponent TestTaskManager { get; private set; }

        /// <summary>
        /// 临时测试组件
        /// </summary>
        public MyTestAppComponent MyTest { get; private set; }
    }
}
