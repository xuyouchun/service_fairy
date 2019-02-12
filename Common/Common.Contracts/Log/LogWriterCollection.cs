using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 日志写入器的集合
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    public class LogWriterCollection<TLogItem> : MarshalByRefObjectEx, ILogWriter<TLogItem> where TLogItem : class, ILogItem
    {
        public LogWriterCollection(ILogWriter<TLogItem>[] writers)
        {
            _writers = writers ?? Array<ILogWriter<TLogItem>>.Empty;
        }

        private readonly ILogWriter<TLogItem>[] _writers;

        public void Write(IEnumerable<TLogItem> items)
        {
            for (int k = 0; k < _writers.Length; k++)
            {
                _writers[k].Write(items);
            }
        }
    }
}
