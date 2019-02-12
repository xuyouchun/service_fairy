using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;
using Common.Contracts.Service;

namespace Common.Package.Log
{
    /// <summary>
    /// 基于控制台输出的LogWriter
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    public class ConsoleLogWriter<TLogItem> : MarshalByRefObjectEx, ILogWriter<TLogItem>
        where TLogItem : class, ILogItem
    {
        public void Write(IEnumerable<TLogItem> items)
        {
            foreach (TLogItem item in items)
            {
                Console.WriteLine(item);
            }
        }

        public override int GetHashCode()
        {
            return typeof(TLogItem).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == typeof(ConsoleLogWriter<TLogItem>);
        }

        public static readonly ConsoleLogWriter<TLogItem> Instance = new ConsoleLogWriter<TLogItem>();
    }
}
