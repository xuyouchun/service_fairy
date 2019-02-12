using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Install;
using Common.Contracts.Service;

namespace ServiceFairy.Product
{
    class TrayRunningConfigurationServiceAssemblyProvider : RunningConfigurationServiceAssemblyProviderBase
    {
        public TrayRunningConfigurationServiceAssemblyProvider(RunningConfiguration runningCfg)
            : base(runningCfg)
        {

        }

        protected override ServiceFairyAssemblyInfo[] LoadServiceFairyAssemblyInfos()
        {
            return new ServiceFairyAssemblyInfo[] {
                LoadServiceFairyAssemblyInfo(ServiceDesc.Parse(SFNames.ServiceNames.Tray)),
            };
        }

        protected override string LoadConfig(ServiceDesc sd)
        {
            return base.LoadConfig(sd);
        }
    }
}
