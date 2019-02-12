using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Utility;
using Common.Framework.TrayPlatform;
using Common;
using Common.Contracts;
using System.Net;
using System.Threading;
using Common.Package;
using ServiceFairy.Client;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Service.Master.Components;
using ServiceFairy.Components;
using System.IO;
using Common.Communication.Wcf;

namespace ServiceFairy.Service.Master
{
    /// <summary>
    /// Service
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Master, "1.0", "中心服务",
        category: AppServiceCategory.Core, weight: 0, desc: "集群控制中心，调度各个服务的运行")]
    class Service : CoreAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            AppComponentManager.AddRange(new IAppComponent[]{
                AppClientManager = new AppClientManager(this),
                DeployMapManager = new AppClientDeployMapManager(this),
                ConfigurationManager = new ConfigurationManager(this),
                PlatformDeployPackageManager = new PlatformDeployPackageManager(this),
                ServiceDeployPackageManager = new ServiceDeployPackageManager(this),
                ServiceUIInfoManager = new ServiceUIInfoManager(this),
                DeployTaskLocker = new DeployTaskLockerAppComponent(this),
                CurrentStateRecorder = new CurrentStateRecorderAppComponent(this),
            });

            AppClientManager.Connected += new EventHandler<AppClientInfoManagerClientConnectedEventArgs>(AppClientManager_Connected);
            AppClientManager.Disconnected += new EventHandler<AppClientInfoManagerClientDisconnectedEventArgs>(AppClientManager_Disconnected);
        }

        void AppClientManager_Connected(object sender, AppClientInfoManagerClientConnectedEventArgs e)
        {
            Common.Package.LogManager.LogMessage("新终端连接: " + string.Join(",", (object[])e.Clients));
        }

        void AppClientManager_Disconnected(object sender, AppClientInfoManagerClientDisconnectedEventArgs e)
        {
            Common.Package.LogManager.LogMessage("终端断开：" + string.Join(",", (object[])e.Clients));
        }

        protected override void OnStart()
        {
            // 开启通信端口
            MasterCommunications = _StartListener();
            Context.Platform.SetData(SFNames.DataKeys.MASTER_COMMUNICATION, MasterCommunications);

            base.OnStart();

            Common.Package.LogManager.LogMessage("中心服务已启动");
        }

        /// <summary>
        /// Master服务器的通信方式
        /// </summary>
        public CommunicationOption[] MasterCommunications { get; private set; }

        /// <summary>
        /// 启动侦听
        /// </summary>
        private CommunicationOption[] _StartListener()
        {
            CommunicationOption[] ops = null;
            try
            {
                ops = ServiceFairyUtility.GetMasterCommunicationOption(Context.ConfigReader);
                Context.Platform.RestartListeners(ops, false);
            }
            catch (Exception ex)
            {
                Common.Package.LogManager.LogError(ex);
            }

            return ops;
        }

        /// <summary>
        /// 终端操作锁，用于防止多个任务同时运行时引起的混乱
        /// </summary>
        public DeployTaskLockerAppComponent DeployTaskLocker { get; private set; }

        /// <summary>
        /// 部署地图管理器
        /// </summary>
        public AppClientDeployMapManager DeployMapManager { get; private set; }

        /// <summary>
        /// 平台安装包管理器
        /// </summary>
        public PlatformDeployPackageManager PlatformDeployPackageManager { get; private set; }

        /// <summary>
        /// 服务安装包管理器
        /// </summary>
        public ServiceDeployPackageManager ServiceDeployPackageManager { get; private set; }

        /// <summary>
        /// 终端管理器
        /// </summary>
        public AppClientManager AppClientManager { get; private set; }

        /// <summary>
        /// 配置信息管理器
        /// </summary>
        public ConfigurationManager ConfigurationManager { get; private set; }

        /// <summary>
        /// 服务管理器的信息管理
        /// </summary>
        public ServiceUIInfoManager ServiceUIInfoManager { get; private set; }

        /// <summary>
        /// 集群当前状态记录器
        /// </summary>
        public CurrentStateRecorderAppComponent CurrentStateRecorder { get; private set; }
    }
}
