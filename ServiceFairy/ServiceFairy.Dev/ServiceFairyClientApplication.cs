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
using System.IO;

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
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IObjectProvider<ServiceFairyAssemblySettings>), ServiceAssemblyProvider.FromConfig("appService"));

            return sp;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            File.AppendAllText(Path.Combine(appDataPath, "UnhandledException.txt"),
                string.Format("[{0}] {1}\r\n====================================\r\n", DateTime.Now, e.ExceptionObject));
        }

        protected override ILogWriter<LogItem> CreateLogWriter()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return new FileLogWriter<LogItem>(Path.Combine(appDataPath , "ServiceFairy\\Log"));
        }

        protected override void OnStart()
        {
            LogManager.LogMessage("ServiceFairy Dev Running ...");
        }
    }
}
