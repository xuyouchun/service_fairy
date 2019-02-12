using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 对日志记录的封装
    /// </summary>
    public interface ILogRecorder
    {
        /// <summary>
        /// 写入一条日志
        /// </summary>
        /// <param name="logItems"></param>
        void Write(IEnumerable<LogItem> logItems);
    }
}
