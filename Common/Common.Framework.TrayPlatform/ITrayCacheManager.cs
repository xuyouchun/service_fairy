using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using System.Diagnostics.Contracts;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 共享缓存管理器
    /// </summary>
    public interface ITrayCacheManager
    {
        /// <summary>
        /// 创建共享缓存
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="name">名称</param>
        /// <param name="autoCreate">是否自动创建</param>
        /// <param name="global">是否为全局缓存，可以被其它的服务使用</param>
        /// <returns></returns>
        ITrayCache<TKey, TValue> Get<TKey, TValue>(string name = null, bool autoCreate = true, bool global = false) where TValue : class;
    }

    /// <summary>
    /// 共享缓存
    /// </summary>
    public interface ITrayCache<TKey, TValue> : IDisposable where TValue : class
    {
        /// <summary>
        /// 获取指定键的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue Get(TKey key);

        /// <summary>
        /// 批量获取缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        KeyValuePair<TKey, TValue>[] GetRange(IEnumerable<TKey> keys);

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        void Add(TKey key, TValue value, TimeSpan timeout);

        /// <summary>
        /// 批量写入缓存
        /// </summary>
        /// <param name="items"></param>
        /// <param name="timeout"></param>
        void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items, TimeSpan timeout);

        /// <summary>
        /// 删除指定键的缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(TKey key);

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys"></param>
        void RemoveRange(IEnumerable<TKey> keys);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void Clear();

        /// <summary>
        /// 获取所有的键
        /// </summary>
        /// <returns></returns>
        TKey[] GetAllKeys();

        /// <summary>
        /// 缓存总数
        /// </summary>
        int Count { get; }
    }

    /// <summary>
    /// 平台缓存的节点
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class TrayCacheChainNode<TKey, TValue> : CacheChainNodeBase<TKey, TValue> where TValue : class
    {
        public TrayCacheChainNode(ITrayCache<TKey, TValue> cache, TimeSpan expire)
        {
            Contract.Requires(cache != null);

            _cache = cache;
            _expire = expire;
        }

        private readonly ITrayCache<TKey, TValue> _cache;
        private readonly TimeSpan _expire;

        protected override KeyValuePair<TKey, TValue>[] OnGet(TKey[] keys)
        {
            return _cache.GetRange(keys);
        }

        protected override void OnAdd(KeyValuePair<TKey, TValue>[] items)
        {
            _cache.AddRange(items, _expire);
        }

        protected override void OnRemove(TKey[] keys)
        {
            _cache.RemoveRange(keys);
        }
    }
}
