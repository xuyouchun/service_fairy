using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 空的日志记录器
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    public class EmptyLogWriter<TLogItem> : MarshalByRefObjectEx, ILogWriter<TLogItem> where TLogItem : class, ILogItem
    {
        private EmptyLogWriter()
        {

        }

        public void Write(IEnumerable<TLogItem> items)
        {

        }

        public static readonly EmptyLogWriter<TLogItem> Instance = new EmptyLogWriter<TLogItem>();
    }
}
