using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collection
{
    /// <summary>
    /// 支持批操作的Dictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IDictionaryRange<TKey, TValue>
    {
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="items"></param>
        /// <param name="ignoreDupKey"></param>
        void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items, bool ignoreDupKey = false);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys"></param>
        int RemoveRange(IEnumerable<TKey> keys);

        /// <summary>
        /// 批量获取
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        KeyValuePair<TKey, TValue>[] GetRange(IEnumerable<TKey> keys);
    }
}
