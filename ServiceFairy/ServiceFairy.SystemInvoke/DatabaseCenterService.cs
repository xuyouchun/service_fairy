using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using ServiceFairy.Entities.DatabaseCenter;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 数据库中心服务
    /// </summary>
    public static class DatabaseCenterService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.DatabaseCenter + "/" + method;
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static ServiceResult<DatabaseCenter_GetConStr_Reply> GetConStr(IServiceClient serviceClient,
            DatabaseCenter_GetConStr_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<DatabaseCenter_GetConStr_Reply>(_GetMethod("GetConStr"), request, settings);
        }
    }
}
