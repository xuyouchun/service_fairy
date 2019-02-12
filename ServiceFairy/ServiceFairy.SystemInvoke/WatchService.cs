using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.User;
using ServiceFairy.Entities.Watch;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 监控服务
    /// </summary>
    public static class WatchService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.Watch + "/" + method;
        }

        /// <summary>
        /// 获取监控信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Watch_GetWatchInfo_Reply> GetWatchInfo(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Watch_GetWatchInfo_Reply>(_GetMethod("GetWatchInfo"), settings);
        }
    }
}
