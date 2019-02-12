using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Framework.TrayPlatform;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Service.Station.Components;
using Common.Package;
using ServiceFairy.Components;

namespace ServiceFairy.Service.Station
{
    /// <summary>
    /// Service
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Station, "1.0", "基站服务",
        category: AppServiceCategory.Core, weight:20, desc:"收集终端服务的运行情况报告发送给中心服务，并将对中心服务的指令分发给指定终端")]
    class Service : CoreAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            AppComponentManager.AddRange(new IAppComponent[] {
                ClientHeatBeatReceiver = new ClientHeatBeatReceiverAppComponent(this),
                ServiceVersionManager = new ServiceVersionManagerAppComponent(this),
                HeartBeatController = new HeartBeatControllerAppComponent(this),
                AppClientInfoManager = new AppClientInfoManagerAppComponent(this),
                ServiceEventManager = new ServiceEventManagerAppComponent(this),
                ServiceAddinManager = new ServiceAddinManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 终端心跳接收器
        /// </summary>
        public ClientHeatBeatReceiverAppComponent ClientHeatBeatReceiver { get; private set; }

        /// <summary>
        /// 服务版本管理器
        /// </summary>
        public ServiceVersionManagerAppComponent ServiceVersionManager { get; private set; }

        /// <summary>
        /// 心跳控制器
        /// </summary>
        public HeartBeatControllerAppComponent HeartBeatController { get; private set; }

        /// <summary>
        /// 客户端实时信息管理
        /// </summary>
        public AppClientInfoManagerAppComponent AppClientInfoManager { get; private set; }

        /// <summary>
        /// 服务事件管理器
        /// </summary>
        public ServiceEventManagerAppComponent ServiceEventManager { get; private set; }

        /// <summary>
        /// 插件管理器
        /// </summary>
        public ServiceAddinManagerAppComponent ServiceAddinManager { get; private set; }
    }
}
