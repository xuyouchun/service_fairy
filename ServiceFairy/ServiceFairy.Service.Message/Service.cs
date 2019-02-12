using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Components;
using Common.Contracts.Service;
using ServiceFairy.Service.Message.Components;
using Common.Contracts;

namespace ServiceFairy.Service.Message
{
    /// <summary>
    /// 消息服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Message, "1.0", "消息服务",
        category: AppServiceCategory.System, desc: "提供服务器主动推送消息的途径")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                MessageDispatcher = new MessageDispatcherAppComponent(this),
                MessageStorageManager = new MessageStorageManagerAppComponent(this),
                MessageSubscriptAddinManager = new MessageSubscriptAddinManager(this),
                UserOnlineInfoCollector = new UserOnlineInfoCollectorAppComponent(this),
            });
        }

        /// <summary>
        /// 消息分发器
        /// </summary>
        public MessageDispatcherAppComponent MessageDispatcher { get; private set; }

        /// <summary>
        /// 消息存储器
        /// </summary>
        public MessageStorageManagerAppComponent MessageStorageManager { get; private set; }

        /// <summary>
        /// 消息订阅插件管理器
        /// </summary>
        public MessageSubscriptAddinManager MessageSubscriptAddinManager { get; private set; }

        /// <summary>
        /// 用户在线信息收集器
        /// </summary>
        public UserOnlineInfoCollectorAppComponent UserOnlineInfoCollector { get; private set; }
    }
}
