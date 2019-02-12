using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 空的日志读取器
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    public class EmptyLogReader<TLogItem> : MarshalByRefObjectEx, ILogReader<TLogItem> where TLogItem : class, ILogItem
    {
        public LogItemGroup[] ReadRootGroups()
        {
            return Array<LogItemGroup>.Empty;
        }

        public LogItemGroup[] ReadGroups(string groupName)
        {
            return Array<LogItemGroup>.Empty;
        }

        public TLogItem[] ReadItems(string groupName)
        {
            return Array<TLogItem>.Empty;
        }

        public LogItemGroup[] ReadGroupsByTime(DateTime start, DateTime end)
        {
            return Array<LogItemGroup>.Empty;
        }

        public static readonly EmptyLogReader<TLogItem> Instance = new EmptyLogReader<TLogItem>();

        public void DeleteGroup(string group)
        {
            
        }
    }
}
