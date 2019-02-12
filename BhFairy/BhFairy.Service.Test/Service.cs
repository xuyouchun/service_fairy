using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BhFairy.Service.Test.Components;
using Common.Contracts.Service;
using ServiceFairy.Components;
using BhFairy.Components;

namespace BhFairy.Service.Test
{
    /// <summary>
    /// 测试服务
    /// </summary>
    [AppEntryPoint, AppService("Application.Test", "1.0.0.0", "测试服务", "用于压力测试")]
    class Service : BhAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                TestAppComponent = new TestAppComponent(this),
            });
        }

        /// <summary>
        /// 测试组件
        /// </summary>
        public TestAppComponent TestAppComponent { get; private set; }
    }
}
