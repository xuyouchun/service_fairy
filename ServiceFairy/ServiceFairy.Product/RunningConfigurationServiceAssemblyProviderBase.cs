using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Install;
using Common.Contracts.Service;
using Common;
using Common.Utility;
using System.IO;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Package;
using ServiceFairy.Entities.Master;
using ServiceFairy.Deploy;
using Common.Framework.TrayPlatform;
using ServiceFairy.Entities;

namespace ServiceFairy.Product
{
    /// <summary>
    /// 基于运行时配置的ServiceAssemblyProvider
    /// </summary>
    abstract class RunningConfigurationServiceAssemblyProviderBase : ServiceAssemblyProvider
    {
        public RunningConfigurationServiceAssemblyProviderBase(RunningConfiguration configuration)
        {
            Configuration = configuration;
            _result = new Lazy<ServiceFairyAssemblySettings>(OnGet);
        }

        public RunningConfiguration Configuration { get; private set; }

        public static RunningConfigurationServiceAssemblyProviderBase FromRunningConfiguration(RunningConfiguration cfg)
        {
            Contract.Requires(cfg != null);

            if (cfg.RunningModel.HasFlag(RunningModel.Master))
                return new MasterRunningConfigurationServiceAssemblyProvider(cfg);

            return new TrayRunningConfigurationServiceAssemblyProvider(cfg);
        }

        public sealed override ServiceFairyAssemblySettings Get()
        {
            return _result.Value;
        }

        protected readonly Lazy<ServiceFairyAssemblySettings> _result;

        protected virtual ServiceFairyAssemblySettings OnGet()
        {
            AppClientInitInfo initInfo = _GetAppClientInitInfo();
            ServiceFairyAssemblyInfo[] assemblyInfos = LoadServiceFairyAssemblyInfos();
            ServiceFairyAssemblySettings settings = new ServiceFairyAssemblySettings(assemblyInfos, initInfo);

            return settings;
        }

        // 获取客户端的初始化信息
        private AppClientInitInfo _GetAppClientInitInfo()
        {
            RunningConfiguration runningCfg = Configuration;
            return new AppClientInitInfo(runningCfg.ClientID, _GetPlatformDeployId(),
                InstallUtility.GetRunningPath(runningCfg.ServiceBasePath),
                InstallUtility.GetServiceBasePath(runningCfg.ServiceBasePath),
                InstallUtility.GetDeployPackageBasePath(runningCfg.ServiceBasePath),
                InstallUtility.GetDataPath(runningCfg.ServiceBasePath),
                InstallUtility.GetLogPath(runningCfg.ServiceBasePath),
                runningCfg.ClientTitle ?? string.Empty, runningCfg.ClientDesc ?? string.Empty
            );
        }

        protected abstract ServiceFairyAssemblyInfo[] LoadServiceFairyAssemblyInfos();

        private readonly Dictionary<ServiceDesc, ServiceFairyAssemblyInfo> _sfAssemblyInfos = new Dictionary<ServiceDesc, ServiceFairyAssemblyInfo>();

        private ServiceFairyAssemblyInfo _LoadServiceFairyAssemblyInfo(ServiceDesc sd)
        {
            RunningConfiguration runningCfg = Configuration;
            string installPathOfService = InstallUtility.GetInstallPathOfService(ref sd);
            if (installPathOfService == null)
                throw new DirectoryNotFoundException("不存在服务“" + sd + "”的安装目录");

            // 拷贝该服务所需的文件
            string deployPackagePath = InstallUtility.GetServicePath(runningCfg.ServiceBasePath, sd);

            try
            {
                PathUtility.CopyDirectoryIfNewer(Path.Combine(PathUtility.GetExecutePath(), "Assembly"), deployPackagePath);
                PathUtility.CopyDirectoryIfNewer(installPathOfService, deployPackagePath);
                PathUtility.CopyFileIfExist(installPathOfService + ".config", deployPackagePath + ".config", true);
            }
            catch
            {
                PathUtility.ClearPath(deployPackagePath);
                throw;
            }

            string config = LoadConfig(sd);
            return new ServiceFairyAssemblyInfo(sd, config);
        }

        // 加载服务的Assembly信息
        protected ServiceFairyAssemblyInfo LoadServiceFairyAssemblyInfo(ServiceDesc sd)
        {
            return _sfAssemblyInfos.GetOrSet(sd, _LoadServiceFairyAssemblyInfo);
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        protected virtual string LoadConfig(ServiceDesc sd)
        {
            if (sd.Name == SFNames.ServiceNames.Tray)
            {
                return CombineConfiguration(sd, new Dictionary<string, string> {
                    { "masterAddress", Configuration.MasterCommunications },
                    { "initServices", Configuration.ServicesToStart },
                    { "initCommunications", Configuration.CommunicationsToOpen },
                });
            }

            return null;
        }

        protected ServiceDesc[] GetServicesToStart()
        {
            RunningConfiguration runningCfg = Configuration;
            string[] ss = (runningCfg.ServicesToStart ?? string.Empty)
                .Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray(s => s.Trim());

            return ss.SelectDistinct(s => ServiceDesc.Parse(s.Trim()))
                .Union(runningCfg.RunningModel.GetServiceDescs(), new ServiceDescComparer()).ToArray();
        }

        #region Class ServiceDescComparer ...

        class ServiceDescComparer : IEqualityComparer<ServiceDesc>
        {
            public bool Equals(ServiceDesc x, ServiceDesc y)
            {
                if (x.Version.IsEmpty || y.Version.IsEmpty)
                    return object.Equals(x.Name, y.Name);

                return object.Equals(x, y);
            }

            public int GetHashCode(ServiceDesc obj)
            {
                return obj.Version.IsEmpty ? obj.Version.GetHashCode() : obj.GetHashCode();
            }
        }

        #endregion


        protected string CombineConfiguration(ServiceDesc serviceDesc, IDictionary<string, string> dict)
        {
            XmlConfigurationBuilder cfgBuilder = new XmlConfigurationBuilder();
            string configPath = SFSettings.GetServiceConfigPath(serviceDesc,
                InstallUtility.GetDeployPackageBasePath(Configuration.ServiceBasePath));

            if (File.Exists(configPath))
            {
                XmlConfigurationReader r = new XmlConfigurationReader(File.ReadAllText(configPath));
                cfgBuilder.Append(r);
            }

            foreach (KeyValuePair<string, string> item in dict)
            {
                cfgBuilder.Append(item.Key, item.Value);
            }

            return cfgBuilder.ToString();
        }

        private static Guid _GetPlatformDeployId()
        {
            string path = Path.Combine(PathUtility.GetExecutePath(), SFSettings.PlatformDeployPackageInfoFile);
            if (!File.Exists(path))
                return Guid.Empty;

            try
            {
                return XmlUtility.DeserailizeFromFile<PlatformDeployPackageInfo>(path).Id;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return Guid.Empty;
            }
        }
    }
}
