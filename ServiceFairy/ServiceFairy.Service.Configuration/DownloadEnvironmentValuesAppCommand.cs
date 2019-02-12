using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Configuration;

namespace ServiceFairy.Service.Configuration
{
    /// <summary>
    /// 获取环境变量
    /// </summary>
    [AppCommand("DownloadEnvironmentValues", "获取环境变量")]
    class DownloadEnvironmentValuesAppCommand : ACS<Service>.Func<Configuration_DownloadEnvironmentValues_Request, Configuration_DownloadEnvironmentValues_Reply>
    {
        protected override Configuration_DownloadEnvironmentValues_Reply OnExecute(AppCommandExecuteContext<Service> context, Configuration_DownloadEnvironmentValues_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            DateTime lastUpdate;
            EnvironmentValue[] values = context.Service.EnvironmentValueManager.GetAll(out lastUpdate);
            if (lastUpdate == request.LastUpdate)
                values = null;

            return new Configuration_DownloadEnvironmentValues_Reply() {
                Values = new EnvironmentValues { Values = values, LastUpdate = lastUpdate }
            };
        }
    }
}
