using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using CacheManagerKey = System.Tuple<string, System.Type>;
using System.Collections.Concurrent;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common.Package;

namespace Common.Framework.TrayPlatform
{
    partial class TrayAppServiceManager
    {
        private TrayCacheManager _globalTrayCacheManager = new TrayCacheManager();

        /// <summary>
        /// 缓存管理器
        /// </summary>
        public class TrayCacheManager : MarshalByRefObjectEx, ITrayCacheManager, IDisposable
        {
            public TrayCacheManager(ITrayCacheManager global = null)
            {
                _global = global;
            }

            private readonly ITrayCacheManager _global;
            private readonly ConcurrentDictionary<CacheManagerKey, object> _trayCacheDict = new ConcurrentDictionary<CacheManagerKey, object>();

            public ITrayCache<TKey, TValue> Get<TKey, TValue>(string name = null, bool autoCreate = true, bool global = false) where TValue : class
            {
                if (global)
                    return _global.Get<TKey, TValue>(name, autoCreate, false);

                CacheManagerKey key = new CacheManagerKey(name ?? Guid.NewGuid().ToString(), typeof(ITrayCache<TKey, TValue>));
                if (!autoCreate)
                {
                    object value;
                    _trayCacheDict.TryGetValue(key, out value);
                    return value as ITrayCache<TKey, TValue>;
                }

                return _trayCacheDict.GetOrAdd(key, (key0) => new TrayCache<TKey, TValue>(this, key0)) as ITrayCache<TKey, TValue>;
            }

            #region Class TrayCache ...

            interface ITrayCache
            {
                void CheckExpired();
            }

            class TrayCache<TKey, TValue> : MarshalByRefObjectEx, ITrayCache<TKey, TValue>, ITrayCache, IDisposable where TValue : class
            {
                public TrayCache(TrayCacheManager cacheManager, CacheManagerKey cacheManagerKey)
                {
                    _cacheManager = cacheManager;
                    _cacheManagerKey = cacheManagerKey;

                    _Register(this);
                }

                private readonly TrayCacheManager _cacheManager;
                private readonly CacheManagerKey _cacheManagerKey;
                private readonly ConcurrentDictionary<TKey, CacheItem<TKey, TValue>> _cacheDict
                    = new ConcurrentDictionary<TKey, CacheItem<TKey, TValue>>();

                public TValue Get(TKey key)
                {
                    CacheItem<TKey, TValue> item;
                    if (!_cacheDict.TryGetValue(key, out item) || item.ExpiredTime < QuickTime.UtcNow)
                        return null;

                    return item.Value;
                }

                public KeyValuePair<TKey, TValue>[] GetRange(IEnumerable<TKey> keys)
                {
                    Contract.Requires(keys != null);

                    var list = from key in keys
                               where key != null
                               let value = Get(key)
                               where value != null
                               select new KeyValuePair<TKey, TValue>(key, value);

                    return list.ToArray();
                }

                public void Add(TKey key, TValue value, TimeSpan timeout)
                {
                    Contract.Requires(key != null);

                    _cacheDict[key] = new CacheItem<TKey, TValue>() { Key = key, Value = value, ExpiredTime = QuickTime.UtcNow + timeout };
                }

                public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items, TimeSpan timeout)
                {
                    Contract.Requires(items != null);

                    DateTime expiredTime = QuickTime.UtcNow + timeout;
                    foreach (KeyValuePair<TKey, TValue> item in items)
                    {
                        if (item.Key != null)
                        {
                            _cacheDict[item.Key] = new CacheItem<TKey, TValue>() { Key = item.Key, Value = item.Value, ExpiredTime = expiredTime };
                        }
                    }
                }

                public void Remove(TKey key)
                {
                    _cacheDict.Remove(key);
                }

                public void RemoveRange(IEnumerable<TKey> keys)
                {
                    _cacheDict.RemoveRange(keys);
                }

                public void Clear()
                {
                    _cacheDict.Clear();
                }

                public TKey[] GetAllKeys()
                {
                    return _cacheDict.Keys.ToArray();
                }

                public int Count
                {
                    get { return _cacheDict.Count; }
                }

                public void Dispose()
                {
                    _Unregister(this);
                    _cacheManager._trayCacheDict.Remove(_cacheManagerKey);
                }

                class CacheItem<TKey, TValue>
                {
                    public TKey Key;
                    public TValue Value;
                    public DateTime ExpiredTime;
                }

                #region Dispose ...

                static TrayCache()
                {
                    GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(10), _CheckExpired, false);
                }

                private static readonly HashSet<ITrayCache> _hs = new HashSet<ITrayCache>();

                private static void _Register(ITrayCache cache)
                {
                    lock (_hs)
                    {
                        _hs.Add(cache);
                    }
                }

                private static void _Unregister(ITrayCache cache)
                {
                    lock (_hs)
                    {
                        _hs.Remove(cache);
                    }
                }

                private static void _CheckExpired()
                {
                    ITrayCache[] caches;
                    lock (_hs)
                    {
                        caches = _hs.ToArray();
                    }

                    foreach (ITrayCache cache in caches)
                    {
                        cache.CheckExpired();
                    }
                }

                void ITrayCache.CheckExpired()
                {
                    DateTime now = DateTime.UtcNow;
                    _cacheDict.RemoveWhere(item => item.Value.ExpiredTime < now);
                }

                ~TrayCache()
                {
                    Dispose();
                }

                #endregion

            }

            #endregion

            public void Dispose()
            {
                _trayCacheDict.Values.ToArray().ForEach(cache => cache.Dispose());
            }
        }
    }
}
