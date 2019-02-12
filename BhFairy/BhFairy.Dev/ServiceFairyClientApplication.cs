using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using Common.Contracts;
using Common.Framework.TrayPlatform;
using Common;
using Common.Contracts.Log;
using Common.Package.Log;

namespace ServiceFairy.Dev
{
    /// <summary>
    /// Service Fairy 客户端
    /// </summary>
    [AppEntryPoint]
    public class ServiceFairyClientApplication : ServiceFairyApplication
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceFairyClientApplication()
            : base(_CreateServiceProvider())
        {
            
        }

        /// <summary>
        /// 创建ServiceProvider
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider _CreateServiceProvider()
        {
            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IObjectProvider<ServiceFairyAssemblySettings>), _saProvider);

            return sp;
        }

        private static readonly ServiceAssemblyProvider _saProvider = ServiceAssemblyProvider.FromConfig("appService");

        protected override ILogWriter<LogItem> CreateLogWriter()
        {
            return new LogWriterCollection<LogItem>(new ILogWriter<LogItem>[] {
                new ConsoleLogWriter<LogItem>(),
                new FileLogWriter<LogItem>(_saProvider.Get().InitInfo.LogPath),
            });
        }

        protected override ILogReader<LogItem> CreateLogReader()
        {
            return new FileLogReader<LogItem>(_saProvider.Get().InitInfo.LogPath);
        }

        protected override void OnStart()
        {
            LogManager.LogMessage("BhFairy Dev Running ...");
        }
    }
}
