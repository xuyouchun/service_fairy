using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Contracts.Log;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Package.Log
{
    /// <summary>
    /// 日志记录的辅助工具
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class LoggerHelper
    {
        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="logger">记录器</param>
        /// <param name="category">类别</param>
        /// <param name="message">消息</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        public static void LogError(this ILogger<LogItem> logger, string message, string category, string detail = "", string source = "")
        {
            Contract.Requires(logger != null);

            logger.Log(LogItem.FromError(category, message, detail, source));
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="logger">记录器</param>
        /// <param name="error">异常</param>
        /// <param name="category">类别</param>
        /// <param name="source">源</param>
        public static void LogError(this ILogger<LogItem> logger, Exception error, string category, string source = "")
        {
            Contract.Requires(logger != null);

            if (error != null)
            {
                logger.Log(LogItem.FromError(category, error.Message, error.ToString(), source));
            }
        }

        /// <summary>
        /// 记录普通消息
        /// </summary>
        /// <param name="logger">记录器</param>
        /// <param name="category">类别</param>
        /// <param name="message">消息</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        public static void LogMessage(this ILogger<LogItem> logger, string message, string category, string detail = "", string source = "")
        {
            Contract.Requires(logger != null);

            logger.Log(LogItem.FromMessage(category, message, detail, source));
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="logger">记录器</param>
        /// <param name="category">类别</param>
        /// <param name="warning">警告信息</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        public static void LogWarning(this ILogger<LogItem> logger, string warning, string category, string detail = "", string source = "")
        {
            Contract.Requires(logger != null);

            logger.Log(LogItem.FromWarning(category, warning, detail, source));
        }
    }
}
