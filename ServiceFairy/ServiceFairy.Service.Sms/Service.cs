using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.Sms.Components;

namespace ServiceFairy.Service.Sms
{
    /// <summary>
    /// Service
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Sms, "1.0", "短信服务",
        category: AppServiceCategory.System, weight: 30, desc:"发送短信")]
    class Service : SystemAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                this.SmsManager = new SmsManager(this),
            });
        }

        public SmsManager SmsManager { get; private set; }
    }
}