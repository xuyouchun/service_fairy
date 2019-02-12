using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using Common.Package;
using ServiceFairy.Entities.Cache;
using Common;

namespace ServiceFairy.Service.Cache.Components
{
    /// <summary>
    /// 支持路由功能的缓存操作
    /// </summary>
    [AppComponent("支持路由功能的缓存操作")]
    class RoutableCacheManager : AppComponent
    {
        public RoutableCacheManager(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 批量获取缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="remove"></param>
        /// <param name="enableRoute"></param>
        /// <returns></returns>
        public CacheKeyValuePair[] Get(string[] keys, bool remove = false, bool enableRoute = true)
        {
            Contract.Requires(keys != null);

            if (!enableRoute)
                return _LocalGet(keys, remove);

            return _service.ServiceConsistentNodeManager.Collect<string, CacheKeyValuePair>(keys,
                localLoader: (localKeys) => _LocalGet(localKeys, remove),
                remoteLoader: (remoteClientId, remoteKeys) => _RemoteGet(remoteClientId, keys, remove),
                hashcodeSelector: CacheUtility.GetRouteHashCode
            );
        }

        private CacheKeyValuePair[] _TryRemoteGet(Guid clientId, string[] keys, bool remove)
        {
            try
            {
                return _RemoteGet(clientId, keys, remove);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return new CacheKeyValuePair[0];
            }
        }

        private CacheKeyValuePair[] _RemoteGet(Guid clientId, string[] keys, bool remove)
        {
            IServiceClient sc = _service.Context.CreateServiceClient(clientId);
            SystemInvoker invoker = SystemInvoker.FromServiceClient(sc);
            return invoker.Cache.GetRange(keys, remove, false);
        }

        private CacheKeyValuePair[] _LocalGet(string[] keys, bool remove)
        {
            var list = from key in keys
                       where key != null
                       let es = _service.CacheManager.Get(key, remove)
                       where !es.IsNullOrEmpty()
                       select new { Es = es, KeyPath = _GetKeyPath(key) };

            IValueLoader vl;
            return list.SelectMany(item => item.Es.Select(e => new CacheKeyValuePair() {
                Key = item.KeyPath + e.Key, Data = (vl = e.ValueLoader) == null ? null : vl.Load()
            })).Where(v => v.Data != null).ToArray();
        }

        private static string _GetKeyPath(string key)
        {
            int k = key.IndexOf(':');
            if (k < 0)
                return "";

            return key.Substring(0, k + 1);
        }

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="items"></param>
        /// <param name="enableRoute"></param>
        public void Set(CacheItem[] items, bool enableRoute = true)
        {
            Contract.Requires(items != null);
            if (items.Length == 0)
                return;

            if (!enableRoute)
            {
                _LocalSet(items);
            }
            else
            {
                _service.ServiceConsistentNodeManager.Apply<CacheItem>(items,
                    localAction: _LocalSet,
                    remoteAction: _RemoteSet,
                    hashcodeSelector: (item) => CacheUtility.GetRouteHashCode(item.Key)
                );
            }
        }

        private void _TryRemoteSet(Guid clientId, CacheItem[] cis)
        {
            try
            {
                _RemoteSet(clientId, cis);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        private void _RemoteSet(Guid clientId, CacheItem[] cis)
        {
            IServiceClient sc = _service.Context.CreateServiceClient(clientId);
            SystemInvoker invoker = SystemInvoker.FromServiceClient(sc);
            invoker.Cache.SetRange(cis, false);
        }

        private void _LocalSet(CacheItem[] items)
        {
            _service.CacheManager.Set(items);
        }

        /// <summary>
        /// 删除指定名称的缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="enableRoute"></param>
        public void Remove(string[] keys, bool enableRoute = true)
        {
            Contract.Requires(keys != null);
            if (keys.Length == 0)
                return;

            if (!enableRoute)
            {
                _LocalRemove(keys);
            }
            else
            {
                _service.ServiceConsistentNodeManager.Apply<string>(keys,
                    localAction: _LocalRemove,
                    remoteAction: _RemoteRemove,
                    hashcodeSelector: CacheUtility.GetRouteHashCode
                );
            }
        }

        private void _TryRemoteRemove(Guid clientId, string[] keys)
        {
            try
            {
                _RemoteRemove(clientId, keys);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        // 删除key
        private void _RemoteRemove(Guid clientId, string[] keys)
        {
            IServiceClient sc = _service.Context.CreateServiceClient(clientId);
            SystemInvoker invoker = SystemInvoker.FromServiceClient(sc);
            invoker.Cache.Remove(keys, false);
        }

        // 在本地删除key
        private void _LocalRemove(string[] keys)
        {
            _service.CacheManager.Remove(keys);
        }

        /// <summary>
        /// 将缓存赋予一个增量
        /// </summary>
        /// <param name="items"></param>
        /// <param name="enableRoute"></param>
        /// <returns></returns>
        public CacheIncreaseResult[] Increase(CacheIncreaseItem[] items, bool enableRoute = true)
        {
            Contract.Requires(items != null);

            if (items.Length == 0)
                return Array<CacheIncreaseResult>.Empty;

            if (!enableRoute)
            {
                return _LocalIncrease(items);
            }
            else
            {
                return _service.ServiceConsistentNodeManager.Collect<CacheIncreaseItem, CacheIncreaseResult>(items,
                    localLoader: _LocalIncrease,
                    remoteLoader: _RemoteIncrease,
                    hashcodeSelector: item => CacheUtility.GetRouteHashCode(item.Key)
                );
            }
        }

        // 在本地执行缓存增量
        private CacheIncreaseResult[] _LocalIncrease(CacheIncreaseItem[] items)
        {
            return _service.CacheManager.Increase(items);
        }

        // 在远程执行缓存增量
        private CacheIncreaseResult[] _TryRemoteIncrease(Guid clientId, CacheIncreaseItem[] items)
        {
            try
            {
                return _RemoteIncrease(clientId, items);
            }
            catch(Exception ex)
            {
                LogManager.LogError(ex);
                return Array<CacheIncreaseResult>.Empty;
            }
        }

        // 在远程执行缓存增量
        private CacheIncreaseResult[] _RemoteIncrease(Guid clientId, CacheIncreaseItem[] items)
        {
            IServiceClient sc = _service.Context.CreateServiceClient(clientId);
            SystemInvoker invoker = SystemInvoker.FromServiceClient(sc);
            return invoker.Cache.Increase(items, false);
        }

        /// <summary>
        /// 获取缓存键
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="enableRoute"></param>
        /// <returns></returns>
        public string[] GetKeys(string[] keys, bool enableRoute = true)
        {
            Contract.Requires(keys != null);

            if (!enableRoute)
            {
                return _LocalGetKeys(keys);
            }
            else
            {
                return _service.ServiceConsistentNodeManager.Collect<string, string>(keys,
                    localLoader: _LocalGetKeys,
                    remoteLoader: _RemoteGetKeys,
                    hashcodeSelector: CacheUtility.GetRouteHashCode
                );
            }
        }

        // 远程获取键
        private string[] _TryRemoteGetKeys(Guid clientId, string[] keys)
        {
            try
            {
                return _RemoteGetKeys(clientId, keys);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return new string[0];
            }
        }

        // 远程获取键
        private string[] _RemoteGetKeys(Guid clientId, string[] keys)
        {
            IServiceClient sc = _service.Context.CreateServiceClient(clientId);
            SystemInvoker invoker = SystemInvoker.FromServiceClient(sc);
            return invoker.Cache.GetKeys(keys, false);
        }

        // 本地获取键
        private string[] _LocalGetKeys(string[] keys)
        {
            return _service.CacheManager.GetKeys(keys);
        }
    }
}