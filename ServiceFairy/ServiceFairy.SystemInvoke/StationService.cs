using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using ServiceFairy.Entities.Station;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// Station服务
    /// </summary>
    public static class StationService
    {
        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Station_ClientHeartBeat_Reply> ClientHeartBeat(IServiceClient serviceClient,
            Station_ClientHeartBeat_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Station_ClientHeartBeat_Reply>(SFNames.ServiceNames.Station + "/ClientHeartBeat", request, settings);
        }

        /// <summary>
        /// 激发事件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RaiseEvent(IServiceClient serviceClient, Station_RaiseEvent_Request request,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Station + "/RaiseEvent", request, settings);
        }

        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RegisterAddins(IServiceClient serviceClient, Station_RegisterAddins_Request request,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Station + "/RegisterAddins", request, settings);
        }
        　
    }
}
