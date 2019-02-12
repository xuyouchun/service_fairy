using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package.Service;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;
using ServiceFairy.Entities.Configuration;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 配置信息服务
    /// </summary>
    public static class ConfigurationService
    {
        /// <summary>
        /// 下载配置信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Configuration_DownloadConfiguration_Reply> DownloadConfiguration(IServiceClient serviceClient,
            Configuration_DownloadConfiguration_Request req, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Configuration_DownloadConfiguration_Reply>(SFNames.ServiceNames.Configuration + "/DownloadConfiguration", req, settings);
        }

        /// <summary>
        /// 下载环境变量
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Configuration_DownloadEnvironmentValues_Reply> DownloadEnvironmentValues(IServiceClient serviceClient,
            Configuration_DownloadEnvironmentValues_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Configuration_DownloadEnvironmentValues_Reply>(SFNames.ServiceNames.Configuration + "/DownloadEnvironmentValues", request, settings);
        }
    }
}
