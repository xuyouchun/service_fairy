using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collection
{
    /// <summary>
    /// 只读列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyList<T> : IEnumerable<T>
    {
        /// <summary>
        /// 按索引读取
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T this[int index] { get; }

        /// <summary>
        /// 总数量
        /// </summary>
        int Count { get; }
    }
}
