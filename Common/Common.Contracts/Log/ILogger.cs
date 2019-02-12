using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 日志记录器接口
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    public interface ILogger<TLogItem>
        where TLogItem : ILogItem
    {
        /// <summary>
        /// 记录一条日志
        /// </summary>
        /// <param name="item"></param>
        void Log(TLogItem item);

        /// <summary>
        /// 批量记录日志
        /// </summary>
        /// <param name="items"></param>
        void Log(IEnumerable<TLogItem> items);
    }
}
