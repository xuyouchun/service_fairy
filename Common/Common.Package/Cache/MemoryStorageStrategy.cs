using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics.Contracts;
using Common.Package.GlobalTimer;
using Common.Collection;

namespace Common.Package.Cache
{
    /// <summary>
    /// 以内存作存储的缓存项存储策略
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    public class MemoryStorageStrategy<TKey, TValue> : IStorageStrategyEx<TKey, TValue>, IDisposable
        where TValue : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxCount">初始容量</param>
        public MemoryStorageStrategy()
        {
            _dict = new LargeDictionary<TKey, CacheItem<TKey, TValue>>();
            _taskHandle = Global.GlobalTimer.Add(TimeSpan.FromSeconds(5), new TaskFuncAdapter(_CheckExpired), false, true);
        }

        #region IStorageStrategy<TKey,TValue> Members

        private readonly LargeDictionary<TKey, CacheItem<TKey, TValue>> _dict;
        private readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

        /// <summary>
        /// 获取内部的哈希表
        /// </summary>
        /// <returns></returns>
        protected IDictionary<TKey, CacheItem<TKey, TValue>> GetDictionary()
        {
            return _dict;
        }

        /// <summary>
        /// 添加一个缓存项
        /// </summary>
        /// <param name="cacheItem">缓存项</param>
        public void Add(CacheItem<TKey, TValue> cacheItem)
        {
            Contract.Requires(cacheItem != null);

            _locker.EnterWriteLock();
            try
            {
                _dict[cacheItem.Key] = cacheItem;
                OnAddCompleted();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        protected virtual void OnAddCompleted()
        {

        }

        /// <summary>
        /// 批量添加缓存项
        /// </summary>
        /// <param name="cacheItems">缓存项</param>
        public void AddRange(IEnumerable<CacheItem<TKey, TValue>> cacheItems)
        {
            Contract.Requires(cacheItems != null);

            _locker.EnterWriteLock();
            try
            {
                foreach (var item in cacheItems)
                {
                    _dict[item.Key] = item;
                }

                OnAddCompleted();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// 获取一个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public CacheItem<TKey, TValue> Get(TKey key)
        {
            Contract.Requires(key != null);

            _locker.EnterReadLock();
            try
            {
                CacheItem<TKey, TValue> cacheItem;
                _dict.TryGetValue(key, out cacheItem);
                return cacheItem;
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public CacheItem<TKey, TValue>[] GetRange(IEnumerable<TKey> keys)
        {
            Contract.Requires(keys != null);

            List<CacheItem<TKey, TValue>> result = new List<CacheItem<TKey, TValue>>();

            _locker.EnterReadLock();
            try
            {
                foreach (TKey key in keys)
                {
                    CacheItem<TKey, TValue> cacheItem;
                    if (_dict.TryGetValue(key, out cacheItem))
                        result.Add(cacheItem);
                }
            }
            finally
            {
                _locker.ExitReadLock();
            }

            return result.ToArray();
        }

        /// <summary>
        /// 获取全部缓存项
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TKey> GetAllKeys()
        {
            _locker.EnterReadLock();
            try
            {
                return _dict.Keys.ToArray();
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        /// <summary>
        /// 删除一个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="reason">删除原因</param>
        public void Remove(TKey key, CacheStorageStrategyRemoveReason reason)
        {
            Contract.Requires(key != null);

            _locker.EnterWriteLock();
            try
            {
                CacheItem<TKey, TValue> cacheItem;
                if (_dict.TryGetValue(key, out cacheItem))
                {
                    _dict.Remove(key, true);
                    if (reason == CacheStorageStrategyRemoveReason.Expired)
                        _RaiseExpiredEvent(new[] { cacheItem });
                    OnRemoveCompleted();
                }
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// 删除完毕
        /// </summary>
        protected virtual void OnRemoveCompleted()
        {

        }

        /// <summary>
        /// 清空所有
        /// </summary>
        /// <param name="reason"></param>
        public void Clear(CacheStorageStrategyRemoveReason reason)
        {
            _locker.EnterWriteLock();
            try
            {
                if (reason != CacheStorageStrategyRemoveReason.Expired || Expired == null)
                {
                    _dict.Clear(true);
                }
                else
                {
                    CacheItem<TKey, TValue>[] items = _dict.Values.ToArray();
                    _dict.Clear(true);
                    _RaiseExpiredEvent(items.ToArray());
                }

                OnRemoveCompleted();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// 批量删除缓存项
        /// </summary>
        /// <param name="keys">缓存键</param>
        /// <param name="reason">删除原因</param>
        public void RemoveRange(IEnumerable<TKey> keys, CacheStorageStrategyRemoveReason reason)
        {
            Contract.Requires(keys != null);

            _locker.EnterWriteLock();
            try
            {
                if (reason != CacheStorageStrategyRemoveReason.Expired || Expired == null)
                {
                    foreach (TKey key in keys)
                    {
                        _dict.Remove(key);
                    }
                }
                else
                {
                    List<CacheItem<TKey, TValue>> items = new List<CacheItem<TKey, TValue>>();
                    foreach (TKey key in keys)
                    {
                        CacheItem<TKey, TValue> item;
                        if (_dict.TryGetValue(key, out item))
                        {
                            _dict.Remove(key);
                            items.Add(item);
                        }
                    }

                    _RaiseExpiredEvent(items.ToArray());
                }

                _dict.TrimExcess();
                OnRemoveCompleted();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _taskHandle.Dispose();
        }

        ~MemoryStorageStrategy()
        {
            Dispose();
        }

        #endregion

        #region ICacheExpireCheckSupported Members

        private readonly IGlobalTimerTaskHandle _taskHandle;

        /// <summary>
        /// 检查缓存过期
        /// </summary>
        private void _CheckExpired()
        {
            List<TKey> expiredKeys = new List<TKey>();

            _locker.EnterReadLock();
            try
            {
                foreach (CacheItem<TKey, TValue> item in _dict.Values)
                {
                    if (item.ExpireDependency.HasExpired())
                        expiredKeys.Add(item.Key);
                }
            }
            finally
            {
                _locker.ExitReadLock();
            }

            if (expiredKeys.Count > 0)
                RemoveRange(expiredKeys, CacheStorageStrategyRemoveReason.Expired);
        }

        #endregion

        /// <summary>
        /// 过期事件
        /// </summary>
        public event EventHandler<MemoryStorageExpiredEventArgs<TKey, TValue>> Expired;

        private void _RaiseExpiredEvent(CacheItem<TKey, TValue>[] items)
        {
            var eh = Expired;
            if (eh != null)
                eh(this, new MemoryStorageExpiredEventArgs<TKey, TValue>(items));
        }
    }

    /// <summary>
    /// 内存缓存策略的过期事件
    /// </summary>
    public class MemoryStorageExpiredEventArgs<TKey, TValue> : EventArgs
        where TValue : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items"></param>
        internal MemoryStorageExpiredEventArgs(CacheItem<TKey, TValue>[] items)
        {
            Items = items;
        }

        /// <summary>
        /// 过期的项
        /// </summary>
        public CacheItem<TKey, TValue>[] Items { get; private set; }
    }
}
