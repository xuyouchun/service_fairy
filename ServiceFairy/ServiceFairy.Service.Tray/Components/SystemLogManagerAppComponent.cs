using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Tray;
using Common.Utility;
using Common;

namespace ServiceFairy.Service.Tray.Components
{
    /// <summary>
    /// 系统日志管理器
    /// </summary>
    [AppComponent("系统日志管理器", "查询系统日志")]
    class SystemLogManagerAppComponent : AppComponent
    {
        public SystemLogManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 获取系统日志的分组
        /// </summary>
        /// <returns></returns>
        public SystemLogGroup[] GetGroups()
        {
            EventLog[] eLogs = EventLog.GetEventLogs();
            return eLogs.ToArray(eLog => new SystemLogGroup {
                Name = eLog.Log, Title = eLog.LogDisplayName, Count = (eLog.Entries == null) ? 0 : eLog.Entries.Count,
            });
        }

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="group">组</param>
        /// <param name="start">起始位置</param>
        /// <param name="count">数量</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public SystemLogItem[] GetLogs(string group, int start, int count, out int totalCount)
        {
            EventLog eLog = EventLog.GetEventLogs().FirstOrDefault(e => e.Log == group);
            if (eLog == null || eLog.Entries == null)
                goto _empty;

            totalCount = eLog.Entries.Count;
            return eLog.Entries.Cast<EventLogEntry>().OrderByDescending(v => v.TimeWritten).Range(e => new SystemLogItem {
                Message = e.Message, Time = e.TimeWritten.ToUniversalTime(), Type = e.EntryType, Source = e.Source,
            }, start, count).ToArray();

        _empty:
            totalCount = 0;
            return Array<SystemLogItem>.Empty;
        }

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="group">组</param>
        /// <param name="start">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public SystemLogItem[] GetLogs(string group, int start, int count)
        {
            int totalCount;
            return GetLogs(group, start, count, out totalCount);
        }

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="group">组</param>
        /// <param name="totalCount">数量</param>
        /// <returns></returns>
        public SystemLogItem[] GetLogs(string group, out int totalCount)
        {
            return GetLogs(group, 0, int.MaxValue, out totalCount);
        }

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="group">组</param>
        /// <returns></returns>
        public SystemLogItem[] GetLogs(string group)
        {
            int totalCount;
            return GetLogs(group, out totalCount);
        }
    }
}
