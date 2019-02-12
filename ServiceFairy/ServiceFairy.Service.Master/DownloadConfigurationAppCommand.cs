using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package;
using Common.Utility;
using Common.Package.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master
{
    /// <summary>
    /// 下载配置文件
    /// </summary>
    [AppCommand("DownloadConfiguration", "下载配置文件", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class DownloadConfigurationAppCommand : ACS<Service>.Func<Master_DownloadConfiguration_Request, Master_DownloadConfiguration_Reply>
    {
        protected override Master_DownloadConfiguration_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_DownloadConfiguration_Request req, ref ServiceResult sr)
        {
            Service service = (Service)context.Service;

            ServiceConfigurationPair[] cfgPairs = null;
            if (!req.ServiceLastUploadPairs.IsNullOrEmpty())
            {
                Dictionary<ServiceDesc, DateTime> lastUpdateDict = req.ServiceLastUploadPairs.ToDictionary(v => v.ServiceDesc, v => v.LastUpdate);
                Dictionary<ServiceDesc, AppServiceConfiguration> configs = service.ConfigurationManager.GetAll();
                cfgPairs = configs.Where(cfg => _IsUpdated(cfg, lastUpdateDict))
                    .Select(cfg => new ServiceConfigurationPair() { ServiceDesc = cfg.Key, Configuration = cfg.Value }).ToArray();
            }

            return new Master_DownloadConfiguration_Reply() {
                ServiceConfigurationPairs = cfgPairs ?? new ServiceConfigurationPair[0]
            };
        }

        private bool _IsUpdated(KeyValuePair<ServiceDesc, AppServiceConfiguration> cfg, Dictionary<ServiceDesc, DateTime> lastUpdateDict)
        {
            DateTime dt;
            if (!lastUpdateDict.TryGetValue(cfg.Key, out dt))
                return false;

            return dt != cfg.Value.LastUpdate;
        }
    }
}
