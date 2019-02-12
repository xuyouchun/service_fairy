using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;
using System.Diagnostics.Contracts;

namespace Common.Package.Log
{
    /// <summary>
    /// 日志记录器的集合
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    public class DynamicLogWriterCollection<TLogItem> : ILogWriter<TLogItem>, IDisposable
        where TLogItem : class, ILogItem
    {
        private ILogWriter<TLogItem>[] _logWriters = new ILogWriter<TLogItem>[0];

        public void Write(IEnumerable<TLogItem> items)
        {
            foreach (ILogWriter<TLogItem> writer in _logWriters)
            {
                if (writer != null)
                    writer.Write(items);
            }
        }

        public void RegisterLogWriter(ILogWriter<TLogItem> writer)
        {
            Contract.Requires(writer != null);
            if (_logWriters.Contains(writer))
                return;

            Array.Resize<ILogWriter<TLogItem>>(ref _logWriters, _logWriters.Length + 1);
            _logWriters[_logWriters.Length - 1] = writer;
        }

        public void Dispose()
        {
            foreach (ILogWriter<TLogItem> writer in _logWriters)
            {
                IDisposable dis = writer as IDisposable;
                if (dis != null)
                    dis.Dispose();
            }
        }
    }
}
