using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public interface ITrayLogManager
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        ILogWriter<LogItem> Writer { get; }

        /// <summary>
        /// 读取日志
        /// </summary>
        ILogReader<LogItem> Reader { get; }
    }
}
