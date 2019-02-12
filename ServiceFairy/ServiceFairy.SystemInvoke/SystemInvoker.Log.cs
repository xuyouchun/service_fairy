using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;
using System.Runtime.Serialization;
using Common.Package.Serializer;
using Common.Utility;
using ServiceFairy.Entities.Cache;
using Common.Contracts.Service;
using Common;
using ServiceFairy.Entities.Log;

namespace ServiceFairy.SystemInvoke
{
	partial class SystemInvoker
	{
        private LogInvoker _log;

        /// <summary>
        /// Log Service
        /// </summary>
        public LogInvoker Log
        {
            get { return _log ?? (_log = new LogInvoker(this)); }
        }

        /// <summary>
        /// 日志
        /// </summary>
        public class LogInvoker : Invoker
        {
            public LogInvoker(SystemInvoker owner)
                : base(owner)
            {

            }

            /// <summary>
            /// 删除日志
            /// </summary>
            /// <param name="startTime">起始时间</param>
            /// <param name="endTime">结束时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult DeleteLogSr(DateTime startTime, DateTime endTime, CallingSettings settings = null)
            {
                return LogService.DeleteLog(Sc, new Log_DeleteLog_Request { StartTime = startTime, EndTime = endTime }, settings);
            }

            /// <summary>
            /// 删除日志
            /// </summary>
            /// <param name="startTime">起始时间</param>
            /// <param name="endTime">结束时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void DeleteLog(DateTime startTime, DateTime endTime, CallingSettings settings = null)
            {
                InvokeWithCheck(DeleteLogSr(startTime, endTime, settings));
            }

            /// <summary>
            /// 清空日志
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ClearLogSr(CallingSettings settings = null)
            {
                return DeleteLogSr(default(DateTime), default(DateTime), settings);
            }

            /// <summary>
            /// 清空日志
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void ClearLog(CallingSettings settings = null)
            {
                InvokeWithCheck(ClearLogSr(settings));
            }

            /// <summary>
            /// 读取日志
            /// </summary>
            /// <param name="startTime">起始时间</param>
            /// <param name="endTime">结束时间</param>
            /// <param name="start">起始位置</param>
            /// <param name="count">数量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Log_ReadLog_Reply> ReadLogSr(DateTime startTime, DateTime endTime, int start, int count, CallingSettings settings = null)
            {
                Log_ReadLog_Request req = new Log_ReadLog_Request { StartTime = startTime, EndTime = endTime, Start = start, Count = count };
                return LogService.ReadLog(Sc, req, settings);
            }

            /// <summary>
            /// 读取日志
            /// </summary>
            /// <param name="startTime">起始时间</param>
            /// <param name="endTime">结束时间</param>
            /// <param name="start">起始位置</param>
            /// <param name="count">数量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Log_ReadLog_Reply ReadLog(DateTime startTime, DateTime endTime, int start, int count, CallingSettings settings = null)
            {
                return InvokeWithCheck(ReadLogSr(startTime, endTime, start, count, settings));
            }

            /// <summary>
            /// 获取当前日志分析结果
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<LogAnalyzeResult> GetAnalyzeResultSr(CallingSettings settings = null)
            {
                var sr = LogService.GetAnalyzeResult(Sc, settings);
                return CreateSr(sr, r => r.Result);
            }

            /// <summary>
            /// 获取当前日志分析结果
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public LogAnalyzeResult GetAnalyzeResult(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAnalyzeResultSr(settings));
            }
        }
    }
}
