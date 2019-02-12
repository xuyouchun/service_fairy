using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common;
using Common.Contracts.Service;
using Common.Package;
using Common.Contracts;

namespace ServiceFairy.Server
{
    /// <summary>
    /// Service Fairy 服务端
    /// </summary>
    [AppEntityPoint]
    public class ServiceFairyServerApplication : ServiceFairyApplication
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceFairyServerApplication()
            : base(_CreateServiceProvider(), _basePath, _clientId)
        {
            
        }

        private static string _basePath;
        private static Guid _clientId;

        /// <summary>
        /// 创建ServiceProvider
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider _CreateServiceProvider()
        {
            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IObjectsProvider<ServiceFairyAssemblyInfo>), ServiceAssemblyProvider.FromConfig("appService", out _basePath, out _clientId));
            return sp;
        }

        protected override void OnStart()
        {
            Console.WriteLine("Tray Server Running ...");
            Console.WriteLine("Press any key to exit!");

            Console.ReadKey();
        }
    }
}
