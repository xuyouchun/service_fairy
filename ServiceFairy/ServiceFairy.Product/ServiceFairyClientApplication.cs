using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts;
using ServiceFairy.Install;
using Common.Utility;
using System.IO;
using Common.Contracts.Service;
using Common.Contracts.Log;
using Common.Package.Log;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Product
{
    /// <summary>
    /// Service Fairy 服务终端
    /// </summary>
    [AppEntryPoint]
    public class ServiceFairyClientApplication : ServiceFairyApplication
    {
        public ServiceFairyClientApplication()
            : base(_CreateServiceProvider())
        {

        }

        static ServiceFairyClientApplication()
        {
            LogManager.SetDefaultSource(SFSettings.PlatformLogSource);
        }

        /// <summary>
        /// 创建ServiceProvider
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider _CreateServiceProvider()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IObjectProvider<ServiceFairyAssemblySettings>), _CreateServiceAssemblyProvider());
            //sp.AddService(typeof(ILogWriter<LogItem>), new FileLogWriter<LogItem>(@"d:\temp\servicefariy log"));

            return sp;
        }

        private static ServiceAssemblyProvider _CreateServiceAssemblyProvider()
        {
            return RunningConfigurationServiceAssemblyProviderBase.FromRunningConfiguration(_runningCfg.Value);
        }

        private static readonly Lazy<RunningConfiguration> _runningCfg = new Lazy<RunningConfiguration>(delegate {
            string cfgFile = Path.Combine(PathUtility.GetExecutePath(), "Configuration.xml");
            return RunningConfiguration.LoadFromFileOrDefault(cfgFile);
        });

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            File.AppendAllText(Path.Combine(_runningCfg.Value.ServiceBasePath, "UnhandledException.txt"),
                string.Format("[{0}] {1}\r\n====================================\r\n", DateTime.Now, e.ExceptionObject));
        }

        protected override ILogWriter<LogItem> CreateLogWriter()
        {
            return new FileLogWriter<LogItem>(InstallUtility.GetLogPath(_runningCfg.Value.ServiceBasePath));
        }

        protected override ILogReader<LogItem> CreateLogReader()
        {
            return new FileLogReader<LogItem>(InstallUtility.GetLogPath(_runningCfg.Value.ServiceBasePath));
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
