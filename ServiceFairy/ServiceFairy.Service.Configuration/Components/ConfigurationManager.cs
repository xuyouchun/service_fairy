using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Collection;
using Common.Utility;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Package;
using Common.Package.GlobalTimer;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Package.Service;
using System.IO;
using Common;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Configuration.Components
{
    /// <summary>
    /// 配置信息管理器
    /// </summary>
    [AppComponent("配置信息管理器", "管理服务的配置信息")]
    class ConfigurationManagerAppComponent : TimerAppComponentBase
    {
        public ConfigurationManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;
        private Dictionary<ServiceDesc, AppServiceConfiguration> _dict;

        protected override void OnExecuteTask(string taskName)
        {
            _Update();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public AppServiceConfiguration Get(ServiceDesc serviceDesc)
        {
            return _dict.GetOrDefault(serviceDesc);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="configuration"></param>
        public void Set(ServiceDesc serviceDesc, AppServiceConfiguration configuration)
        {
            Contract.Requires(serviceDesc != null && configuration != null);

            _dict[serviceDesc] = configuration;
        }

        private void _Update()
        {
            try
            {
                string servicePath = _service.ServiceBasePath;
                _dict = (_dict ?? SFSettings.GetAllConfigurations(servicePath));
                ServiceLastUpdatePair[] pairs = _dict.Select(v => new ServiceLastUpdatePair() { LastUpdate = v.Value.LastUpdate, ServiceDesc = v.Key }).ToArray();
                ServiceResult<Master_DownloadConfiguration_Reply> sr = MasterService.DownloadConfiguration(_service.Context.ServiceClient, new Master_DownloadConfiguration_Request() { ServiceLastUploadPairs = pairs });

                if (sr.Succeed && !sr.Result.ServiceConfigurationPairs.IsNullOrEmpty())
                {
                    foreach (ServiceConfigurationPair pair in sr.Result.ServiceConfigurationPairs)
                    {
                        _SaveConfiguration(pair.ServiceDesc, pair.Configuration, servicePath);
                    }
                }

                _dict = SFSettings.GetAllConfigurations(servicePath);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        private void _SaveConfiguration(ServiceDesc serviceDesc, AppServiceConfiguration config, string servicePath)
        {
            try
            {
                string path = SFSettings.GetServiceConfigPath(serviceDesc, servicePath);
                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(path, config.Content ?? string.Empty, Encoding.UTF8);
                File.SetLastWriteTimeUtc(path, config.LastUpdate);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }
    }
}
