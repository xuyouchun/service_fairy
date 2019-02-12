using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Common.Utility;

namespace Common.Algorithms
{
    /// <summary>
    /// 算法集合
    /// </summary>
    public unsafe static class Algorithm
    {
        /// <summary>
        /// 按折半算法查找元素的顺序，如果未找到则返回-1
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">元素列表</param>
        /// <param name="item">需要查找的元素</param>
        /// <param name="listSortType">列表顺序</param>
        /// <returns>元素所在的位置，如果未找到则返回-1</returns>
        public static int BinarySearch<T>(IList<T> list, T item, Comparison<T> comparison, SortType listSortType = SortType.Asc)
        {
            Contract.Requires(list != null && comparison != null && item != null);

            int left = 0, right = list.Count - 1, mid;
            if (listSortType == SortType.Asc) // 升序列表
            {
                while (left <= right)
                {
                    mid = (left + right) / 2;
                    T item0 = list[mid];

                    int result = comparison(item0, item);
                    if (result == 0)
                        return mid;

                    if (result < 0)
                        left = mid + 1;
                    else
                        right = mid - 1;
                }
            }
            else // 降序列表
            {
                while (left <= right)
                {
                    mid = (left + right) / 2;
                    T item0 = list[mid];

                    int result = comparison(item0, item);
                    if (result == 0)
                        return mid;

                    if (result < 0)
                        right = mid - 1;
                    else
                        left = mid + 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// 按折半算法查找指定的元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">列表类型</param>
        /// <param name="item">元素</param>
        /// <param name="listSortType">排序方式</param>
        /// <returns>元素所在的位置，如果未找到则返回-1</returns>
        public static int BinarySearch<T>(IList<T> list, T item, SortType listSortType = SortType.Asc) where T : IComparable<T>
        {
            Contract.Requires(list != null && item != null);

            return BinarySearch<T>(list, item, (x, y) => x.CompareTo(y), listSortType);
        }

        /// <summary>
        /// 按折半算法查找指定的元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">键</param>
        /// <param name="keySelector">键选择器</param>
        /// <param name="comparison">比较器</param>
        /// <param name="listSortType">排序方式</param>
        /// <returns>元素所在的位置，如果未找到则返回-1</returns>
        public static int BinarySearch<T, TKey>(IList<T> list, TKey key, Func<T, TKey> keySelector, Comparison<TKey> comparison, SortType listSortType = SortType.Asc)
        {
            Contract.Requires(list != null && comparison != null && key != null && keySelector != null);

            int left = 0, right = list.Count - 1, mid;
            if (listSortType == SortType.Asc) // 升序列表
            {
                while (left <= right)
                {
                    mid = (left + right) / 2;
                    T item0 = list[mid];

                    int result = comparison(keySelector(item0), key);
                    if (result == 0)
                        return mid;

                    if (result < 0)
                        left = mid + 1;
                    else
                        right = mid - 1;
                }
            }
            else // 降序列表
            {
                while (left <= right)
                {
                    mid = (left + right) / 2;
                    T item0 = list[mid];

                    int result = comparison(keySelector(item0), key);
                    if (result == 0)
                        return mid;

                    if (result < 0)
                        right = mid - 1;
                    else
                        left = mid + 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// 按折半算法查找指定的元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <param name="list">元素列表</param>
        /// <param name="key">键</param>
        /// <param name="keySelector">键选择器</param>
        /// <param name="listSortType">排序类型</param>
        /// <returns>位置，如果未找到则返回-1</returns>
        public static int BinarySearch<T, TKey>(IList<T> list, TKey key, Func<T, TKey> keySelector, SortType listSortType = SortType.Asc) where TKey : IComparable<TKey>
        {
            return BinarySearch<T, TKey>(list, key, keySelector, (x, y) => x.CompareTo(y), listSortType);
        }
    }
}
