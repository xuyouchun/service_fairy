using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using Common.Package;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 缓存管理器
    /// </summary>
    [AppComponent("缓存链管理器", "提供创建缓存链，查看缓存信息等功能", AppComponentCategory.System, "Sys_CacheChainManager")]
    public class CacheChainManagerAppComponent : AppComponent
    {
        public CacheChainManagerAppComponent(SystemAppServiceBase service)
            : base(service)
        {
            _service = service;
        }

        private readonly SystemAppServiceBase _service;

        /// <summary>
        /// 创建缓存链
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="enableWeakReference">是否允许弱引用缓存</param>
        /// <param name="memoryCacheExpire">内存缓存时间</param>
        /// <param name="trayCacheExpire">平台缓存时间</param>
        /// <param name="distributedCacheExpire">分布式缓存时间</param>
        /// <param name="distributedCachePrefix">分布式缓存键前缀</param>
        /// <param name="loader">加载器</param>
        /// <returns></returns>
        public CacheChain<TKey, TValue> CreateCacheChain<TKey, TValue>(
            bool enableWeakReference = false,
            TimeSpan memoryCacheExpire = default(TimeSpan),
            TimeSpan trayCacheExpire = default(TimeSpan),
            TimeSpan distributedCacheExpire = default(TimeSpan),
            string distributedCachePrefix = null,
            CacheChainValueLoader<TKey, TValue> loader = null
            ) where TValue : class
        {
            return CreateCacheChain<TKey, TValue>(new CacheChainCreationInfo<TKey, TValue> {
                EnableWeakReferenceCache = enableWeakReference,
                MemoryCacheExpire = memoryCacheExpire,
                TrayCacheExpire = trayCacheExpire,
                DistributedCacheExpire = distributedCacheExpire,
                DistributedCachePrefix = distributedCachePrefix,
                Loader = loader,
            });
        }

        /// <summary>
        /// 缓存链的创建器
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public CacheChain<TKey, TValue> CreateCacheChain<TKey, TValue>(CacheChainCreationInfo<TKey, TValue> info)
            where TValue : class
        {
            Contract.Requires(info != null);

            CacheChain<TKey, TValue> chain = new CacheChain<TKey, TValue>();

            if (info.EnableWeakReferenceCache)
                chain.AddWeakReferenceCache();

            if (info.MemoryCacheExpire > TimeSpan.Zero)
                chain.AddMemoryCache(info.MemoryCacheExpire);

            if (info.TrayCacheExpire > TimeSpan.Zero)
                chain.AddTrayCache(_service.Context, info.TrayCacheName, info.TrayCacheExpire, info.TrayCacheGlobal);

            if (info.DistributedCacheExpire > TimeSpan.Zero)
            {
                if (info.DistributedCacheKeyCreator != null)
                    chain.AddDistributedCache(_service.Invoker, info.DistributedCacheExpire, info.DistributedCacheKeyCreator);
                else
                    chain.AddDistributedCache(_service.Invoker, info.DistributedCacheExpire, info.DistributedCachePrefix);
            }

            if (info.Loader != null)
                chain.AddLoader(info.Loader);

            if(!info.OtherNodes.IsNullOrEmpty())
                chain.AddNodes(info.OtherNodes);

            return chain;
        }
    }

    /// <summary>
    /// 缓存链的创建参数
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheChainCreationInfo<TKey, TValue> where TValue : class
    {
        public CacheChainCreationInfo()
        {
            
        }

        /// <summary>
        /// 使用弱引用（默认false）
        /// </summary>
        public bool EnableWeakReferenceCache { get; set; }

        /// <summary>
        /// 内存缓存时间
        /// </summary>
        public TimeSpan MemoryCacheExpire { get; set; }

        /// <summary>
        /// 分布式缓存过期时间（默认5分钟）
        /// </summary>
        public TimeSpan DistributedCacheExpire { get; set; }

        /// <summary>
        /// 分布式缓存键前缀（默认为空）
        /// </summary>
        public string DistributedCachePrefix { get; set; }

        /// <summary>
        /// 分布式缓存键创建器
        /// </summary>
        public Func<TKey, string> DistributedCacheKeyCreator { get; set; }

        /// <summary>
        /// 平台缓存时间（默认5分钟）
        /// </summary>
        public TimeSpan TrayCacheExpire { get; set; }

        /// <summary>
        /// 平台缓存名称（默认为空，如果不是全局缓存，可不指定）
        /// </summary>
        public string TrayCacheName { get; set; }

        /// <summary>
        /// 平台缓存是否为全局缓存（服务共享，默认为false）
        /// </summary>
        public bool TrayCacheGlobal { get; set; }

        /// <summary>
        /// 加载器
        /// </summary>
        public CacheChainValueLoader<TKey, TValue> Loader { get; set; }

        /// <summary>
        /// 其它的缓存节点
        /// </summary>
        public ICacheChainNode<TKey, TValue>[] OtherNodes { get; set; }
    }
}
