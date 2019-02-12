﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Package.Cache.CacheExpireDependencies;
using Common.Package.Cache;
using Common.Contracts.Service;

namespace Common.Package
{
    /// <summary>
    /// 缓存
    /// </summary>
    /// <typeparam name="TKey">缓存键</typeparam>
    /// <typeparam name="TValue">缓存值</typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.DebuggerTypeProxy(typeof(Cache<,>.DebuggerProxy))]
	public class Cache<TKey, TValue> : MarshalByRefObjectEx, IDisposable
        where TValue : class
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storageStrategy"></param>
        public Cache(IStorageStrategy<TKey, TValue> storageStrategy = null)
        {
            _SupportGetAllKeys = storageStrategy is IStorageStrategyEx<TKey, TValue>;

            if (storageStrategy == null)
                _StorageStrategy = new MemoryStorageStrategy<TKey, TValue>();
            else
                _StorageStrategy = _SupportGetAllKeys ?
                    (IStorageStrategyEx<TKey, TValue>)storageStrategy : new CacheStorageStrategyExWrapper<TKey, TValue>(storageStrategy);
        }

        private readonly IStorageStrategyEx<TKey, TValue> _StorageStrategy;
        private readonly bool _SupportGetAllKeys;

        /// <summary>
        /// 添加一个缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireDependency">缓存过期依赖方式</param>
        /// <param name="valueLoader">缓存值加载器</param>
        public void Add(TKey key, TValue value = null,
            ICacheExpireDependency expireDependency = null, ICacheValueLoader<TKey, TValue> valueLoader = null)
        {
            Contract.Requires(key != null);

            var cacheItem = new CacheItem<TKey, TValue>(key, value,
                expireDependency ?? _EmptyCacheExpireDependency, valueLoader);
            _StorageStrategy.Add(cacheItem);
        }

        /// <summary>
        /// 添加一个缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireDependency">缓存过期依赖方式</param>
        /// <param name="valueLoader">缓存值加载器</param>
        public void Add(TKey key, ICacheExpireDependency expireDependency, ICacheValueLoader<TKey, TValue> valueLoader = null)
        {
            Contract.Requires(key != null);

            Add(key, null, expireDependency, valueLoader);
        }

        private static readonly NoExpireCacheExpireDependency _EmptyCacheExpireDependency = new NoExpireCacheExpireDependency();

        /// <summary>
        /// 获取一个缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            Contract.Requires(key != null);

            CacheItem<TKey, TValue> cacheItem = _StorageStrategy.Get(key);
            if (cacheItem == null)
                return null;

            return _GetCacheItemValue(cacheItem);
        }

        private TValue _GetCacheItemValue(CacheItem<TKey, TValue> cacheItem)
        {
            TValue value = cacheItem.Value;
            if (value != null && !cacheItem.ExpireDependency.HasExpired())
            {
                cacheItem.ExpireDependency.AccessNotify();
                return value;
            }

            if (cacheItem.ValueLoader == null) // 不需要重新加载
            {
                Remove(cacheItem.Key);
                return null;
            }
            else
            {
                lock (cacheItem.ValueLoader.GetSyncLocker(cacheItem.Key))
                {
                    if (cacheItem.Value == null || cacheItem.ExpireDependency.HasExpired())
                    {
                        value = cacheItem.ValueLoader.Load(cacheItem.Key);
                        cacheItem.Value = value;
                        cacheItem.ExpireDependency.Reset();
                    }

                    return cacheItem.Value;
                }
            }
        }

        /// <summary>
        /// 批量获取缓存项
        /// </summary>
        /// <param name="keys">键值集合</param>
        /// <returns>缓存项的键值对</returns>
        public IDictionary<TKey, TValue> GetRange(IEnumerable<TKey> keys)
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            foreach (CacheItem<TKey, TValue> item in _StorageStrategy.GetRange(keys))
            {
                TValue value = _GetCacheItemValue(item);
                if (value != null)
                    result[item.Key] = value;
            }

            return result;
        }

        /// <summary>
        /// 获取所有的键
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TKey> GetAllKeys()
        {
            return _StorageStrategy.GetAllKeys();
        }

        /// <summary>
        /// 是否支持获取所有的键
        /// </summary>
        public bool SupportGetAllKeys
        {
            get { return _SupportGetAllKeys; }
        }

        /// <summary>
        /// 删除一个缓存项
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key)
        {
            Contract.Requires(key != null);

            _StorageStrategy.Remove(key, CacheStorageStrategyRemoveReason.UserRemove);
        }

        /// <summary>
        /// 批量删除缓存项
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveRange(IEnumerable<TKey> keys)
        {
            Contract.Requires(keys != null);

            _StorageStrategy.RemoveRange(keys, CacheStorageStrategyRemoveReason.UserRemove);
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _StorageStrategy.Dispose();
        }

        ~Cache()
        {
            Dispose();
        }

        #endregion

        #region Class DebuggerProxy ...

        class DebuggerProxy
        {
            public DebuggerProxy(Cache<TKey, TValue> owner)
            {
                _owner = owner;
                Items = _owner.GetRange(_owner.GetAllKeys());
            }

            private readonly Cache<TKey, TValue> _owner;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public IDictionary<TKey, TValue> Items;
        }

        #endregion

    }
}
