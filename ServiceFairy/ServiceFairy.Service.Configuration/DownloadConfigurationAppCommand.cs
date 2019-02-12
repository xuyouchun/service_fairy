using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Configuration;

namespace ServiceFairy.Service.Configuration
{
    /// <summary>
    /// 获取配置信息
    /// </summary>
    [AppCommand("DownloadConfiguration", "下载配置信息")]
    class DownloadConfigurationAppCommand : ACS<Service>.Func<Configuration_DownloadConfiguration_Request, Configuration_DownloadConfiguration_Reply>
    {
        protected override Configuration_DownloadConfiguration_Reply OnExecute(AppCommandExecuteContext<Service> context, Configuration_DownloadConfiguration_Request req, ref ServiceResult sr)
        {
            Service svr = (Service)context.Service;
            AppServiceConfiguration cfg = svr.ConfigurationManager.Get(req.ServiceDesc);
            if (cfg == null || cfg.LastUpdate == req.LastUpdate)
                return new Configuration_DownloadConfiguration_Reply() { Configuration = null };

            return new Configuration_DownloadConfiguration_Reply() { Configuration = cfg };
        }
    }
}
