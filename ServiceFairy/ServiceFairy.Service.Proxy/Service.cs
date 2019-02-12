using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Framework.TrayPlatform;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Service.Proxy.Components;
using Common.Package;
using Common.Package.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.Proxy.Addins;

namespace ServiceFairy.Service.Proxy
{
    /// <summary>
    /// 代理服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Proxy, "1.0", "代理服务",
        category: AppServiceCategory.System, weight:10, desc:"接收并转发集群之外的访问请求")]
    class Service : SystemAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            AppComponentManager.AddRange(new IAppComponent[] {
                ProxyAccessManager = new ProxyAccessManager(this),
                ProxyLifeController = new ProxyLifeControllerAppComponent(this),
                OnlineUserManager = new OnlineUserManagerAppComponent(this),
                MessageDispatcher = new MessageDispatcherAppComponent(this),
            });

            this.ServiceAddIn.Register(SFNames.ServiceNames.Message, new MesasgeSubscriptAddin(this));
        }

        /// <summary>
        /// 代理访问管理器
        /// </summary>
        public ProxyAccessManager ProxyAccessManager { get; private set; }

        /// <summary>
        /// 代理生命周期
        /// </summary>
        public ProxyLifeControllerAppComponent ProxyLifeController { get; set; }

        /// <summary>
        /// 消息订阅管理器
        /// </summary>
        public OnlineUserManagerAppComponent OnlineUserManager { get; private set; }

        /// <summary>
        /// 消息分发器
        /// </summary>
        public MessageDispatcherAppComponent MessageDispatcher { get; private set; }
    }
}
