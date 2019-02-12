using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common;

namespace ServiceFairy
{
    public class ServiceAssemblyProvider : IObjectProvider<ServiceFairyAssemblySettings>
    {
        public ServiceAssemblyProvider(ServiceFairyAssemblySettings settings = null)
        {
            _settings = settings ?? ServiceFairyAssemblySettings.Empty;
        }

        private readonly ServiceFairyAssemblySettings _settings;

        public virtual ServiceFairyAssemblySettings Get()
        {
            return _settings;
        }

        public static ServiceAssemblyProvider FromConfig(string configName)
        {
            return new ServiceAssemblyProvider(ServiceFairyAssemblyInfoConfiguration.Load(configName));
        }
    }
}
