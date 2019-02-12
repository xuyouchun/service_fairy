using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// 时间工具类
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class TimeUtility
    {
        static TimeUtility()
        {
            
        }

        /// <summary>
        /// 判断该时间是否为空值（将default(DateTime)作为空值）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DateTime dt)
        {
            return dt == default(DateTime);
        }

        /// <summary>
        /// 判断该时间间隔是否为空值（将default(TimeSpan)作为空值）
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool IsEmpty(this TimeSpan ts)
        {
            return ts == default(TimeSpan);
        }
    }
}
