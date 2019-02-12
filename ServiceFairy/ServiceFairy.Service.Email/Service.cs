using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Components;
using Common.Contracts.Service;
using ServiceFairy.Service.Email.Components;

namespace ServiceFairy.Service.Email
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Email, "1.0", "邮件服务",
        category: AppServiceCategory.System, desc:"收发电子邮件")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                EmailPool = new EmailPoolAppComponent(this),
                EmailSender = new EmailSenderAppComponent(this),
            });
        }

        /// <summary>
        /// 邮件池
        /// </summary>
        public EmailPoolAppComponent EmailPool { get; private set; }

        /// <summary>
        /// 邮件发送器
        /// </summary>
        public EmailSenderAppComponent EmailSender { get; private set; }
    }
}
