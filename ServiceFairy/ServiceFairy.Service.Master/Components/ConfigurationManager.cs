using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts.Service;
using Common.Contracts;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Package.Service;


namespace ServiceFairy.Service.Master.Components
{
    /// <summary>
    /// 配置文件管理器
    /// </summary>
    [AppComponent("配置信息管理器", "管理服务的配置文件")]
    class ConfigurationManager : AppComponent
    {
        public ConfigurationManager(Service service)
            : base(service)
        {
            _service = service;
            _cache = new SingleMemoryCache<Dictionary<ServiceDesc, AppServiceConfiguration>>(TimeSpan.FromSeconds(10), _LoadConfiguration);
        }

        /// <summary>
        /// 获取全部配置文件
        /// </summary>
        /// <returns></returns>
        public Dictionary<ServiceDesc, AppServiceConfiguration> GetAll()
        {
            return _cache.Get();
        }

        /// <summary>
        /// 获取指定服务的配置文件
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public AppServiceConfiguration Get(ServiceDesc sd)
        {
            Contract.Requires(sd != null);

            return _cache.Get().GetOrDefault(sd);
        }

        private readonly Service _service;
        private readonly SingleMemoryCache<Dictionary<ServiceDesc, AppServiceConfiguration>> _cache;

        private Dictionary<ServiceDesc, AppServiceConfiguration> _LoadConfiguration()
        {
            return SFSettings.GetAllConfigurations(_service.ServiceBasePath);
        }
    }
}
