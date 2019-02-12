using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Cache;
using System.Diagnostics.Contracts;
using Common.Package.Cache.CacheExpireDependencies;

namespace Common.Package
{
    [System.Diagnostics.DebuggerStepThrough]
    public class SingleMemoryCache<TValue> : IDisposable
        where TValue : class
    {
        public SingleMemoryCache(TimeSpan timeout, Func<TValue> loader)
            : this(new RelativeCacheExpireDependency(timeout), loader)
        {
            
        }

        public SingleMemoryCache(ICacheExpireDependency expireDependency, Func<TValue> loader)
        {
            Contract.Requires(expireDependency != null && loader != null);

            _expireDependency = expireDependency;
            _cacheValueLoader = CacheHelper.ConvertToValueLoader<Guid, TValue>(key => loader());
        }

        public TValue Get()
        {
            return _innerCache.GetOrAdd(_key, _expireDependency, _cacheValueLoader);
        }

        private readonly Guid _key = Guid.NewGuid();
        private readonly ICacheExpireDependency _expireDependency;
        private readonly ICacheValueLoader<Guid, TValue> _cacheValueLoader;

        private static readonly Cache<Guid, TValue> _innerCache = new Cache<Guid, TValue>();

        /// <summary>
        /// 删除缓存
        /// </summary>
        public void Remove()
        {
            _innerCache.Remove(_key);
        }

        public void Dispose()
        {
            Remove();
        }
    }
}
