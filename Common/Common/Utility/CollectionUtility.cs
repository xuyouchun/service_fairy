using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Collection;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace Common.Utility
{
    /// <summary>
    /// 集合的工具类
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class CollectionUtility
    {
        /// <summary>
        /// 从指定的哈希表中寻找指定键的值，若寻找不到则返回默认值
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dict">哈希表</param>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            if (dict == null || key == null || !dict.TryGetValue(key, out value))
                return defaultValue;

            return value;
        }

        /// <summary>
        /// 批量获限
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dict">哈希表</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TValue>[] GetRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<TKey> keys)
        {
            Contract.Requires(dict != null && keys != null);

            IDictionaryRange<TKey, TValue> dictRange = dict as IDictionaryRange<TKey, TValue>;

            if (dictRange != null)
            {
                return dictRange.GetRange(keys);
            }
            else
            {
                List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
                foreach (TKey key in keys)
                {
                    TValue value;
                    if (dict.TryGetValue(key, out value))
                        list.Add(new KeyValuePair<TKey, TValue>(key, value));
                }

                return list.ToArray();
            }
        }

        /// <summary>
        /// 以线程安全的方式清除哈希表
        /// </summary>
        /// <param name="dict">哈希表</param>
        public static void SafeClear(this IDictionary dict)
        {
            if (dict == null || dict.Count == 0)
                return;

            lock (dict)
            {
                dict.Clear();
            }
        }

        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="TSource">源元素类型</typeparam>
        /// <typeparam name="TResult">目标元素类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="converter">转换器</param>
        /// <returns></returns>
        public static TResult[] ToArray<TSource, TResult>(this IList<TSource> collection, Func<TSource, TResult> converter)
        {
            Contract.Requires(converter != null);

            if (collection == null)
                return null;

            TResult[] results = new TResult[collection.Count];
            for (int k = 0, length = collection.Count; k < length; k++)
            {
                results[k] = converter(collection[k]);
            }

            return results;
        }

        /// <summary>
        /// 集合转换为去重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collection"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> SelectDistinct<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> converter)
        {
            Contract.Requires(converter != null);

            if (collection == null)
                return null;

            return collection.Select(converter).Distinct();
        }

        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collection"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> converter)
        {
            Contract.Requires(converter != null);

            if (collection == null)
                return null;

            return collection.Select(item => converter(item)).ToArray();
        }

        /// <summary>
        /// 转换为顺序相反的Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] ToReverseArray<T>(this IList<T> list)
        {
            if (list.IsNullOrEmpty())
                return Array<T>.Empty;

            T[] arr = new T[list.Count];
            for (int i = list.Count - 1, j = 0; i >= 0; i--, j++)
            {
                arr[j] = arr[i];
            }

            return arr;
        }

        /// <summary>
        /// 转换为数组，在转换之前先锁定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T[] SafeToArray<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                return Array<T>.Empty;

            lock (collection)
            {
                return collection.ToArray();
            }
        }

        /// <summary>
        /// 转换为数组，在转换之前先锁定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="clear"></param>
        /// <returns></returns>
        public static T[] SafeToArray<T>(this ICollection<T> collection, bool clear = false)
        {
            if (collection == null || collection.Count == 0)
                return Array<T>.Empty;

            lock (collection)
            {
                T[] array = new T[collection.Count];
                collection.CopyTo(array, 0);

                if (clear)
                    collection.Clear();

                return array;
            }
        }

        /// <summary>
        /// 转换为数组，在转换之前先锁定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hs"></param>
        /// <param name="clear"></param>
        /// <param name="trimExcess"></param>
        /// <returns></returns>
        public static T[] SafeToArray<T>(this HashSet<T> hs, bool clear = false, bool trimExcess = false)
        {
            T[] arr = SafeToArray<T>((ICollection<T>)hs, clear);
            if (trimExcess && hs != null)
                hs.TrimExcess();

            return arr;
        }

        /// <summary>
        /// 转换为数组，并剔除为空的元素
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collection"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static TResult[] ToArrayNotNull<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> converter)
        {
            Contract.Requires(converter != null);

            if (collection == null)
                return null;

            return collection.WhereNotNull().Select(item => converter(item)).WhereNotNull().ToArray();
        }

        /// <summary>
        /// 转换为数组，并剔除为空的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T[] ToArrayNotNull<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                return null;

            return collection.WhereNotNull().ToArray();
        }

        /// <summary>
        /// 转换为只读集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                return new ReadOnlyCollection<T>(Array<T>.Empty);

            IList<T> list = (collection as List<T>);
            if (list != null)
                return new ReadOnlyCollection<T>(list);

            return new ReadOnlyCollection<T>(collection.ToList());
        }

        /// <summary>
        /// 从集合中的每个元素中提取出更多的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> collection)
        {
            if (collection == null)
                return null;

            return collection.SelectMany(v => v);
        }

        /// <summary>
        /// 从集合中的每个元素中提取出更多的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IList<T>> collection)
        {
            if (collection == null)
                return null;

            return collection.SelectMany(v => v);
        }

        /// <summary>
        /// 从集合中的每个元素中提取出更多的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectMany<T>(this IEnumerable<T[]> collection)
        {
            if (collection == null)
                return null;

            return collection.SelectMany(v => v);
        }

        /// <summary>
        /// 是否为空引用或空数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// 是否为空引用或空数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// 列表数量，如果为空则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int CountOrDefault<T>(this IList<T> list, int defaultValue = 0)
        {
            if (list == null)
                return defaultValue;

            return list.Count;
        }

        /// <summary>
        /// 集合数量，如果为空则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int CountOrDefault<T>(this IEnumerable<T> collection, int defaultValue = 0)
        {
            if (collection == null)
                return defaultValue;

            return collection.Count();
        }

        /// <summary>
        /// 从Dictionary中读取指定键的值，如果不存在则调用creator来创建
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static TValue GetOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> creator)
        {
            Contract.Requires(dict != null && key != null && creator != null);

            TValue value;
            if (dict.TryGetValue(key, out value))
                return value;

            lock (dict)
            {
                if (!dict.TryGetValue(key, out value))
                {
                    dict.Add(key, value = creator(key));
                }
            }

            return value;
        }

        /// <summary>
        /// 从Dictionary中读取指定键的值，如果不存在则创建
        /// </summary>
        /// <typeparam name="TKey">键</typeparam>
        /// <typeparam name="TValue">值</typeparam>
        /// <param name="dict">Dictionary</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static TValue GetOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            return GetOrSet(dict, key, (item) => new TValue());
        }

        /// <summary>
        /// 从指定的Dictionary中读取指定的值，如果不存在则调用creator来创建
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <typeparam name="TArg">其它参数类型</typeparam>
        /// <param name="dict">哈希表</param>
        /// <param name="key">键</param>
        /// <param name="arg">其它参数</param>
        /// <param name="creator">创建器</param>
        /// <returns></returns>
        public static TValue GetOrSet<TKey, TValue, TArg>(this IDictionary<TKey, TValue> dict, TKey key, TArg arg, Func<TKey, TArg, TValue> creator)
        {
            Contract.Requires(dict != null && key != null && creator != null);

            TValue value;
            if (dict.TryGetValue(key, out value))
                return value;

            lock (dict)
            {
                if (!dict.TryGetValue(key, out value))
                {
                    dict.Add(key, value = creator(key, arg));
                }
            }

            return value;
        }

        /// <summary>
        /// 从Dictionary删除指定键的元素
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dict">Dictionary</param>
        /// <param name="keys">要删除的键</param>
        /// <returns>已删除的个数</returns>
        public static int RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<TKey> keys)
        {
            Contract.Requires(dict != null && keys != null);

            IDictionaryRange<TKey, TValue> dictRange = dict as IDictionaryRange<TKey, TValue>;
            if (dictRange != null)
            {
                return dictRange.RemoveRange(keys);
            }
            else
            {
                int count = 0;
                foreach (TKey key in keys)
                {
                    if (dict.Remove(key))
                        count++;
                }

                return count;
            }
        }

        /// <summary>
        /// 将Dictionary转换为线程安全版本
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static ThreadSafeDictionaryWrapper<TKey, TValue> ToThreadSafe<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            Contract.Requires(dict != null);

            return dict as ThreadSafeDictionaryWrapper<TKey, TValue> ?? new ThreadSafeDictionaryWrapper<TKey, TValue>(dict);
        }

        /// <summary>
        /// 对集合中的元素执行指定的操作
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Contract.Requires(source != null && action != null);

            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// 寻找符合条件的元素的位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="condition"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int FindIndex<T>(this IList<T> list, Func<T, bool> condition, int start = 0)
        {
            Contract.Requires(list != null && condition != null);

            for (int k = start, len = list.Count; k < len; k++)
            {
                if (condition(list[k]))
                    return k;
            }

            return -1;
        }


        /// <summary>
        /// 从结尾向前寻找符合条件的元素的位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="condition"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int FindLastIndex<T>(this IList<T> list, Func<T, bool> condition, int start = 0)
        {
            Contract.Requires(list != null && condition != null);

            for (int k = list.Count - start - 1; k >= 0; k--)
            {
                if (condition(list[k]))
                    return k;
            }

            return -1;
        }

        /// <summary>
        /// 寻找指定元素的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int FindIndex<T>(this IEnumerable<T> collection, T item)
        {
            Contract.Requires(collection != null);

            int index = 0;
            foreach (T item0 in collection)
            {
                if (object.Equals(item0, item))
                    return index++;
            }

            return -1;
        }

        /// <summary>
        /// 寻找指定元素的索引位置
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        public static int FindIndex(this IEnumerable collection, object item)
        {
            Contract.Requires(collection != null);

            int index = 0;
            foreach (object item0 in collection)
            {
                if (object.Equals(item0, item))
                    return index;

                index++;
            }

            return -1;
        }

        /// <summary>
        /// 将集合转换为HashSet类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            if (source == null)
                return null;

            if (comparer == null)
            {
                HashSet<T> hashSet = source as HashSet<T>;
                if (hashSet != null)
                    return hashSet;
            }

            return new HashSet<T>(source, comparer);
        }

        /// <summary>
        /// 将集合转换为Queue类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return null;

            Queue<T> queue = source as Queue<T>;
            if (queue != null)
                return queue;

            return new Queue<T>(source);
        }

        /// <summary>
        /// 从队列中取出所有的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static T[] DequeueAll<T>(this Queue<T> queue)
        {
            if(queue==null)
                return null;

            T[] items = new T[queue.Count];
            for (int k = 0, length = items.Length; k < length; k++)
            {
                items[k] = queue.Dequeue();
            }
            return items;
        }

        /// <summary>
        /// 从队列中取出所有的项，线程安全
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static T[] SafeDequeueAll<T>(this Queue<T> queue)
        {
            lock (queue)
            {
                return DequeueAll<T>(queue);
            }
        }

        /// <summary>
        /// 将元素入队列，线程安全
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="item"></param>
        public static void SafeEnqueue<T>(this Queue<T> queue, T item)
        {
            Contract.Requires(queue != null);

            lock (queue)
            {
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// 将元素指入队列、线程安全
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="items"></param>
        public static void SafeEnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            Contract.Requires(queue != null && items != null);

            lock (queue)
            {
                queue.EnqueueRange(items);
            }
        }

        /// <summary>
        /// 从队列中取出指定最大数量的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T[] DequeueRange<T>(this Queue<T> queue, int max = int.MaxValue)
        {
            Contract.Requires(max > 0);

            if (queue == null)
                return null;

            T[] items = new T[Math.Min(queue.Count, max)];
            for (int k = 0, length = items.Length; k < length; k++)
            {
                items[k] = queue.Dequeue();
            }

            return items;
        }

        /// <summary>
        /// 从队列中取出所有的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static T[] DequeueAll<T>(this ConcurrentQueue<T> queue)
        {
            if (queue == null)
                return null;

            T[] items = new T[queue.Count];
            for (int k = 0, length = items.Length; k < length; k++)
            {
                queue.TryDequeue(out items[k]);
            }
            return items;
        }

        /// <summary>
        /// 从队列中取出指定数量的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T[] DequeueRange<T>(this ConcurrentQueue<T> queue, int max = int.MaxValue)
        {
            if (queue == null)
                return null;

            T[] items = new T[Math.Min(queue.Count, max)];
            for (int k = 0, length = items.Length; k < length; k++)
            {
                queue.TryDequeue(out items[k]);
            }

            return items;
        }

        /// <summary>
        /// 弹出所有项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static T[] PopAll<T>(this Stack<T> stack, bool reverse = false)
        {
            if (stack == null)
                return null;

            T[] arr = new T[stack.Count];
            if (!reverse)
            {
                for (int k = 0; k < arr.Length; k++)
                {
                    arr[k] = stack.Pop();
                }
            }
            else
            {
                for (int k = arr.Length - 1; k >= 0; k--)
                {
                    arr[k] = stack.Pop();
                }
            }

            return arr;
        }

        /// <summary>
        /// 将集合转换为Stack类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Stack<T> ToStack<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return null;

            Stack<T> stack = source as Stack<T>;
            if (stack != null)
                return stack;

            return new Stack<T>(source);
        }

        /// <summary>
        /// 截取指定范围之内元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static IEnumerable<T> Range<T>(this IEnumerable<T> source, int start, int count = int.MaxValue)
        {
            Contract.Requires(count >= 0 && start >= 0);

            IList<T> list = source as IList<T>;
            if (list != null)
            {
                for (int k = 0, maxCount = Math.Min(list.Count, count); k < maxCount; k++)
                {
                    yield return list[start + k];
                }
            }
            else
            {
                int index = 0, end = index + count;
                foreach (T item in source)
                {
                    if (index >= start)
                    {
                        if (index++ < end)
                            yield return item;
                        else
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// 截取指定范围之内元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="convert"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static IEnumerable<T> Range<TItem, T>(this IEnumerable<TItem> source, Func<TItem, T> convert, int start, int count = int.MaxValue)
        {
            Contract.Requires(count >= 0 && start >= 0);

            IList<TItem> list = source as IList<TItem>;
            if (list != null)
            {
                for (int k = 0, maxCount = Math.Min(list.Count, count); k < maxCount; k++)
                {
                    yield return convert(list[start + k]);
                }
            }
            else
            {
                int index = 0, end = index + count;
                foreach (TItem item in source)
                {
                    if (index >= start)
                    {
                        if (index++ < end)
                            yield return convert(item);
                        else
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] Cast<T>(this IList list)
        {
            if (list == null)
                return null;
            
            T[] items = new T[list.Count];

            for (int k = 0; k < list.Count; k++)
            {
                items[k] = (T)list[k];
            }

            return items;
        }

        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static T[] Cast<T>(this IList list, Func<object, T> converter)
        {
            if (list == null)
                return null;

            Contract.Requires(converter != null);
            T[] items = new T[list.Count];

            for (int k = 0; k < list.Count; k++)
            {
                items[k] = converter(list[k]);
            }

            return items;
        }

        /// <summary>
        /// 转换为String
        /// </summary>
        /// <param name="list"></param>
        /// <param name="defStr"></param>
        /// <returns></returns>
        public static string[] CastAsString(this IList list, string defStr = null)
        {
            return Cast<string>(list, obj => obj.ToStringIgnoreNull(defStr));
        }

        /// <summary>
        /// 转换为object数组
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static object[] CastAsObject(this IList list)
        {
            return Cast<object>(list, obj => obj);
        }

        /// <summary>
        /// 调转数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static void Reverse<T>(this IList<T> list, int start, int count)
        {
            Contract.Requires(list != null && start >= 0 && start + count < list.Count);

            int end = start + count - 1;
            while (start < end)
            {
                T item = list[start];
                list[start] = list[end];
                list[end] = item;
            }
        }

        /// <summary>
        /// 调转数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Reverse<T>(this IList<T> list)
        {
            Contract.Requires(list != null);

            Reverse<T>(list, 0, list.Count);
        }

        /// <summary>
        /// 截取指定范围之内的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] Range<T>(this IList<T> source, int start, int count = int.MaxValue)
        {
            Contract.Requires(count >= 0 && start >= 0);

            if (source == null)
                return null;

            T[] list = new T[Math.Max(Math.Min(count, source.Count - start), 0)];
            for (int k = 0; k < list.Length; k++)
            {
                list[k] = source[start + k];
            }

            return list;
        }

        /// <summary>
        /// 截取指定范围之内的元素
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="convert"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] Range<TItem, T>(this IList<TItem> source, Func<TItem, T> convert, int start, int count = int.MaxValue)
        {
            Contract.Requires(convert != null && count >= 0 && start >= 0);

            if (source == null)
                return null;

            T[] list = new T[Math.Max(Math.Min(count, source.Count - start), 0)];
            for (int k = 0; k < list.Length; k++)
            {
                list[k] = convert(source[start + k]);
            }

            return list;
        }

        /// <summary>
        /// 截取指定范围之内的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] Range<T>(this IReadOnlyList<T> source, int start, int count = int.MaxValue)
        {
            Contract.Requires(count >= 0 && start >= 0);

            if (source == null)
                return null;

            T[] list = new T[Math.Max(Math.Min(count, source.Count - start), 0)];
            for (int k = 0; k < list.Length; k++)
            {
                list[k] = source[start + k];
            }

            return list;
        }

        /// <summary>
        /// 截取指定范围之内的元素
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="convert"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] Range<TItem, T>(this IReadOnlyList<TItem> source, Func<TItem, T> convert, int start, int count = int.MaxValue)
        {
            Contract.Requires(convert != null && count >= 0 && start >= 0);

            if (source == null)
                return null;

            T[] list = new T[Math.Max(Math.Min(count, source.Count - start), 0)];
            for (int k = 0; k < list.Length; k++)
            {
                list[k] = convert(source[start + k]);
            }

            return list;
        }

        /// <summary>
        /// 将集合中的元素拷贝到指定的列表中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="destList"></param>
        /// <param name="arrayIndex"></param>
        public static void CopyTo<T>(this IEnumerable<T> source, IList<T> destList, int arrayIndex = 0)
        {
            Contract.Requires(source != null && destList != null && arrayIndex >= 0);

            foreach (T item in source)
            {
                destList[arrayIndex++] = item;
            }
        }

        /// <summary>
        /// 选择集合中不为空的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
        {
            Contract.Requires(source != null);

            return source.Where(v => v != null);
        }

        /// <summary>
        /// 选取不为空引用或空串的字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<string> WhereNotNullOrEmpty(this IEnumerable<string> source)
        {
            Contract.Requires(source != null);

            return source.Where(s => !string.IsNullOrEmpty(s));
        }

        /// <summary>
        /// 选取不为空引用或空白字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<string> WhereNotNullOrWhiteSpace(this IEnumerable<string> source)
        {
            Contract.Requires(source != null);

            return source.Where(s => !string.IsNullOrWhiteSpace(s));
        }

        /// <summary>
        /// 选择集合中不为空的元素，并转换为相应的类型
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static IEnumerable<TTarget> WhereNotNull<TSource, TTarget>(this IEnumerable<TSource> source, Func<TSource, TTarget> converter)
        {
            Contract.Requires(converter != null);
            if (source == null)
                return null;

            return source.WhereNotNull().Select(converter).WhereNotNull();
        }

        /// <summary>
        /// 将集合中符合条件的元素转换为数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static T[] WhereToArray<T>(this IEnumerable<T> source, Func<T, bool> filter)
        {
            Contract.Requires(filter != null);
            if (source == null)
                return null;

            List<T> list = new List<T>();

            foreach (T item in source)
            {
                if (filter(item))
                    list.Add(item);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 求指定集合的和
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static uint Sum(this IEnumerable<uint> collection)
        {
            Contract.Requires(collection != null);

            uint sum = default(uint);
            foreach (uint item in collection)
            {
                sum += item;
            }

            return sum;
        }

        /// <summary>
        /// 求最大值，如果集合为空，则返回默认值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int MaxOrDefault(this IEnumerable<int> collection, int defaultValue = default(int))
        {
            if (collection == null)
                return defaultValue;

            int max = int.MinValue;

            using (IEnumerator<int> enumerator = collection.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return defaultValue;

                do
                {
                    int cur = enumerator.Current;
                    if (cur > max)
                        max = cur;

                } while (enumerator.MoveNext());
            }

            return max;
        }

        /// <summary>
        /// 求最大值，如果集合为空，则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="valueSelector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int MaxOrDefault<T>(this IEnumerable<T> collection, Func<T, int> valueSelector, int defaultValue = default(int))
        {
            return collection.Select(valueSelector).MaxOrDefault(defaultValue);
        }

        /// <summary>
        /// 求最小值，如果集合为空，则返回默认值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int MinOrDefault(this IEnumerable<int> collection, int defaultValue = default(int))
        {
            if (collection == null)
                return defaultValue;
            
            int min = int.MaxValue;

            using (IEnumerator<int> enumerator = collection.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return defaultValue;

                do
                {
                    int cur = enumerator.Current;
                    if (cur < min)
                        min = cur;

                } while (enumerator.MoveNext());
            }

            return min;
        }

        /// <summary>
        /// 求最小值，如果集合为空，则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="valueSelector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int MinOrDefault<T>(this IEnumerable<T> collection, Func<T, int> valueSelector, int defaultValue = default(int))
        {
            if (collection == null)
                return defaultValue;

            return collection.Select(valueSelector).MinOrDefault(defaultValue);
        }

        /// <summary>
        /// 求指定集合的和
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static uint Sum<T>(this IEnumerable<T> collection, Func<T, uint> selector)
        {
            Contract.Requires(collection != null);

            uint sum = default(uint);
            foreach (T item in collection)
            {
                sum += selector(item);
            }

            return sum;
        }

        /// <summary>
        /// 向指定的Dictionary中批量添加元素
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="dict">Dictionary</param>
        /// <param name="items">需要添加的元素</param>
        /// <param name="keySelector">键选择器</param>
        /// <param name="valueSelector">值选择器</param>
        /// <returns>添加的个数</returns>
        public static void AddRange<TKey, TValue, T>(this IDictionary<TKey, TValue> dict,
            IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool ignoreDupKey = false)
        {
            Contract.Requires(dict != null && items != null && keySelector != null && valueSelector != null);

            IDictionaryRange<TKey, TValue> rangeDict = dict as IDictionaryRange<TKey, TValue>;
            if (rangeDict != null)
            {
                rangeDict.AddRange(items.Select(item => new KeyValuePair<TKey, TValue>(keySelector(item), valueSelector(item))), ignoreDupKey);
            }
            else
            {
                if (ignoreDupKey)
                {
                    foreach (T item in items)
                    {
                        dict[keySelector(item)] = valueSelector(item);
                    }
                }
                else
                {
                    foreach (T item in items)
                    {
                        dict.Add(keySelector(item), valueSelector(item));
                    }
                }
            }
        }

        /// <summary>
        /// 向指定的Dictionary中批量添加元素
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dict">Dictionary</param>
        /// <param name="items">需要添加的元素</param>
        /// <param name="keySelector">键选择器</param>
        /// <param name="clearFirst">是否在添加之前先清空Dictionary</param>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            IEnumerable<TValue> items, Func<TValue, TKey> keySelector, bool ignoreDupKey = false, bool clearFirst = false)
        {
            Contract.Requires(dict != null && items != null && keySelector != null);

            if (clearFirst)
                dict.Clear();

            IDictionaryRange<TKey, TValue> rangeDict = dict as IDictionaryRange<TKey, TValue>;
            if (rangeDict != null)
            {
                rangeDict.AddRange(items.Select(item => new KeyValuePair<TKey, TValue>(keySelector(item), item)), ignoreDupKey);
            }
            else
            {
                if (ignoreDupKey)
                {
                    foreach (TValue item in items)
                    {
                        dict[keySelector(item)] = item;
                    }
                }
                else
                {
                    foreach (TValue item in items)
                    {
                        dict.Add(keySelector(item), item);
                    }
                }
            }
        }

        /// <summary>
        /// 向哈希表中批量添加数据
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dict">哈希表</param>
        /// <param name="list">要添加的项</param>
        /// <param name="ignoreDupKey">是否忽略重复键</param>
        /// <param name="clearFirst">是否先清空哈希表</param>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            IEnumerable<KeyValuePair<TKey, TValue>> list, bool ignoreDupKey = false, bool clearFirst = false)
        {
            Contract.Requires(dict != null && list != null);

            if (clearFirst)
                dict.Clear();

            IDictionaryRange<TKey, TValue> rangeDict = dict as IDictionaryRange<TKey, TValue>;
            if (rangeDict != null)
            {
                rangeDict.AddRange(list, ignoreDupKey);
            }
            else
            {
                if (ignoreDupKey)
                {
                    foreach (KeyValuePair<TKey, TValue> item in list)
                    {
                        dict[item.Key] = item.Value;
                    }
                }
                else
                {
                    foreach (KeyValuePair<TKey, TValue> item in list)
                    {
                        dict.Add(item.Key, item.Value);
                    }
                }
            }
        }

        private static int _GetListLength<T>(IList<T> list)
        {
            return list == null ? 0 : list.Count;
        }

        /// <summary>
        /// 将两个数组组合在一起
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static T[] Concat<T>(this IList<T> list1, IList<T> list2)
        {
            int count = _GetListLength(list1) + _GetListLength(list2);

            if (count == 0)
                return Array<T>.Empty;

            T[] list = new T[count];

            int index = 0;
            if (list1 != null)
            {
                list1.CopyTo(list, index);
                index += list1.Count;
            }

            if (list2 != null)
            {
                list2.CopyTo(list, index);
            }

            return list;
        }

        /// <summary>
        /// 将两个数组组合在一起
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static T[] Concat<T>(this IList<T> list1, IList<T>[] lists)
        {
            int count = _GetListLength(list1);
            for (int k = 0; k < lists.Length; k++)
            {
                count += _GetListLength(lists[k]);
            }

            if (count == 0)
                return Array<T>.Empty;

            T[] list = new T[count];

            int index = 0;
            if (list1 != null)
            {
                list1.CopyTo(list, index);
                index += list1.Count;
            }

            for (int k = 0; k < lists.Length; k++)
            {
                lists[k].CopyTo(list, index);
                index += lists[k].Count;
            }

            return list;
        }

        /// <summary>
        /// 将一个Dictionary中的所有值添加到另一个Dictionary中
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dict"></param>
        /// <param name="dict2"></param>
        /// <param name="ignoreDupKey">是否忽略掉重复的键</param>
        /// <returns>已添加的元素个数</returns>
        public static int AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            IDictionary<TKey, TValue> dict2, bool ignoreDupKey = false, bool clearFirst = false)
        {
            Contract.Requires(dict != null && dict2 != null);

            if (clearFirst)
                dict.Clear();

            int count = 0;
            if (ignoreDupKey)
            {
                foreach (var item in dict2)
                {
                    dict[item.Key] = item.Value;
                    count++;
                }
            }
            else
            {
                foreach (var item in dict2)
                {
                    dict.Add(item.Key, item.Value);
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 将集合转化为Dictionary
        /// </summary>
        /// <typeparam name="TItem">集合元素类型</typeparam>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="keySelector">键选择器</param>
        /// <param name="valueSelector">值选择器</param>
        /// <param name="ignoreDupKeys">是否忽略掉重复的键</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TItem, TKey, TValue>(this IEnumerable<TItem> source,
            Func<TItem, TKey> keySelector, Func<TItem, TValue> valueSelector, bool ignoreDupKeys)
        {
            Contract.Requires(source != null && keySelector != null && valueSelector != null);

            if (!ignoreDupKeys)
                return source.ToDictionary(keySelector, valueSelector);

            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            foreach (var item in source)
            {
                TKey key = keySelector(item);
                if (key != null)
                    dict[key] = valueSelector(item);
            }

            return dict;
        }

        /// <summary>
        /// 将集合转化为Dictionary
        /// </summary>
        /// <typeparam name="TItem">元素类型</typeparam>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="keySelector">键选择器</param>
        /// <param name="ignoreDupKeys">是否忽略掉重复的键</param>
        /// <returns></returns>
        public static Dictionary<TKey, TItem> ToDictionary<TItem, TKey>(this IEnumerable<TItem> source,
            Func<TItem, TKey> keySelector, bool ignoreDupKeys)
        {
            Contract.Requires(source != null && keySelector != null);

            if (!ignoreDupKeys)
                return source.ToDictionary(keySelector);

            Dictionary<TKey, TItem> dict = new Dictionary<TKey, TItem>();
            foreach (var item in source)
            {
                TKey key = keySelector(item);
                if (key != null)
                    dict[key] = item;
            }

            return dict;
        }

        /// <summary>
        /// 转换为IgnoreCaseDictionary
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="ignoreDupKeys"></param>
        /// <returns></returns>
        public static IgnoreCaseDictionary<TItem> ToIgnoreCaseDictionary<TItem>(this IEnumerable<TItem> source,
            Func<TItem, string> keySelector, bool ignoreDupKeys = false)
        {
            Contract.Requires(source != null && keySelector != null);

            IgnoreCaseDictionary<TItem> dict = new IgnoreCaseDictionary<TItem>();
            foreach (var item in source)
            {
                string key = keySelector(item);

                if (ignoreDupKeys)
                    dict[key] = item;
                else
                    dict.Add(key, item);
            }

            return dict;
        }

        /// <summary>
        /// 转换为IgnoreCaseDictionary
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="valueSelector"></param>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="ignoreDupKeys"></param>
        /// <returns></returns>
        public static IgnoreCaseDictionary<TValue> ToIgnoreCaseDictionary<TItem, TValue>(this IEnumerable<TItem> source,
            Func<TItem, string> keySelector, Func<TItem, TValue> valueSelector, bool ignoreDupKeys = false)
        {
            Contract.Requires(source != null && keySelector != null && valueSelector != null);

            IgnoreCaseDictionary<TValue> dict = new IgnoreCaseDictionary<TValue>();
            foreach (var item in source)
            {
                string key = keySelector(item);
                TValue value = valueSelector(item);

                if (ignoreDupKeys)
                    dict[key] = value;
                else
                    dict.Add(key, value);
            }

            return dict;
        }

        /// <summary>
        /// 转换为哈希表
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, bool ignoreCase = false)
        {
            Contract.Requires(source != null);

            return source.ToDictionary(v => v.Key, v => v.Value, ignoreCase);
        }

        /// <summary>
        /// 转换为忽略大小写的HashSet
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IgnoreCaseHashSet ToIgnoreCaseHashSet(this IEnumerable<string> source)
        {
            Contract.Requires(source != null);

            IgnoreCaseHashSet hs = new IgnoreCaseHashSet();
            foreach (var item in source)
            {
                hs.Add(item);
            }

            return hs;
        }

        /// <summary>
        /// 向队列中批量添加元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static int EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            Contract.Requires(queue != null && items != null);

            int count = 0;
            foreach (T item in items)
            {
                queue.Enqueue(item);
                count++;
            }

            return count;
        }

        /// <summary>
        /// 向堆栈中批量添加元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static int PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            Contract.Requires(stack != null && items != null);

            int count = 0;
            foreach (T item in items)
            {
                stack.Push(item);
                count++;
            }

            return count;
        }

        /// <summary>
        /// 向HashSet中批量添加元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashSet"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static int AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> items)
        {
            Contract.Requires(hashSet != null && items != null);

            int count = 0;
            foreach (T item in items)
            {
                hashSet.Add(item);
                count++;
            }

            return count;
        }

        /// <summary>
        /// 以线程安全方式向动态数组中添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static void SafeAdd<T>(this IList<T> list, T item)
        {
            Contract.Requires(list != null);

            lock (list)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// 以线程安全方式向动态数组中批量添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        public static void SafeAddRange<T>(this List<T> list, IEnumerable<T> items)
        {
            Contract.Requires(list != null);

            if (items == null)
                return;

            lock (list)
            {
                list.AddRange(items);
            }
        }

        /// <summary>
        /// 以线程安全的方式向哈希表中添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hs"></param>
        /// <param name="item"></param>
        public static void SafeAdd<T>(this HashSet<T> hs, T item)
        {
            Contract.Requires(hs != null);

            lock (hs)
            {
                hs.Add(item);
            }
        }

        /// <summary>
        /// 以线程安全的方式批量添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hs"></param>
        /// <param name="items"></param>
        public static void SafeAddRange<T>(this HashSet<T> hs, IEnumerable<T> items)
        {
            Contract.Requires(hs != null && items != null);

            lock (hs)
            {
                hs.AddRange(items);
            }
        }

        /// <summary>
        /// 返回除去指定元素的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, params T[] items)
        {
            Contract.Requires(source != null);

            return Enumerable.Except(source, items);
        }

        /// <summary>
        /// 返回除去指定元素的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer, params T[] items)
        {
            Contract.Requires(source != null && comparer != null);

            return Enumerable.Except(source, items, comparer);
        }

        /// <summary>
        /// 剔重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="comparier"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> keySelector, IEqualityComparer<TKey> equalityComparer = null)
        {
            Contract.Requires(collection != null && keySelector != null);

            Dictionary<TKey, T> dict = new Dictionary<TKey, T>(equalityComparer);
            foreach (T item in collection)
            {
                dict[keySelector(item)] = item;
            }

            return dict.Values;
        }

        /// <summary>
        /// 剔重并返回一个数组
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static T[] DistinctToArray<T>(this IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            if (collection == null)
                return Array<T>.Empty;

            HashSet<T> hs = new HashSet<T>(comparer);
            List<T> list = new List<T>();

            foreach (T item in collection)
            {
                if (hs.Add(item))
                    list.Add(item);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 剔重并返回一个数组
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="collection">数组</param>
        /// <returns></returns>
        public static T[] DistinctToArray<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                return Array<T>.Empty;

            HashSet<T> hs = new HashSet<T>();
            List<T> list = new List<T>();

            foreach (T item in collection)
            {
                if (hs.Add(item))
                    list.Add(item);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 删除符合条件的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="condition"></param>
        public static int RemoveWhere<T>(this IList<T> list, Func<T, bool> condition)
        {
            Contract.Requires(list != null && condition != null);

            int count = 0;
            for (int k = 0; k < list.Count; )
            {
                if (condition(list[k]))
                {
                    count++;
                    list.RemoveAt(k);
                }
                else
                {
                    k++;
                }
            }

            return count;
        }

        /// <summary>
        /// 删除符合条件的元素
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int RemoveWhere<TKey, TValue>(this IDictionary<TKey, TValue> dict, Func<KeyValuePair<TKey, TValue>, bool> condition)
        {
            Contract.Requires(dict != null && condition != null);

            List<TKey> keys = new List<TKey>();
            foreach (KeyValuePair<TKey, TValue> item in dict)
            {
                if (condition(item))
                    keys.Add(item.Key);
            }

            for (int k = 0; k < keys.Count; k++)
            {
                dict.Remove(keys[k]);
            }

            return keys.Count;
        }

        /// <summary>
        /// 删除符合条件的元素
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> RemoveWhereWithReturn<TKey, TValue>(this IDictionary<TKey, TValue> dict, Func<KeyValuePair<TKey, TValue>, bool> condition)
        {
            Contract.Requires(dict != null && condition != null);

            Dictionary<TKey, TValue> removedItems = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> item in dict)
            {
                if (condition(item))
                    removedItems.Add(item.Key, item.Value);
            }

            foreach (TKey key in removedItems.Keys)
            {
                dict.Remove(key);
            }

            return removedItems;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hs"></param>
        /// <param name="objs"></param>
        public static int RemoveRange<T>(this HashSet<T> hs, IEnumerable<T> objs)
        {
            Contract.Requires(hs != null && objs != null);

            int count = 0;
            foreach (T key in objs)
            {
                if (hs.Remove(key))
                    count++;
            }

            return count;
        }

        /// <summary>
        /// 删除指定的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hs"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] RemoveWhereWithReturn<T>(this HashSet<T> hs, Func<T, bool> condition)
        {
            Contract.Requires(hs != null && condition != null);

            List<T> items = new List<T>();
            foreach (T item in hs)
            {
                if (condition(item))
                {
                    items.Add(item);
                }
            }

            T[] arr = items.ToArray();
            for (int k = 0; k < arr.Length; k++)
            {
                hs.Remove(arr[k]);
            }

            return arr;
        }

        /// <summary>
        /// 从集合中随机选取指定数量的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IList<T> PickRandom<T>(this IList<T> source, int max)
        {
            Contract.Requires(source != null && max >= 0);

            if (max == 0)
                return new T[0];

            int k = 0, count = source.Count;
            if (count <= max)
                return source;

            T[] items = new T[max];
            foreach (T item in source)
            {
                if (k < items.Length)
                    items[k] = item;
                else
                {
                    int n = _r.Next(count);
                    if (n < max)
                        items[n] = item;
                }

                k++;
            }

            return items;
        }

        private static Random _r = new Random();

        /// <summary>
        /// 从集合中随机选取一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T PickOneRandom<T>(this IList<T> source)
        {
            Contract.Requires(source != null);

            return PickRandom(source, 1).First();
        }

        /// <summary>
        /// 从集合中随机选取一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T PickOneRandomOrDefault<T>(this IList<T> source)
        {
            Contract.Requires(source != null);

            return PickRandom(source, 1).FirstOrDefault();
        }

        /// <summary>
        /// 随机排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderRandom<T>(this IEnumerable<T> source)
        {
            Contract.Requires(source != null);

            T[] arr = source.ToArray();
            for (int k = 0, length = arr.Length; k < length; k++)
            {
                var k2 = _r.Next(length);
                T item = arr[k];
                arr[k] = arr[k2];
                arr[k2] = item;
            }

            return source;
        }

        /// <summary>
        /// 比较两个哈希表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="old">源</param>
        /// <param name="new">目标</param>
        /// <param name="onAdd">添加回调</param>
        /// <param name="onRemove">删除回调</param>
        /// <param name="callback">改变回调，参数分别为键、源值、目标值</param>
        public static void CompareTo<TKey, TValue>(this IDictionary<TKey, TValue> old, IDictionary<TKey, TValue> @new,
            Action<TKey, TValue, TValue, CollectionChangedType> callback)
        {
            Contract.Requires(old != null && @new != null && callback != null);

            foreach (KeyValuePair<TKey, TValue> item in @new)
            {
                TValue vSource;
                if (!old.TryGetValue(item.Key, out vSource))
                {
                    callback(item.Key, default(TValue), item.Value, CollectionChangedType.Add);
                }
                else if (!object.Equals(item.Value, vSource))
                {
                    callback(item.Key, vSource, item.Value, CollectionChangedType.Update);
                }
                else
                {
                    callback(item.Key, vSource, item.Value, CollectionChangedType.NoChange);
                }
            }

            foreach (KeyValuePair<TKey, TValue> item in old)
            {
                TValue vTarget;
                if (!@new.TryGetValue(item.Key, out vTarget))
                {
                    callback(item.Key, item.Value, default(TValue), CollectionChangedType.Remove);
                }
            }
        }

        /// <summary>
        /// 比较两个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="old"></param>
        /// <param name="new"></param>
        /// <param name="callback"></param>
        public static void CompareTo<T>(this IEnumerable<T> old, IEnumerable<T> @new, Action<T, CollectionChangedType> callback)
        {
            HashSet<T> oldHs = old is HashSet<T> ? (HashSet<T>)old : old.ToHashSet();
            HashSet<T> newHs = @new is HashSet<T> ? (HashSet<T>)@new : @new.ToHashSet();

            foreach (T item in newHs)
            {
                if (!oldHs.Contains(item))
                    callback(item, CollectionChangedType.Add);
                else
                    callback(item, CollectionChangedType.NoChange);
            }

            foreach (T item in oldHs)
            {
                if (!newHs.Contains(item))
                    callback(item, CollectionChangedType.Remove);
            }
        }

        /// <summary>
        /// 修改一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="add"></param>
        /// <param name="remove"></param>
        /// <param name="comparer"></param>
        public static IEnumerable<T> Adjust<T>(this IEnumerable<T> collection, IEnumerable<T> add, IEnumerable<T> remove = null, IEqualityComparer<T> comparer = null, Action<T, CollectionChangedType> callback = null)
        {
            Contract.Requires(collection != null);

            if (add == null && remove == null)
                return collection;

            return _Adjust(collection, add, remove, comparer, callback);
        }

        private static IEnumerable<T> _Adjust<T>(this IEnumerable<T> collection, IEnumerable<T> add, IEnumerable<T> remove, IEqualityComparer<T> comparer, Action<T, CollectionChangedType> callback)
        {
            HashSet<T> addHashSet = (add == null) ? null : add.ToHashSet(comparer);
            HashSet<T> removeHashSet = (remove == null) ? null : remove.ToHashSet(comparer);

            if (callback == null)
            {
                foreach (T item in collection)
                {
                    if (addHashSet != null && addHashSet.Contains(item) || removeHashSet != null && removeHashSet.Contains(item))
                        continue;

                    yield return item;
                }

                if (addHashSet != null)
                {
                    foreach (T item in addHashSet)
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                HashSet<T> dupHashSet = new HashSet<T>();
                foreach (T item in collection)
                {
                    if (removeHashSet != null && removeHashSet.Contains(item))
                    {
                        callback(item, CollectionChangedType.Remove);
                        continue;
                    }

                    if (addHashSet != null && addHashSet.Contains(item))
                    {
                        dupHashSet.Add(item);
                        continue;
                    }

                    yield return item;
                }

                if (addHashSet != null)
                {
                    foreach (T item in addHashSet)
                    {
                        if (!dupHashSet.Contains(item))
                            callback(item, CollectionChangedType.Add);

                        yield return item;
                    }
                }
            }
        }


        /// <summary>
        /// 生成一个数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static T[] GenerateArray<T>(int count, Func<int, T> creator = null)
        {
            Contract.Requires(count >= 0);

            T[] items = new T[count];
            for (int k = 0; k < count; k++)
            {
                items[k] = (creator == null) ? default(T) : creator(k);
            }

            return items;
        }

        /// <summary>
        /// 转换为只读的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> collection)
        {
            Contract.Requires(collection != null);

            return new ReadOnlyCollection<T>(collection.ToArray());
        }

        /// <summary>
        /// 转换为只读的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            Contract.Requires(list != null);

            return new ReadOnlyCollection<T>(list);
        }

        /// <summary>
        /// 将集合分成指定个数的组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> collection, int count)
        {
            Contract.Requires(collection != null && count > 0);

            IList<T> list = collection as IList<T>;
            if (list != null)
                return Split<T>(list, count);

            return _Split(collection, count);
        }

        private static IEnumerable<IEnumerable<T>> _Split<T>(IEnumerable<T> collection, int count)
        {
            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return _Split<T>(enumerator, count - 1);
                }
            }
        }

        private static IEnumerable<T> _Split<T>(IEnumerator<T> enumerator, int count)
        {
            do
            {
                yield return enumerator.Current;
                count--;

            } while (count > 0 && enumerator.MoveNext());
        }

        /// <summary>
        /// 将集合分成指定个数的组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IList<T> list, int count)
        {
            Contract.Requires(list != null && count > 0);

            int index = 0;
            while (index < list.Count)
            {
                yield return _Split(list, index, count);
                index += count;
            }
        }

        private static IEnumerable<T> _Split<T>(IList<T> list, int index, int count)
        {
            for (int k = index, c = Math.Min(count + index, list.Count); k < c; k++)
            {
                yield return list[k];
            }
        }

        /// <summary>
        /// 提取出符合条件的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int[] PickIndex<T>(this IList<T> list, Func<T, bool> condition)
        {
            Contract.Requires(list != null && condition != null);

            List<int> indexes = new List<int>();
            for (int k = 0, length = list.Count; k < length; k++)
            {
                if (condition(list[k]))
                    indexes.Add(k);
            }

            return indexes.ToArray();
        }

        /// <summary>
        /// 提取出指定索引的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="indexes"></param>
        /// <returns></returns>
        public static IEnumerable<T> PickValueByIndexes<T>(this IList<T> list, IEnumerable<int> indexes)
        {
            Contract.Requires(list != null && indexes != null);

            foreach (int index in indexes)
            {
                yield return list[index];
            }
        }

        /// <summary>
        /// 删除指定的键
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            Contract.Requires(dict != null);

            TValue value;
            dict.TryRemove(key, out value);
            return value;
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] Add<T>(this T[] array, IList<T> list)
        {
            if (array == null)
                array = Array<T>.Empty;

            if (list.IsNullOrEmpty())
                return array;

            T[] newArr = new T[array.Length + list.Count];
            Array.Copy(array, newArr, array.Length);

            int pos = array.Length;
            for (int k = 0, length = list.Count; k < length; k++)
            {
                newArr[pos++] = list[k];
            }

            return newArr;
        }

        /// <summary>
        /// 向数组中添加一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T[] Add<T>(this T[] array, T item)
        {
            if (array == null)
                array = Array<T>.Empty;

            T[] newArr = new T[array.Length + 1];
            Array.Copy(array, newArr, array.Length);
            newArr[array.Length] = item;
            return newArr;
        }

        /// <summary>
        /// 从数组中删除一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T[] Remove<T>(this T[] array, T item)
        {
            int index = Array.IndexOf<T>(array, item);
            if (index < 0)
                return array;

            T[] newArr = new T[array.Length - 1];
            Array.Copy(array, newArr, index);
            Array.Copy(array, index + 1, newArr, index, array.Length - index - 1);

            return newArr;
        }

        /// <summary>
        /// 是否有重复的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static bool IsRepeated<T>(this IEnumerable<T> collection, IEqualityComparer<T> equalityComparer = null)
        {
            Contract.Requires(collection != null);

            HashSet<T> hs = new HashSet<T>(equalityComparer);
            foreach (T item in collection)
            {
                if (!hs.Add(item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 寻找重复的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="func"></param>
        public static void FindRepeated<T>(this IEnumerable<T> collection, Func<T, bool> func,
            IEqualityComparer<T> equalityComparer = null)
        {
            Contract.Requires(collection != null && func != null);

            HashSet<T> hs = new HashSet<T>(equalityComparer);
            foreach (T item in collection)
            {
                if (!hs.Add(item) && !func(item))
                    break;
            }
        }

        /// <summary>
        /// 寻找重复的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="collection"></param>
        /// <param name="keySelector"></param>
        /// <param name="func"></param>
        public static void FindRepeated<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> keySelector, Func<T, bool> func,
            IEqualityComparer<TKey> equalityComparer = null)
        {
            Contract.Requires(collection != null && func != null);

            HashSet<TKey> hs = new HashSet<TKey>(equalityComparer);
            foreach (T item in collection)
            {
                if (!hs.Add(keySelector(item)) && !func(item))
                    break;
            }
        }

        /// <summary>
        /// 判断是否有
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="condition">判断条件</param>
        /// <param name="expItem">预期的项</param>
        /// <returns></returns>
        public static bool Any<T>(this IEnumerable<T> collection, Func<T, bool> condition, out T expItem)
        {
            Contract.Requires(collection != null && condition != null);

            foreach (T item in collection)
            {
                if (condition(item))
                {
                    expItem = item;
                    return false;
                }
            }

            expItem = default(T);
            return true;
        }

        /// <summary>
        /// 判断是否所有的项都满足条件
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="condition">判断条件</param>
        /// <param name="unexpItem">非预期的项</param>
        /// <returns></returns>
        public static bool All<T>(this IEnumerable<T> collection, Func<T, bool> condition, out T unexpItem)
        {
            Contract.Requires(collection != null && condition != null);

            foreach (T item in collection)
            {
                if (!condition(item))
                {
                    unexpItem = item;
                    return false;
                }
            }

            unexpItem = default(T);
            return true;
        }

        /// <summary>
        /// 判断字符串是否在集合中存在
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool Contains(this IEnumerable<string> strs, string s, bool ignoreCase = false)
        {
            Contract.Requires(strs != null);
            if (!ignoreCase)
                return strs.Contains(s);
            else
                return strs.Contains(s, IgnoreCaseEqualityComparer.Instance);
        }
    }
}

