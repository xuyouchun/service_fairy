using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.IO;
using Common.Utility;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using Common.Package;
using ServiceFairy.Entities;

namespace ServiceFairy
{
    public class ServiceFairyAssemblyInfoConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("services", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ServiceElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, RemoveItemName = "remove")] 
        public ServiceCollection Services
        {
            get { return base["services"] as ServiceCollection; }
            set { base["services"] = value; }
        }

        #region Class ServiceCollection ...

        public class ServiceCollection : ConfigurationElementCollection 
        {
            [ConfigurationProperty("runningPath")]
            public string RunningPath
            {
                get { return base["runningPath"] as string; }
                set { base["runningPath"] = value; }
            }

            [ConfigurationProperty("servicePath")]
            public string ServicePath
            {
                get { return base["servicePath"] as string; }
                set { base["servicePath"] = value; }
            }

            [ConfigurationProperty("deployPackagePath")]
            public string DeployPackagePath
            {
                get { return base["deployPackagePath"] as string; }
                set { base["deployPackagePath"] = value; }
            }

            [ConfigurationProperty("dataPath")]
            public string DataPath
            {
                get { return base["dataPath"] as string; }
                set { base["dataPath"] = value; }
            }

            [ConfigurationProperty("logPath")]
            public string LogPath
            {
                get { return base["logPath"] as string; }
                set { base["logPath"] = value; }
            }

            [ConfigurationProperty("clientId")]
            public string ClientID
            {
                get { return base["clientId"] as string; }
                set { base["clientId"] = value; }
            }

            [ConfigurationProperty("clientTitle")]
            public string ClientTitle
            {
                get { return base["clientTitle"] as string; }
                set { base["clientTitle"] = value; }
            }

            [ConfigurationProperty("clientDesc")]
            public string ClientDesc
            {
                get { return base["clientDesc"] as string; }
                set { base["clientDesc"] = value; }
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new ServiceElement() { Service = Guid.NewGuid().ToString() };
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((ServiceElement)element).Service;
            }
        }

        #endregion

        #region Class ServiceElement ...

        public class ServiceElement : ConfigurationElement
        {
            [ConfigurationProperty("service", IsRequired = true)]
            public string Service
            {
                get { return this["service"] as string; }
                set { this["service"] = value; }
            }
        }

        #endregion

        private static string _GetFullPath(string path, string defaultPath)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = defaultPath;

            return PathUtility.Revise(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
        }

        public static ServiceFairyAssemblySettings Load(string configurationName)
        {
            ServiceFairyAssemblyInfoConfiguration cfg = ConfigurationManager.GetSection(configurationName) as ServiceFairyAssemblyInfoConfiguration;
            if (cfg == null)
                return null;

            string runningPath = _GetFullPath(cfg.Services.RunningPath, SFSettings.DefaultRunningPath);
            string servicePath = _GetFullPath(cfg.Services.ServicePath, SFSettings.DefaultServicePath);
            string deployPackagePath = _GetFullPath(cfg.Services.DeployPackagePath, SFSettings.DefaultDeployPackagePath);
            string dataPath = _GetFullPath(cfg.Services.DataPath, SFSettings.DefaultDataPath);
            string logPath = _GetFullPath(cfg.Services.LogPath, SFSettings.DefaultLogPath);

            Guid clientId;
            if (!Guid.TryParse(cfg.Services.ClientID, out clientId))
                clientId = Guid.NewGuid();

            ServiceFairyAssemblyInfo[] assemblyInfos = cfg.Services.OfType<ServiceElement>().Select(delegate(ServiceElement se) {
                ServiceDesc serviceDesc = ServiceDesc.Parse(se.Service);
                if (serviceDesc.Version.IsEmpty)
                    serviceDesc = new ServiceDesc(serviceDesc.Name, "1.0");
                return new ServiceFairyAssemblyInfo(serviceDesc);
            }).ToArray();

            AppClientInitInfo initInfo = new AppClientInitInfo(clientId,
                _GetPlatformDeployId(), runningPath: runningPath, servicePath: servicePath, deployPackagePath: deployPackagePath,
                dataPath: dataPath, logPath: logPath, clientTitle: cfg.Services.ClientTitle, clientDesc: cfg.Services.ClientDesc);
            return new ServiceFairyAssemblySettings(assemblyInfos, initInfo);
        }

        private static Guid _GetPlatformDeployId()
        {
            string path = Path.Combine(PathUtility.GetExecutePath(), SFSettings.PlatformDeployPackageInfoFile);
            if (!File.Exists(path))
                return Guid.Empty;

            try
            {
                return PlatformDeployPackageInfo.DeserailizeFromFile(path).Id;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return Guid.Empty;
            }
        }
    }
}
