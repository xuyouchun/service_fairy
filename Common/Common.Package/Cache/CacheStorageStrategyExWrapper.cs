using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Package.Cache
{
    /// <summary>
    /// 缓存存储策略的包装器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    class CacheStorageStrategyExWrapper<TKey, TValue> : IStorageStrategyEx<TKey, TValue>
        where TValue : class
    {
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="strategy"></param>
        public CacheStorageStrategyExWrapper(IStorageStrategy<TKey, TValue> strategy)
        {
            _Strategy = strategy;
        }

        private readonly IStorageStrategy<TKey, TValue> _Strategy;

        #region IStorageStrategyEx<TKey,TValue> Members

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="cacheItems"></param>
        public void AddRange(IEnumerable<CacheItem<TKey, TValue>> cacheItems)
        {
            Contract.Requires(cacheItems != null);

            foreach (CacheItem<TKey, TValue> item in cacheItems)
            {
                _Strategy.Add(item);
            }
        }

        /// <summary>
        /// 批量获取
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public CacheItem<TKey, TValue>[] GetRange(IEnumerable<TKey> keys)
        {
            Contract.Requires(keys != null);

            List<CacheItem<TKey, TValue>> result = new List<CacheItem<TKey, TValue>>();
            foreach (TKey key in keys)
            {
                var item = Get(key);
                if (item != null)
                    result.Add(item);;
            }

            return result.ToArray();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="reason"></param>
        public void RemoveRange(IEnumerable<TKey> keys, CacheStorageStrategyRemoveReason reason)
        {
            foreach (TKey key in keys)
            {
                _Strategy.Remove(key, reason);
            }
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TKey> GetAllKeys()
        {
            throw new NotSupportedException("不支持获取所有的缓存键");
        }

        #endregion

        #region IStorageStrategy<TKey,TValue> Members

        public void Add(CacheItem<TKey, TValue> cacheItem)
        {
            _Strategy.Add(cacheItem);
        }

        public CacheItem<TKey, TValue> Get(TKey key)
        {
            return _Strategy.Get(key);
        }

        public void Remove(TKey key, CacheStorageStrategyRemoveReason reason)
        {
            _Strategy.Remove(key, reason);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _Strategy.Dispose();
        }

        #endregion
    }
}
