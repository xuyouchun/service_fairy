using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.Navigation;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 导航服务
    /// </summary>
    public static class NavigationService
    {
        /// <summary>
        /// 获取代理列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Navigation_GetProxyList_Reply> GetProxyList(IServiceClient serviceClient,
            Navigation_GetProxyList_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Navigation_GetProxyList_Reply>(SFNames.ServiceNames.Navigation + "/GetProxyList", request, settings);
        }

        /// <summary>
        /// 获取代理地址
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Navigation_GetProxy_Reply> GetProxy(IServiceClient serviceClient,
            Navigation_GetProxy_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Navigation_GetProxy_Reply>(SFNames.ServiceNames.Navigation + "/GetProxy", request, settings);
        }

        /// <summary>
        /// 获取代理地址Url
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Navigation_GetProxyUrl_Reply> GetProxyUrl(IServiceClient serviceClient,
            Navigation_GetProxyUrl_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Navigation_GetProxyUrl_Reply>(SFNames.ServiceNames.Navigation + "/GetProxyUrl", request, settings);
        }
    }
}
