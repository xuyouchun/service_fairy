using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Install;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using System.IO;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Product
{
    /// <summary>
    /// Master
    /// </summary>
    class MasterRunningConfigurationServiceAssemblyProvider : RunningConfigurationServiceAssemblyProviderBase
    {
        public MasterRunningConfigurationServiceAssemblyProvider(RunningConfiguration runningCfg)
            : base(runningCfg)
        {
            
        }

        protected override ServiceFairyAssemblySettings OnGet()
        {
            foreach (ServiceDesc sd in InstallUtility.GetAllServiceDescsFromInstallPath())
            {
                try
                {
                    LoadServiceFairyAssemblyInfo(sd);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }

            return base.OnGet();
        }

        protected override ServiceFairyAssemblyInfo[] LoadServiceFairyAssemblyInfos()
        {
            ServiceDesc[] servicesDescs = GetServicesToStart();
            return servicesDescs.ToArray(sd => LoadServiceFairyAssemblyInfo(sd));
        }

        protected override string LoadConfig(ServiceDesc sd)
        {
            string cfg = base.LoadConfig(sd);
            if (!string.IsNullOrWhiteSpace(cfg))
                return cfg;

            if (sd.Name == SFNames.ServiceNames.Master)  // 中心服务的配置文件
            {
                return CombineConfiguration(sd, new Dictionary<string, string> {
                    { "masterAddress", Configuration.MasterCommunications },
                });
            }

            return null; // 返回null将加载默认的配置文件
        }
    }
}
