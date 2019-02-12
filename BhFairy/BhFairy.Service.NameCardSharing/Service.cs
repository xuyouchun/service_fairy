using System;
using System.Collections.Generic;
using System.Text;
using ServiceFairy.Components;
using Common.Contracts.Service;
using ServiceFairy;
using BhFairy.Service.NameCardSharing.Components;
using BhFairy.Components;

namespace BhFairy.Service.NameCardSharing
{
    [AppEntryPoint, AppService(BhNames.ServiceNames.NameCardSharing, "1.0", "名片交换", desc: "收集用户的名片并分享给指定范围之内的其它用户")]
    class Service : BhAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                MyComponent = new MyComponent(this),
            });
        }

        public MyComponent MyComponent { get; private set; }
    }
}