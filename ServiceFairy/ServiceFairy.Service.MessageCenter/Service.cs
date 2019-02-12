using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.MessageCenter.Components;

namespace ServiceFairy.Service.MessageCenter
{
    /// <summary>
    /// 消息中心
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.MessageCenter, "1.0", "消息中心",
        category: AppServiceCategory.System, desc: "负责消息的持久化存储")]
    class Service : SystemAppServiceBase
    {
        public Service()
        {

        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                MessageStorage = new MessageStorageAppComponent(this),
            });
        }

        /// <summary>
        /// 消息存储器
        /// </summary>
        public MessageStorageAppComponent MessageStorage { get; private set; }
    }
}
