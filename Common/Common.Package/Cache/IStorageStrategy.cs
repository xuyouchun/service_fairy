using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Package.Cache
{
    /// <summary>
    /// 缓存的存储策略
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    public interface IStorageStrategy<TKey, TValue> : IDisposable
        where TValue : class
    {
        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="cacheItem"></param>
        void Add(CacheItem<TKey, TValue> cacheItem);

        /// <summary>
        /// 获取一个缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>缓存项</returns>
        CacheItem<TKey, TValue> Get(TKey key);

        /// <summary>
        /// 删除缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="reason">删除原因</param>
        void Remove(TKey key, CacheStorageStrategyRemoveReason reason);
    }

    /// <summary>
    /// 缓存策略中，删除原因
    /// </summary>
    public enum CacheStorageStrategyRemoveReason
    {
        /// <summary>
        /// 过期删除
        /// </summary>
        Expired,
        
        /// <summary>
        /// 用户删除
        /// </summary>
        UserRemove,
    }
}
