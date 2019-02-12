using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;
using System.Runtime.Serialization;
using Common.Package.Serializer;
using Common.Utility;
using ServiceFairy.Entities.Cache;
using Common.Contracts.Service;
using Common;

namespace ServiceFairy.SystemInvoke
{
	partial class SystemInvoker
	{
        private CacheInvoker _cache;

        /// <summary>
        /// Cache Service
        /// </summary>
        public CacheInvoker Cache
        {
            get { return _cache ?? (_cache = new CacheInvoker(this)); }
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public class CacheInvoker : Invoker
        {
            public CacheInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <param name="keys">缓存键</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<CacheKeyValuePair[]> GetRangeSr(string[] keys, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(keys != null);

                var sr = CacheService.GetRange(Sc, new Cache_GetRange_Request() { Keys = keys, Remove = remove, EnableRoute = enableRoute }, settings);
                return CreateSr(sr, r => r.Datas);
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <param name="keys">缓存键</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public CacheKeyValuePair[] GetRange(string[] keys, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetRangeSr(keys, remove, enableRoute));
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="keys">缓存键</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<CacheKeyValuePair<T>[]> GetRangeSr<T>(string[] keys, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(keys != null);
                var sr = GetRangeSr(keys, remove, enableRoute, settings);
                return CreateSr(sr, result => result.ToArray(r => CacheKeyValuePair<T>.FromCacheKeyValuePair(r)));
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <param name="keys">缓存键</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public CacheKeyValuePair<T>[] GetRange<T>(string[] keys, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetRangeSr<T>(keys, remove, enableRoute, settings));
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<byte[]> GetSr(string key, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);

                var sr = CacheService.Get(Sc, new Cache_Get_Request { Key = key, Remove = remove, EnableRoute = enableRoute }, settings);
                return CreateSr(sr, r => r.Data);
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public byte[] Get(string key, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetSr(key, remove, enableRoute, settings));
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="remove">是否同时删除缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<T> GetSr<T>(string key, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                var sr = GetSr(key, remove, enableRoute, settings);
                return CreateSr(sr, r => CacheKeyValuePair<T>.Deserialize(r));
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="remove">是否同时删除缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public T Get<T>(string key, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetSr<T>(key, remove, enableRoute, settings));
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="value">缓存值</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<bool> TryGetSr<T>(string key, out T value, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                var sr = GetSr<T>(key, remove, enableRoute, settings);
                bool r;
                if (sr != null && sr.Result != null)
                {
                    r = true;
                    value = sr.Result;
                }
                else
                {
                    r = false;
                    value = default(T);
                }

                return CreateSr(sr, r);
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="value">缓存值</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public bool TryGet<T>(string key, out T value, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(TryGetSr<T>(key, out value, remove, enableRoute, settings));
            }

            /// <summary>
            /// 获取缓存，如果不存在则返回默认值
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="defaultValue">默认值</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<T> GetOrDefaultSr<T>(string key, T defaultValue = default(T), bool remove = false, bool enableRoute = true, CallingSettings settings = null)
                where T : class
            {
                Contract.Requires(key != null);
                var sr = GetSr<T>(key, remove, enableRoute, settings);

                return CreateSr(sr, r => r ?? defaultValue, defaultValue);
            }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="defaultValue">默认值</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public T GetOrDefault<T>(string key, T defaultValue = default(T), bool remove = false, bool enableRoute = true, CallingSettings settings = null)
                where T : class
            {
                return InvokeWithCheck(GetOrDefaultSr<T>(key, defaultValue, remove, enableRoute, settings));
            }

            /// <summary>
            /// 获取或设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">键</param>
            /// <param name="loader">加载器</param>
            /// <param name="expire">过期时间</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<T> GetOrSetSr<T>(string key, Func<string, T> loader, TimeSpan expire, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null && loader != null);

                T value;
                var sr = TryGetSr<T>(key, out value, settings: settings);
                if (!sr.Succeed)
                    return CreateSr(sr, default(T));

                value = loader(key);
                if (value == null)
                    return CreateSr(sr, default(T));

                var sr0 = SetSr<T>(key, value, expire, enableRoute, settings);
                return CreateSr(sr0, value);
            }

            /// <summary>
            /// 获取或设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="loader">加载器</param>
            /// <param name="expire">过期时间</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public T GetOrSet<T>(string key, Func<string, T> loader, TimeSpan expire, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetOrSetSr<T>(key, loader, expire, enableRoute, settings));
            }

            /// <summary>
            /// 批量获取缓存，通常指定带有通配符的键
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="keyPath">缓存键</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<T[]> GetRangeSr<T>(string keyPath, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(keyPath != null);

                var sr = GetRangeSr<T>(new[] { keyPath }, remove, enableRoute, settings);
                return CreateSr(sr, r => r.ToArray(v => v.Value));
            }

            /// <summary>
            /// 批量获取缓存，通常指定带有通配符的键
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="keyPath">缓存键</param>
            /// <param name="remove">是否同时删除该缓存</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public T[] GetRange<T>(string keyPath, bool remove = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetRangeSr<T>(keyPath, remove, enableRoute, settings));
            }

            /// <summary>
            /// 批量设置缓存
            /// </summary>
            /// <param name="items"></param>
            /// <param name="enableRoute"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult SetRangeSr(CacheItem[] items, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(items != null);
                return CacheService.SetRange(Sc, new Cache_SetRange_Request() { Items = items, EnableRoute = enableRoute }, settings);
            }

            /// <summary>
            /// 批量设置缓存
            /// </summary>
            /// <param name="items">缓存项</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public void SetRange(CacheItem[] items, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(SetRangeSr(items, enableRoute, settings));
            }

            /// <summary>
            /// 批量设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="items">缓存项</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetRangeSr<T>(CacheItem<T>[] items, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(items != null);
                return SetRangeSr(items.ToArray(it => it.ToCacheItem()), enableRoute, settings);
            }

