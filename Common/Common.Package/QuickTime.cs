using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Package
{
    /// <summary>
    /// 快速获取当前时间
    /// </summary>
    public static class QuickTime
    {
        static QuickTime()
        {
            _Refresh();
            GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(1), new TaskFuncAdapter(_Refresh), false);
        }

        private static object _utcTime, _localTime;  // 使用object是为了使用装箱原理实现线程安全

        private static void _Refresh()
        {
            _utcTime = DateTime.UtcNow;
            _localTime = ((DateTime)_utcTime).ToLocalTime();
        }

        /// <summary>
        /// 标准时间
        /// </summary>
        public static DateTime UtcNow
        {
            get { return (DateTime)_utcTime; }
        }

        /// <summary>
        /// 本地时间
        /// </summary>
        public static DateTime Now
        {
            get { return (DateTime)_localTime; }
        }
    }
}
