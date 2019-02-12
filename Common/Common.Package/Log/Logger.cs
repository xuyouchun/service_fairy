using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Package.Log;
using Common.Contracts.Log;
using Common.Utility;

namespace Common.Package.Log
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    public class Logger<TLogItem> : ILogger<TLogItem>
        where TLogItem : class, ILogItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logWriter"></param>
        public Logger(ILogWriter<TLogItem> logWriter)
        {
            Contract.Requires(logWriter != null);

            _LogWriter = logWriter;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logDirectory">日志记录目录</param>
        /// <param name="maxFileSize">日志文件的最大尺寸</param>
        public Logger(string logDirectory, int maxFileSize = FileLogWriter<TLogItem>.DEFAULT_MAX_FILE_SIZE)
            : this(new FileLogWriter<TLogItem>(logDirectory, maxFileSize))
        {

        }

        private readonly ILogWriter<TLogItem> _LogWriter;

        /// <summary>
        /// 记录一条日志
        /// </summary>
        /// <param name="item"></param>
        public void Log(TLogItem item)
        {
            Contract.Requires(item != null);
            Log(new[] { item });
        }

        /// <summary>
        /// 批量记录日志
        /// </summary>
        /// <param name="items"></param>
        public void Log(IEnumerable<TLogItem> items)
        {
            Contract.Requires(items != null);
            _LogWriter.Write(items.WhereNotNull());
        }
    }
}
