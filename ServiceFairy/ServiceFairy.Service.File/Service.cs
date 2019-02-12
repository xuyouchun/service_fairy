using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.File.Components;
using System.IO;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 文件服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.File, "1.0", "文件服务",
        category: AppServiceCategory.System, desc: "分布式文件系统")]
    class Service : SystemAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            Settings.DataBasePath = Context.ConfigReader.Get<string>("data_base_path", Path.Combine(ServiceDataPath, "files"));

            this.AppComponentManager.AddRange(new IAppComponent[] {
                FileSystemManager = new FileSystemManagerAppComponent(this),
                FileRoute = new FileRouteAppComponent(this),
                StreamTableManager = new StreamTableManagerAppComponent(this),
                FileScaner = new FileScanerAppComponent(this),
                FileCryptoExecutor = new FileCryptoExecutorAppComponent(this),
            });

            this.LoadStatusCodeInfosFromType(typeof(FileStatusCode));
        }

        /// <summary>
        /// 文件扫描器
        /// </summary>
        public FileScanerAppComponent FileScaner { get; private set; }

        /// <summary>
        /// 文件系统管理器
        /// </summary>
        public FileSystemManagerAppComponent FileSystemManager { get; private set; }

        /// <summary>
        /// 文件路由器
        /// </summary>
        public FileRouteAppComponent FileRoute { get; private set; }

        /// <summary>
        /// StreamTable管理器
        /// </summary>
        public StreamTableManagerAppComponent StreamTableManager { get; private set; }

        /// <summary>
        /// 文件安全服务
        /// </summary>
        public FileCryptoExecutorAppComponent FileCryptoExecutor { get; private set; }
    }
}