            /// <summary>
            /// 批量设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="items">缓存项</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public void SetRange<T>(CacheItem<T>[] items, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(SetRangeSr<T>(items, enableRoute, settings));
            }

            /// <summary>
            /// 设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="item">缓存项</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetSr<T>(CacheItem<T> item, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(item != null);

                CacheItem it = item.ToCacheItem();
                Cache_Set_Request req = new Cache_Set_Request { Key = it.Key, Data = it.Data, Expired = it.Expired, EnableRoute = enableRoute };
                return CacheService.Set(Sc, req, settings);
            }

            /// <summary>
            /// 设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="item">缓存项</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public void Set<T>(CacheItem<T> item, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(SetSr<T>(item, enableRoute, settings));
            }

            /// <summary>
            /// 设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="value">缓存值</param>
            /// <param name="expire">过期时间</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetSr<T>(string key, T value, TimeSpan expire, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);
                return SetSr<T>(new CacheItem<T>(key, value, expire), enableRoute, settings);
            }

            /// <summary>
            /// 设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="value">缓存值</param>
            /// <param name="expire">过期时间</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public void Set<T>(string key, T value, TimeSpan expire, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(SetSr(key, value, expire, enableRoute, settings));
            }

            /// <summary>
            /// 设置一项缓存
            /// </summary>
            /// <param name="expire">过期时间</param>
            /// <param name="value">缓存值</param>
            /// <param name="key">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult SetSr(string key, object value, TimeSpan expire, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);
                return SetSr(key, SerializerUtility.SerializeToBytes(DataFormat.Binary, value), expire, enableRoute, settings);
            }

            /// <summary>
            /// 设置一项缓存
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="value">缓存值</param>
            /// <param name="expire">过期时间</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public void Set(string key, object value, TimeSpan expire, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(SetSr(key, value, expire, enableRoute, settings));
            }

            /// <summary>
            /// 批量设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="dict">缓存集合</param>
            /// <param name="expired">过期时间</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetRangeSr<T>(IDictionary<string, T> dict, TimeSpan expired, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(dict != null);
                return SetRangeSr<T>(dict.Select(d => new CacheItem<T>(d.Key, d.Value, expired)).ToArray(), enableRoute, settings);
            }

            /// <summary>
            /// 批量设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="dict">缓存集合</param>
            /// <param name="expired">过期时间</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public void SetRange<T>(IDictionary<string, T> dict, TimeSpan expired, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(SetRangeSr<T>(dict, expired, enableRoute, settings));
            }

            /// <summary>
            /// 批量设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="routePath">缓存路径</param>
            /// <param name="dict">缓存集合</param>
            /// <param name="expired">过期时间</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetRangeSr<T>(string routePath, IDictionary<string, T> dict, TimeSpan expired, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(routePath != null && dict != null);
                return SetRangeSr<T>(dict.Select(d => new CacheItem<T>(routePath + ":" + d.Key, d.Value, expired)).ToArray(), enableRoute, settings);
            }

            /// <summary>
            /// 批量设置缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="routePath">缓存路径</param>
            /// <param name="dict">缓存集合</param>
            /// <param name="expired">过期时间</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void SetRange<T>(string routePath, IDictionary<string, T> dict, TimeSpan expired, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(SetRangeSr<T>(routePath, dict, expired, enableRoute, settings));
            }

            /// <summary>
            /// 批量删除缓存
            /// </summary>
            /// <param name="keys">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult RemoveSr(string[] keys, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(keys != null);

                return CacheService.Remove(Sc, new Cache_Remove_Request() { Keys = keys, EnableRoute = enableRoute }, settings);
            }

            /// <summary>
            /// 批量删除缓存
            /// </summary>
            /// <param name="keys">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public void Remove(string[] keys, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(RemoveSr(keys, enableRoute, settings));
            }

            /// <summary>
            /// 删除一项缓存
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RemoveSr(string key, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);

                return RemoveSr(new[] { key }, enableRoute, settings);
            }

            /// <summary>
            /// 删除一项缓存
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public void Remove(string key, bool enableRoute = true, CallingSettings settings = null)
            {
                Validate(RemoveSr(key, enableRoute, settings));
            }

            /// <summary>
            /// 批量获取键
            /// </summary>
            /// <param name="keys">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<string[]> GetKeysSr(string[] keys, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(keys != null);
                var sr = CacheService.GetKeys(Sc, new Cache_GetKeys_Request() { Keys = keys, EnableRoute = enableRoute }, settings);
                return CreateSr(sr, r => r.Keys);
            }

            /// <summary>
            /// 批量获取键
            /// </summary>
            /// <param name="keys">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public string[] GetKeys(string[] keys, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetKeysSr(keys, enableRoute, settings));
            }

            /// <summary>
            /// 获取键
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<string[]> GetKeySr(string key, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);

                return GetKeysSr(new[] { key }, enableRoute, settings);
            }

            /// <summary>
            /// 获取键
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public string[] GetKey(string key, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);

                return GetKeys(new[] { key }, enableRoute, settings);
            }

            /// <summary>
            /// 是否存在指定的键
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<bool> ExistsKeySr(string key, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);

                var sr = GetKeySr(key, enableRoute, settings);
                return CreateSr(sr, sr == null || sr.Result == null ? false : sr.Result.Length > 0);
            }

            /// <summary>
            /// 是否存在指定的键
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public bool ExistsKey(string key, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);

                return InvokeWithCheck(ExistsKeySr(key, enableRoute, settings));
            }

            /// <summary>
            /// 将缓存赋予一个增量
            /// </summary>
            /// <param name="items">缓存项</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<CacheIncreaseResult[]> IncreaseSr(CacheIncreaseItem[] items, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(items != null);

                var sr = CacheService.Increase(Sc, new Cache_Increase_Request() {
                    Items = items, EnableRoute = enableRoute
                }, settings);

                return CreateSr(sr, r => r.Results);
            }

            /// <summary>
            /// 将缓存赋予一个增量
            /// </summary>
            /// <param name="items">缓存项</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public CacheIncreaseResult[] Increase(CacheIncreaseItem[] items, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(items != null);

                return InvokeWithCheck(IncreaseSr(items, enableRoute, settings));
            }

            /// <summary>
            /// 将缓存赋予一个增量
            /// </summary>
            /// <param name="item">缓存项</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<CacheIncreaseResult[]> IncreaseSr(CacheIncreaseItem item, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(item != null);
                return IncreaseSr(new[] { item }, enableRoute, settings);
            }

            /// <summary>
            /// 将缓存赋予一个增量
            /// </summary>
            /// <param name="item">缓存项</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public CacheIncreaseResult[] Increase(CacheIncreaseItem item, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(IncreaseSr(item, enableRoute, settings));
            }

            /// <summary>
            /// 将缓存赋予一个整型增量
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="increament">增量</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <param name="checked">是否检查溢出</param>
            /// <returns></returns>
            public ServiceResult<int> IncreaseSr(string key, int increament, bool @checked = false, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);
                var sr = IncreaseSr(new CacheIncreaseItem() { Key = key, Checked = @checked, Increament = increament }, enableRoute, settings);
                return CreateSr(sr, r => (r.Length == 0) ? default(int) : _ConvertToInt32(r[0].NewValue, @checked));
            }

            private static int _ConvertToInt32(decimal value, bool @checked)
            {
                if (!@checked)
                    return (int)value;

                try
                {
                    return checked((int)value);
                }
                catch (OverflowException)
                {
                    throw new ServiceException(ServerErrorCode.DataError, "数据溢出");
                }
            }

            /// <summary>
            /// 将缓存赋予一个整型增量
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="increament">增量</param>
            /// <param name="checcked">是否检查溢出</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int Increase(string key, int increament, bool @checked = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(IncreaseSr(key, increament, @checked, enableRoute, settings));
            }

            /// <summary>
            /// 将缓存赋予一个长整型常量
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="increament">增量</param>
            /// <param name="checked">是否检查溢出</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<long> IncreaseSr(string key, long increament, bool @checked = false, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);

                var sr = IncreaseSr(new CacheIncreaseItem() { Key = key, Checked = @checked, Increament = increament }, enableRoute, settings);
                return CreateSr(sr, r => (r.Length == 0) ? default(long) : _ConvertToInt64(r[0].NewValue, @checked));
            }

            private static long _ConvertToInt64(decimal value, bool @checked)
            {
                if (!@checked)
                    return (long)value;

                try
                {
                    return checked((long)value);
                }
                catch (OverflowException)
                {
                    throw new ServiceException(ServerErrorCode.DataError, "数据溢出");
                }
            }

            /// <summary>
            /// 将缓存赋予一个长整型增量
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="increament"><增量/param>
            /// <param name="checked">是否检查溢出</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public long Increase(string key, long increament, bool @checked = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(IncreaseSr(key, increament, @checked, enableRoute, settings));
            }

            /// <summary>
            /// 将缓存赋予一个双精度浮点增量
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="increament">增量</param>
            /// <param name="checked">是否检查溢出</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<double> IncreaseSr(string key, double increament, bool @checked = false, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);

                var sr = IncreaseSr(new CacheIncreaseItem() { Key = key, Checked = @checked, Increament = (decimal)increament }, enableRoute, settings);
                return CreateSr(sr, r => (r.Length == 0) ? default(double) : _ConvertToDouble(r[0].NewValue, @checked));
            }

            private static double _ConvertToDouble(decimal value, bool @checked)
            {
                if (!@checked)
                    return (double)value;

                try
                {
                    return checked((double)value);
                }
                catch (OverflowException)
                {
                    throw new ServiceException(ServerErrorCode.DataError, "数据溢出");
                }
            }

            /// <summary>
            /// 将缓存赋予一个双精度浮点增量
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="increament">增量</param>
            /// <param name="checked">是否检查溢出</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public double Increase(string key, double increament, bool @checked = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(IncreaseSr(key, increament, @checked, enableRoute, settings));
            }

            /// <summary>
            /// 将缓存赋予一个双精度浮点增量
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="increament">增量</param>
            /// <param name="checked">是否检查溢出</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<decimal> IncreaseSr(string key, decimal increament, bool @checked = false, bool enableRoute = true, CallingSettings settings = null)
            {
                Contract.Requires(key != null);

                var sr = IncreaseSr(new CacheIncreaseItem() { Key = key, Checked = @checked, Increament = increament }, enableRoute, settings);
                return CreateSr(sr, r => (r.Length == 0) ? default(decimal) : r[0].NewValue);
            }

            /// <summary>
            /// 将缓存赋予一个双精度浮点增量
            /// </summary>
            /// <param name="key">缓存键</param>
            /// <param name="increament">增量</param>
            /// <param name="checked">是否检查溢出</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public decimal Increase(string key, decimal increament, bool @checked = false, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(IncreaseSr(key, increament, @checked, enableRoute, settings));
            }
        }
    }

    /// <summary>
    /// 泛型的缓存项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract]
    public class CacheItem<T>
    {
        public CacheItem(string key, T value, TimeSpan expired)
        {
            Contract.Requires(key != null);

            Key = key;
            Value = value;
            Expired = expired;
        }

        /// <summary>
        /// 键
        /// </summary>
        [DataMember]
        public string Key { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public T Value { get; private set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [DataMember]
        public TimeSpan Expired { get; private set; }

        /// <summary>
        /// 转换为字节流形式的缓存项
        /// </summary>
        /// <returns></returns>
        public CacheItem ToCacheItem()
        {
            return new CacheItem() {
                Key = Key,
                Data = SerializerUtility.OptimalBinarySerialize(Value),
                Expired = Expired
            };
        }

        /// <summary>
        /// 从字节流形式的缓存项转换
        /// </summary>
        /// <param name="cacheItem"></param>
        /// <returns></returns>
        public static CacheItem<T> FromCacheItem(CacheItem cacheItem)
        {
            if (cacheItem == null)
                return null;

            return new CacheItem<T>(cacheItem.Key, SerializerUtility.OptimalBinaryDeserialize<T>(cacheItem.Data), cacheItem.Expired);
        }
    }

    /// <summary>
    /// 泛型的缓存键值对
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract]
    public class CacheKeyValuePair<T>
    {
        public CacheKeyValuePair(string key, T value)
        {
            Contract.Requires(key != null);

            Key = key;
            Value = value;
        }

        /// <summary>
        /// 键
        /// </summary>
        [DataMember]
        public string Key { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public T Value { get; private set; }

        /// <summary>
        /// 转换为字节流形式的键值对
        /// </summary>
        /// <returns></returns>
        public CacheKeyValuePair ToCacheKeyValuePair()
        {
            return new CacheKeyValuePair {
                Key = Key,
                Data = SerializerUtility.OptimalBinarySerialize(Value)
            };
        }

        /// <summary>
        /// 将字节流形式的键值对转换为泛型
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public static CacheKeyValuePair<T> FromCacheKeyValuePair(CacheKeyValuePair pair)
        {
            if (pair == null)
                return null;

            return new CacheKeyValuePair<T>(pair.Key, Deserialize(pair.Data));
        }

        public static T Deserialize(byte[] data)
        {
            return SerializerUtility.OptimalBinaryDeserialize<T>(data);
        }
    }
}
