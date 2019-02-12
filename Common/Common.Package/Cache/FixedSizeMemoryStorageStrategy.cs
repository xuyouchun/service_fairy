using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Package.Cache
{
    /// <summary>
    /// 固定大小的内存缓存策略
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    public class FixedSizeMemoryStorageStrategy<TKey, TValue> : MemoryStorageStrategy<TKey, TValue>
        where TValue : class, IComparable<TValue>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxSize">最大大小</param>
        /// <param name="removeRate">超出最大大小之后,需要删除元素的比率</param>
        public FixedSizeMemoryStorageStrategy(int maxSize, float removeRate = 0.3f)
        {
            Contract.Requires(maxSize > 0);
            Contract.Requires(removeRate > 0 && removeRate <= 1);

            _maxSize = maxSize;
            _removeRate = removeRate;
        }

        private readonly int _maxSize;
        private readonly float _removeRate;

        protected override void OnAddCompleted()
        {
            IDictionary<TKey, CacheItem<TKey, TValue>> dict = GetDictionary();
            if (dict.Count <= _maxSize)
                return;

            var items = dict.OrderBy(v => v.Value.Value).Take((int)(_maxSize * _removeRate)).Select(v => v.Key).ToArray();
            List<CacheItem<TKey, TValue>> expiredItems = new List<CacheItem<TKey, TValue>>();
            for (int k = 0; k < items.Length; k++)
            {
                TKey key = items[k];
                expiredItems.Add(dict[key]);
                dict.Remove(key);
            }

            if (expiredItems.Count > 0)
                _RaiseFullEvent(expiredItems.ToArray());
        }

        public event EventHandler<FixedSizeMemoryStorageStrategyFullEventArgs<TKey, TValue>> Full;

        private void _RaiseFullEvent(CacheItem<TKey, TValue>[] values)
        {
            var eh = Full;
            if (eh != null)
                eh(this, new FixedSizeMemoryStorageStrategyFullEventArgs<TKey, TValue>(values));
        }
    }

    /// <summary>
    /// 固定缓存策略的过期事件
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class FixedSizeMemoryStorageStrategyFullEventArgs<TKey, TValue> : EventArgs
        where TValue : class
    {
        internal FixedSizeMemoryStorageStrategyFullEventArgs(CacheItem<TKey, TValue>[] items)
        {
            Items = items;
        }

        public CacheItem<TKey, TValue>[] Items { get; private set; }
    }
}
