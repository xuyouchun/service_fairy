using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Utility;
using Common.Contracts;
using System.Net;
using Common.Communication.Wcf;
using Common.Framework.TrayPlatform;
using System.Threading;
using Common;
using Common.Package;
using Common.Package.GlobalTimer;
using ServiceFairy.Entities;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Service.Tray.Components;
using ServiceFairy.Components;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ServiceFairy.Service.Tray
{
    [AppEntryPoint, AppService(SFNames.ServiceNames.Tray, "1.0", "终端服务",
        category: AppServiceCategory.Core, weight:10, desc:"管理该终端上运行的所有服务，并保持与基站服务的心跳")]
    class Service : SystemAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            AppComponentManager.AddRange(new IAppComponent[] {
                DeployExecutor = new DeployExecutorAppComponent(this),
                HeartBeatController = new HeartBeatControllerAppComponent(this),
                LocalLogManager = new LocalLogManagerAppComponent(this),
                FileSystem = new FileSystemManagerAppComponent(this),
                SystemLogManager = new SystemLogManagerAppComponent(this),
                ProcessManager = new ProcessManagerAppComponent(this),
                SecurityProvider = new SecurityProviderAppComponent(this),
                ServiceCommunicationSearcher = new ServiceCommunicationSearcherAppComponent(this),
                SystemInformationManager = new SystemInformationManagerAppComponent(this),
            });

            Context.Platform.RegisterHandler(typeof(IServiceCommunicationSearcher), ServiceCommunicationSearcher);
            Context.Platform.RegisterHandler(typeof(ISecurityProvider), SecurityProvider);
        }

        /// <summary>
        /// 心跳逻辑
        /// </summary>
        public HeartBeatControllerAppComponent HeartBeatController { get; private set; }

        /// <summary>
        /// 升级与部署
        /// </summary>
        public DeployExecutorAppComponent DeployExecutor { get; private set; }

        /// <summary>
        /// 本地日志管理器
        /// </summary>
        public LocalLogManagerAppComponent LocalLogManager { get; private set; }

        /// <summary>
        /// 文件系统管理器
        /// </summary>
        public FileSystemManagerAppComponent FileSystem { get; private set; }

        /// <summary>
        /// 系统日志管理器
        /// </summary>
        public SystemLogManagerAppComponent SystemLogManager { get; private set; }

        /// <summary>
        /// 进程管理器
        /// </summary>
        public ProcessManagerAppComponent ProcessManager { get; private set; }

        /// <summary>
        /// 安全策略
        /// </summary>
        public SecurityProviderAppComponent SecurityProvider { get; private set; }

        /// <summary>
        /// 服务搜索器
        /// </summary>
        public ServiceCommunicationSearcherAppComponent ServiceCommunicationSearcher { get; private set; }

        /// <summary>
        /// 系统信息管理器
        /// </summary>
        public SystemInformationManagerAppComponent SystemInformationManager { get; private set; }

        protected override void OnStart()
        {
            CommunicationOption op = ServiceFairyUtility.TryGetMasterCommunicationOption(Context.ConfigReader);
            if (op != null)
                Context.Platform.SetData(SFNames.DataKeys.MASTER_COMMUNICATION, op);

            base.OnStart();
        }

        private bool _TryLoadService(ServiceDesc serviceDesc)
        {
            try
            {
                return DeployExecutor.LoadService(serviceDesc);
            }
            catch (Exception ex)
            {
                Common.Package.LogManager.LogError(ex);
                return false;
            }
        }

        protected override void OnDispose()
        {
            ServiceEvent.SyncRaise(ServiceEventNames.EVENT_CLIENT_DISCONNECTED);

            base.OnDispose();
        }
    }
}
