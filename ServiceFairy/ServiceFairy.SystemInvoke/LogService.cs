using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities.Message;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;
using ServiceFairy.Entities.Log;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public static class LogService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.Log + "/" + method;
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult DeleteLog(IServiceClient serviceClient, Log_DeleteLog_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("DeleteLog"), request, settings);
        }

        /// <summary>
        /// 读取日志
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Log_ReadLog_Reply> ReadLog(IServiceClient serviceClient, Log_ReadLog_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Log_ReadLog_Reply>(_GetMethod("ReadLog"), request, settings);
        }

        /// <summary>
        /// 获取当前日志分析结果
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Log_GetAnalyzeResult_Reply> GetAnalyzeResult(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Log_GetAnalyzeResult_Reply>(_GetMethod("GetAnalyzeResult"), settings);
        }
    }
}
