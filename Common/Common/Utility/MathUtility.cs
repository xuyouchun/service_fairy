using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Utility
{
    /// <summary>
    /// 数学工具
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class MathUtility
    {
        /// <summary>
        /// 返回两个数的最大值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static T Max<T>(T value1, T value2) where T : struct, IComparable<T>
        {
            return value1.CompareTo(value2) >= 0 ? value1 : value2;
        }

        /// <summary>
        /// 返回两个数的最小值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static T Min<T>(T value1, T value2) where T : struct, IComparable<T>
        {
            return value1.CompareTo(value2) <= 0 ? value1 : value2;
        }
    }
}
