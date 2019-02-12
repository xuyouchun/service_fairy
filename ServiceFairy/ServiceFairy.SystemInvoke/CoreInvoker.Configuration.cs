using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Configuration;

namespace ServiceFairy.SystemInvoke
{
	partial class CoreInvoker
	{
        private ConfigurationInvoker _configuration;

        /// <summary>
        /// Configuration Service
        /// </summary>
        public ConfigurationInvoker Configuration
        {
            get { return _configuration ?? (_configuration = new ConfigurationInvoker(this)); }
        }

        /// <summary>
        /// Configuration Service
        /// </summary>
        public class ConfigurationInvoker : Invoker
        {
            public ConfigurationInvoker(CoreInvoker owner)
                : base(owner)
            {

            }

            /// <summary>
            /// 下载指定服务的配置信息
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="lastUpdate">最后更新时间，如果相同则返回空</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppServiceConfiguration> DownloadConfigurationSr(ServiceDesc serviceDesc,
                DateTime lastUpdate = default(DateTime), CallingSettings settings = null)
            {
                var sr = ConfigurationService.DownloadConfiguration(Sc,
                    new Configuration_DownloadConfiguration_Request { LastUpdate = lastUpdate, ServiceDesc = serviceDesc }
                    , settings);

                return CreateSr(sr, r => r.Configuration);
            }

            /// <summary>
            /// 下载指定服务的配置信息
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="lastUpdate">最后更新时间，如果相同则返回空</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppServiceConfiguration DownloadConfiguration(ServiceDesc serviceDesc,
                DateTime lastUpdate = default(DateTime), CallingSettings settings = null)
            {
                return InvokeWithCheck(DownloadConfigurationSr(serviceDesc, lastUpdate, settings));
            }

            /// <summary>
            /// 下载环境变量
            /// </summary>
            /// <param name="lastUpdate">最后更新时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<EnvironmentValues> DownloadEnvironmentValuesSr(DateTime lastUpdate = default(DateTime), CallingSettings settings = null)
            {
                var sr = ConfigurationService.DownloadEnvironmentValues(Sc, new Configuration_DownloadEnvironmentValues_Request {
                    LastUpdate = lastUpdate
                });

                return CreateSr(sr, r => r.Values);
            }

            /// <summary>
            /// 下载环境变量
            /// </summary>
            /// <param name="lastUpdate">最后更新时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public EnvironmentValues DownloadEnvironmentValues(DateTime lastUpdate = default(DateTime), CallingSettings settings = null)
            {
                return InvokeWithCheck(DownloadEnvironmentValuesSr(lastUpdate, settings));
            }
        }
	}
}
