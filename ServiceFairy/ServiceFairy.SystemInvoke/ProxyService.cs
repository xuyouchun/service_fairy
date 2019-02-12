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
using ServiceFairy.Entities.Proxy;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 代理服务
    /// </summary>
    public static class ProxyService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.Proxy + "/" + method;
        }

        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Proxy_GetOnlineUsers_Reply> GetOnlineUsers(IServiceClient serviceClient,
            Proxy_GetOnlineUsers_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Proxy_GetOnlineUsers_Reply>(_GetMethod("GetOnlineUsers"), request, settings);
        }

        /// <summary>
        /// 设置终端设置的信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Init(IServiceClient serviceClient, Proxy_Init_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("Init"), request, settings);
        }
    }
}
