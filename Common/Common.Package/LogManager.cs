using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;
using Common.Package.Log;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Package
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class LogManager
    {
        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="error">错误</param>
        /// <param name="category">类别</param>
        /// <param name="source">源</param>
        public static void LogError(this Exception error, string category = "", string source = "")
        {
            if (error == null)
                return;

            _default.LogError(error, _GetCategory(category, error.StackTrace), source: _ReviseSource(source));
        }

        private static string _GetCategory(string category, string stackTrace = "")
        {
            if (!string.IsNullOrEmpty(category))
                return category;

            if (string.IsNullOrEmpty(stackTrace))
                stackTrace = Environment.StackTrace;

            return stackTrace.GetHashCode().ToString();
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="errorMsg">错误</param>
        /// <param name="category">类别</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        public static void LogError(string errorMsg, string category = "", string detail = "", string source = "")
        {
            if (errorMsg == null)
                return;

            _default.LogError(errorMsg, _GetCategory(category), detail, source);
        }

        /// <summary>
        /// 记录普通消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="category">类别</param>
        /// <param name="source">源</param>
        public static void LogMessage(string message, string category = "", string source = "")
        {
            if (message == null)
                return;

            _default.LogMessage(message, _GetCategory(category), source: _ReviseSource(source));
        }

        /// <summary>
        /// 记录普通消息
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="msgFormat">消息格式</param>
        /// <param name="args">消息参数</param>
        public static void LogMessageFormat(string category, string msgFormat, params object[] args)
        {
            Contract.Requires(msgFormat != null);

            LogMessageFormat(category, msgFormat, args, "");
        }

        /// <summary>
        /// 记录普通消息
        /// </summary>
        /// <param name="msgFormat">消息格式</param>
        /// <param name="args">消息参数</param>
        /// <param name="category">类别</param>
        /// <param name="source">源</param>
        public static void LogMessageFormat(string msgFormat, object[] args, string category, string source)
        {
            LogMessage(string.Format(msgFormat, args), category, source);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="warning">警告信息</param>
        /// <param name="category">类别</param>
        /// <param name="source">源</param>
        public static void LogWarning(string warning, string category, string source = "")
        {
            if (warning == null)
                return;

            _default.LogWarning(warning, _GetCategory(category), source: _ReviseSource(source));
        }


        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="msgFormat">消息格式</param>
        /// <param name="args">消息参数</param>
        public static void LogWarningFormat(string category, string msgFormat, params object[] args)
        {
            Contract.Requires(msgFormat != null);

            LogWarningFormat(category, msgFormat, args, "");
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="msgFormat">消息格式</param>
        /// <param name="args">消息参数</param>
        /// <param name="category">类别</param>
        /// <param name="source">源</param>
        public static void LogWarningFormat(string msgFormat, object[] args, string category, string source)
        {
            LogMessage(string.Format(msgFormat, args), category, source);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="item"></param>
        public static void Log(LogItem item)
        {
            if (item != null)
                _default.Log(_Revise(item));
        }

        /// <summary>
        /// 批量记录日志
        /// </summary>
        /// <param name="items"></param>
        public static void Log(IEnumerable<LogItem> items)
        {
            if (items != null)
                _default.Log(items.WhereNotNull(item => _Revise(item)));
        }

        /// <summary>
        /// 批量记录日志
        /// </summary>
        /// <param name="items"></param>
        public static void Log(this LogItemCollection<LogItem> items)
        {
            Log((IEnumerable<LogItem>)items);
        }

        private static readonly DynamicLogWriterCollection<LogItem> _logWriters = new DynamicLogWriterCollection<LogItem>();
        private static readonly Logger<LogItem> _default = new Logger<LogItem>(_logWriters);

        /// <summary>
        /// 注册日志记录器
        /// </summary>
        /// <param name="logWritter"></param>
        public static void RegisterLogWritter(ILogWriter<LogItem> logWritter)
        {
            Contract.Requires(logWritter != null);
            _logWriters.RegisterLogWriter(logWritter);
        }

        /// <summary>
        /// 注册文件日志记录器
        /// </summary>
        /// <param name="directory"></param>
        public static void RegisterFileLogWriter(string directory)
        {
            Contract.Requires(directory != null);

            _logWriters.RegisterLogWriter(new FileLogWriter<LogItem>(directory));
        }

        /// <summary>
        /// 添加控制台输出窗口
        /// </summary>
        public static void RegisterConsoleWindowWriter()
        {
            RegisterLogWritter(ConsoleLogWriter<LogItem>.Instance);
        }

        /// <summary>
        /// 设置默认的事件源
        /// </summary>
        /// <param name="defaultSource"></param>
        public static void SetDefaultSource(string defaultSource)
        {
            _defaultSource = defaultSource ?? "";
        }

        private static string _defaultSource = "";

        private static LogItem _Revise(LogItem item)
        {
            if (item == null)
                return null;

            item.Source = _ReviseSource(item.Source);
            return item;
        }

        private static string _ReviseSource(string source)
        {
            if (string.IsNullOrEmpty(source))
                return _defaultSource;

            return source;
        }
    }
}
