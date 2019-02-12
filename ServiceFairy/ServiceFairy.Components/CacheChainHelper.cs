using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using System.Diagnostics.Contracts;
using ServiceFairy.SystemInvoke;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 缓存链的帮助类
    /// </summary>
    public static class CacheChainHelper
    {
        /// <summary>
        /// 创建分布式缓存节点
        /// </summary>
        /// <param name="invoker">系统调用</param>
        /// <param name="expired">过期时间</param>
        /// <param name="prefix">缓存键前值</param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> CreateDistribuedCacheChainNode<TKey, TValue>(SystemInvoker invoker, TimeSpan expired, string prefix = null)
            where TValue : class
        {
            Contract.Requires(invoker != null);

            return new DistribuedCacheChainNode<TKey, TValue>(invoker, expired, prefix);
        }

        /// <summary>
        /// 创建分布式缓存节点
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="cacheChain"></param>
        /// <param name="invoker"></param>
        /// <param name="expired"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> AddDistributedCache<TKey, TValue>(this CacheChain<TKey, TValue> cacheChain,
            SystemInvoker invoker, TimeSpan expired, string prefix = null)
            where TValue : class
        {
            Contract.Requires(cacheChain != null);

            var node = CreateDistribuedCacheChainNode<TKey, TValue>(invoker, expired, prefix);
            cacheChain.AddNode(node);
            return node;
        }

        /// <summary>
        /// 创建分布式缓存节点
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="expired"></param>
        /// <param name="keyBuilder"></param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> CreateDistribuedCacheChainNode<TKey, TValue>(SystemInvoker invoker, TimeSpan expired, Func<TKey, string> keyBuilder)
            where TValue : class
        {
            Contract.Requires(invoker != null && keyBuilder != null);

            return new DistribuedCacheChainNode<TKey, TValue>(invoker, expired, keyBuilder);
        }

        /// <summary>
        /// 创建分布式缓存节点
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="cacheChain"></param>
        /// <param name="invoker"></param>
        /// <param name="expired"></param>
        /// <param name="keyBuilder"></param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> AddDistributedCache<TKey, TValue>(this CacheChain<TKey, TValue> cacheChain, SystemInvoker invoker, TimeSpan expired, Func<TKey, string> keyBuilder)
            where TValue : class
        {
            Contract.Requires(cacheChain != null);

            var node = CreateDistribuedCacheChainNode<TKey, TValue>(invoker, expired, keyBuilder);
            cacheChain.AddNode(node);
            return node;
        }

        /// <summary>
        /// 创建Tray平台缓存的节点
        /// </summary>
        /// <param name="trayContext"></param>
        /// <param name="cacheName"></param>
        /// <param name="expired"></param>
        /// <param name="global"></param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> CreateTrayCacheChainNode<TKey, TValue>(TrayContext trayContext, string cacheName, TimeSpan expired, bool global = false)
            where TValue : class
        {
            Contract.Requires(trayContext != null && cacheName != null);

            return CreateTrayCacheChainNode(trayContext.CacheManager.Get<TKey, TValue>(cacheName, true, global), expired);
        }

        /// <summary>
        /// 添加Tray平台缓存节点
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="trayContext"></param>
        /// <param name="cacheName"></param>
        /// <param name="expired"></param>
        /// <param name="global"></param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> AddTrayCache<TKey, TValue>(this CacheChain<TKey, TValue> cacheChain, TrayContext trayContext, string cacheName, TimeSpan expired, bool global = false)
            where TValue : class
        {
            Contract.Requires(cacheChain != null);

            var node = CreateTrayCacheChainNode<TKey, TValue>(trayContext, cacheName, expired, global);
            cacheChain.AddNode(node);
            return node;
        }

        /// <summary>
        /// 创建Tray平台缓存的节点
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="trayCache"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> CreateTrayCacheChainNode<TKey, TValue>(ITrayCache<TKey, TValue> trayCache, TimeSpan expired)
            where TValue : class
        {
            Contract.Requires(trayCache != null);

            return new TrayCacheChainNode<TKey, TValue>(trayCache, expired);
        }

        /// <summary>
        /// 添加Tray平台缓存的节点
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="cacheChain"></param>
        /// <param name="trayCache"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> AddTrayCache<TKey, TValue>(this CacheChain<TKey, TValue> cacheChain, ITrayCache<TKey, TValue> trayCache, TimeSpan expired)
            where TValue : class
        {
            Contract.Requires(cacheChain != null);

            var node = CreateTrayCacheChainNode<TKey, TValue>(trayCache, expired);
            cacheChain.AddNode(node);
            return node;
        }
    }
}
