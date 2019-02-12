using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using Common.Contracts;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Client
{
    /// <summary>
    /// Service Fairy 客户端
    /// </summary>
    [AppEntityPoint]
    public class ServiceFairyClientApplication : ServiceFairyApplication
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceFairyClientApplication()
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
            Console.WriteLine("BhFairy Server Running ...");
            Console.WriteLine("Press any key to exit!");

            Console.ReadKey();
        }
    }
}
