using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.GlobalTimer;
using Common;
using Common.Utility;
using ServiceFairy.SystemInvoke;
using Common.Package.Service;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Station.Components
{
    /// <summary>
    /// 版本号管理器
    /// </summary>
    [AppComponent("版本号管理器", "提供部署地图、安装包及配置文件的版本号")]
    class ServiceVersionManagerAppComponent : TimerAppComponentBase
    {
        public ServiceVersionManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;
        private Dictionary<ServiceDesc, DateTime> _configurationLastUpdateVersions;
        private Dictionary<Guid, DateTime> _deployLastUpdateVersions;

        protected override void OnExecuteTask(string taskName)
        {
            _Update();
        }

        public bool IsConfigurationVersionsReady()
        {
            return _configurationLastUpdateVersions != null;
        }

        /// <summary>
        /// 获取配置文件的版本号
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public DateTime GetConfigurationVersion(ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);

            if (_configurationLastUpdateVersions == null)
                return default(DateTime);

            return _configurationLastUpdateVersions.GetOrDefault(serviceDesc);
        }

        public bool IsDeployVersionReady()
        {
            return _deployLastUpdateVersions != null;
        }

        /// <summary>
        /// 获取部署的版本号
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public DateTime GetDeployVersion(Guid clientId)
        {
            if (_deployLastUpdateVersions == null)
                return default(DateTime);

            return _deployLastUpdateVersions.GetOrDefault(clientId);
        }

        /// <summary>
        /// 返回服务配置版本号
        /// </summary>
        /// <param name="serviceDescs"></param>
        /// <returns></returns>
        public ConfigurationVersionPair[] GetConfigurationVersions(ServiceDesc[] serviceDescs)
        {
            Contract.Requires(serviceDescs != null);

            if (!IsConfigurationVersionsReady())
                return null;

            var list = from sd in serviceDescs
                       let ver = GetConfigurationVersion(sd)
                       where ver != default(DateTime)
                       select new ConfigurationVersionPair() { Version = ver, ServiceDesc = sd };

            return list.ToArray();
        }

        /// <summary>
        /// 更新版本号
        /// </summary>
        private void _Update()
        {
            ServiceResult<Master_GetAllVersions_Reply> reply = MasterService.GetAllVersions(_service.Context.ServiceClient);
            if (reply.Succeed && reply.Result != null)
            {
                Master_GetAllVersions_Reply r = reply.Result;
                if (r.ConfigurationVersionPairs != null)
                    _configurationLastUpdateVersions = r.ConfigurationVersionPairs.ToDictionary(v => v.ServiceDesc, v => v.Version, true);

                if (r.DeployVersionPairs != null)
                    _deployLastUpdateVersions = r.DeployVersionPairs.ToDictionary(v => v.ClientID, v => v.Version, true);
            }
        }
    }
}
