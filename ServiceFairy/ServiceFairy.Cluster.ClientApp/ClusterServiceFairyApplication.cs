using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts;
using Common.Package.Log;
using Common.Contracts.Log;
using Common.Contracts.Service;

namespace ServiceFairy.Cluster.ClientApp
{
    class ClusterServiceFairyApplication : ServiceFairyApplication
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClusterServiceFairyApplication(string basePath, Guid clientId, ServiceAddress serverAddress)
            : base(_CreateServiceProvider(serverAddress), basePath, clientId)
        {
            LogManager.RegisterLogWritter(new ConsoleLogWriter<LogItem>());
        }

        /// <summary>
        /// 创建ServiceProvider
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider _CreateServiceProvider(ServiceAddress serverAddress)
        {
            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IObjectsProvider<ServiceFairyAssemblyInfo>), new ServiceFairyAssemblyInfoProvider(serverAddress));

            return sp;
        }

        class ServiceFairyAssemblyInfoProvider : IObjectsProvider<ServiceFairyAssemblyInfo>
        {
            public ServiceFairyAssemblyInfoProvider(ServiceAddress serverAddress)
            {
                _serverAddress = serverAddress;
            }

            private readonly ServiceAddress _serverAddress;

            public IEnumerable<ServiceFairyAssemblyInfo> GetAll()
            {
                ServiceDesc sd = new ServiceDesc(SystemServiceNames.Client, "1.0");
                return new ServiceFairyAssemblyInfo[] {
                    new ServiceFairyAssemblyInfo(SystemServiceNames.Client, ServiceFairyUtility.GetConfigPathOfService(sd),
                        Common.Framework.TrayPlatform.TrayAppServiceStartType.AsyncStart, 
                        string.Format("serverAddress={0};serverPort={1}", _serverAddress.Address, _serverAddress.Port))
                };
            }
        }

    }
}
