using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts;
using Common.Package.Log;
using Common.Contracts.Log;
using Common.Contracts.Service;
using System.Threading;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Cluster
{
    public class ClusterServiceFairyApplication : ServiceFairyApplication
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClusterServiceFairyApplication(AppClientInitInfo initInfo, CommunicationOption serverCo, ManualResetEvent waitForExit)
            : base(_CreateServiceProvider(serverCo, initInfo))
        {
            _waitForExit = waitForExit;
            //LogManager.RegisterLogWritter(new ConsoleLogWriter<LogItem>());
        }

        private readonly ManualResetEvent _waitForExit;

        /// <summary>
        /// 创建ServiceProvider
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider _CreateServiceProvider(CommunicationOption serverCo, AppClientInitInfo initInfo)
        {
            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IObjectProvider<ServiceFairyAssemblySettings>),
                new ServiceFairyAssemblyInfoProvider(serverCo, initInfo));

            return sp;
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        class ServiceFairyAssemblyInfoProvider : IObjectProvider<ServiceFairyAssemblySettings>
        {
            public ServiceFairyAssemblyInfoProvider(CommunicationOption serverCo, AppClientInitInfo initInfo)
            {
                _serverCo = serverCo;
                _initInfo = initInfo;
            }

            private readonly CommunicationOption _serverCo;
            private readonly AppClientInitInfo _initInfo;

            public ServiceFairyAssemblySettings Get()
            {
                return new ServiceFairyAssemblySettings(
                    new ServiceFairyAssemblyInfo[] { 
                        new ServiceFairyAssemblyInfo(Settings.TrayServiceDesc, string.Format("masterAddress={0}", _serverCo))
                    }, _initInfo
                );
            }
        }
    }
}
