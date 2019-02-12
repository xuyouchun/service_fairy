using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 日志项集合
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    public class LogItemCollection<TLogItem> : IEnumerable<TLogItem> where TLogItem : ILogItem
    {
        public LogItemCollection()
            : this("")
        {

        }

        public LogItemCollection(string defaultSource)
        {
            DefaultSource = defaultSource ?? string.Empty;
        }

        private readonly List<TLogItem> _logItems = new List<TLogItem>();

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="item"></param>
        public void Append(TLogItem item)
        {
            if (item == null)
                return;

            _logItems.Add(item);
        }

        /// <summary>
        /// 源
        /// </summary>
        public string DefaultSource { get; private set; }

        public IEnumerator<TLogItem> GetEnumerator()
        {
            return _logItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class LogItemCollection : LogItemCollection<LogItem>
    {
        public LogItemCollection()
        {

        }

        public LogItemCollection(string defaultSource)
            : base(defaultSource)
        {

        }
    }

    public static class LogItemCollectionUtility
    {
        private static string _GetSource(this LogItemCollection<LogItem> logCollection, string source)
        {
            if (logCollection == null)
                return source;

            if (string.IsNullOrEmpty(source))
                return logCollection.DefaultSource;

            return source;
        }

        public static void AppendError(this LogItemCollection<LogItem> logCollection, string category, Exception error, string source = "")
        {
            if (logCollection != null)
                logCollection.Append(LogItem.FromError(category, error, logCollection._GetSource(source)));
        }

        public static void AppendErrorFormat(this LogItemCollection<LogItem> logCollection, string category, string titleFormat, object[] args, string detail = "", string source = "")
        {
            if (logCollection != null)
                logCollection.Append(LogItem.FromErrorFormat(category, titleFormat, args, detail, logCollection._GetSource(source)));
        }

        public static void AppendErrorFormat(this LogItemCollection<LogItem> logCollection, string category, string titleFormat, params object[] args)
        {
            if (logCollection != null)
                logCollection.Append(LogItem.FromErrorFormat(category, titleFormat, args));
        }

        public static void AppendWarning(this LogItemCollection<LogItem> logCollection, string category, string title, string detail = "", string source = "")
        {
            if (logCollection != null)
                logCollection.Append(LogItem.FromWarning(category, title, detail, logCollection._GetSource(source)));
        }

        public static void AppendWarningFormat(this LogItemCollection<LogItem> logCollection, string category, string titleFormat, object[] args, string detail = "", string source = "")
        {
            if (logCollection != null)
                logCollection.Append(LogItem.FromWarningFormat(category, titleFormat, args, detail, logCollection._GetSource(source)));
        }

        public static void AppendWarningFormat(this LogItemCollection<LogItem> logCollection, string category, string titleFormat, params object[] args)
        {
            if (logCollection != null)
                logCollection.Append(LogItem.FromWarningFormat(category, titleFormat, args));
        }

        public static void AppendMessage(this LogItemCollection<LogItem> logCollection, string category, string title, string detail = "", string source = "")
        {
            if (logCollection != null)
                logCollection.Append(LogItem.FromMessage(category, title, detail, logCollection._GetSource(source)));
        }

        public static void AppendMessageFormat(this LogItemCollection<LogItem> logCollection, string category, string titleFormat, object[] args, string detail = "", string source = "")
        {
            if (logCollection != null)
                logCollection.Append(LogItem.FromMessageFormat(category, titleFormat, args, detail, logCollection._GetSource(source)));
        }

        public static void AppendMessageFormat(this LogItemCollection<LogItem> logCollection, string category, string titleFormat, params object[] args)
        {
            if (logCollection != null)
                logCollection.Append(LogItem.FromMessageFormat(category, titleFormat, args));
        }
    }
}
