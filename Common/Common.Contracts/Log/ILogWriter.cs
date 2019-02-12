using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 日志记录策略
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    public interface ILogWriter<in TLogItem>
        where TLogItem : class, ILogItem
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="items"></param>
        void Write(IEnumerable<TLogItem> items);
    }
}
