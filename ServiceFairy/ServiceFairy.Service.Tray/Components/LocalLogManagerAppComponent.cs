using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;
using Common.Package;
using Common.Framework.TrayPlatform;
using Common.Contracts.Log;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.Tray.Components
{
    [AppComponent("本地日志管理器")]
    class LocalLogManagerAppComponent : AppComponent
    {
        public LocalLogManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
            _logManager = service.Context.LogManager;
        }

        private readonly Service _service;
        private readonly ITrayLogManager _logManager;
        private readonly Cache<string, LogGroupStat> _logStats = new Cache<string, LogGroupStat>();

        class LogGroupStat
        {
            //public DateTime LastModify;
            public string Name = null;
            //public int Count;
        }

        /// <summary>
        /// 获取本地日志组
        /// </summary>
        /// <param name="parentGroup"></param>
        /// <returns></returns>
        public LogItemGroup[] GetLocalLogGroups(string parentGroup)
        {
            LogItemGroup[] logItemGroups = _logManager.Reader.ReadGroups(parentGroup);
            return logItemGroups;
        }

        /// <summary>
        /// 获取指定组的日志
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public LogItem[] GetLocalLogItems(string group)
        {
            LogItem[] logItems = _logManager.Reader.ReadItems(group);
            return logItems;
        }

        /// <summary>
        /// 获取指定时间范围内的日志
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="count">最大数量</param>
        /// <returns></returns>
        public LogItem[] GetLocalLogItemsByTime(DateTime start, DateTime end, int count)
        {
            List<LogItem> items = new List<LogItem>();
            LogItemGroup[] groups = _logManager.Reader.ReadGroupsByTime(start, end);
            foreach (LogItem logItem in _ReadLogItems(groups, start, end))
            {
                items.Add(logItem);
                if (count > 0 && items.Count >= count)
                    break;
            }

            return items.ToArray();
        }

        // 读取指定时间范围之内的日志，并按时间顺序排列
        private IEnumerable<LogItem> _ReadLogItems(LogItemGroup[] groups, DateTime start, DateTime end)
        {
            List<LogItem> list = new List<LogItem>();
            foreach (LogItemGroup g in groups.OrderBy(g => g.CreationTime))
            {
                LogItem[] items = _logManager.Reader.ReadItems(g.Name);
                foreach (LogItem item in items.OrderBy(it => it.Time))
                {
                    if ((start == default(DateTime) || start <= item.Time) && (end == default(DateTime) || end <= item.Time))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// 删除日志组
        /// </summary>
        /// <param name="groups"></param>
        public void DeleteLogGroups(string[] groups)
        {
            Contract.Requires(groups != null);

            foreach (string group in groups)
            {
                _logManager.Reader.DeleteGroup(group);
            }
        }

        /// <summary>
        /// 删除日志组
        /// </summary>
        /// <param name="group"></param>
        public void DeleteLogGroup(string group)
        {
            Contract.Requires(group != null);

            DeleteLogGroups(new[] { group });
        }
    }
}
