using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Package.Cache.CacheExpireDependencies
{
    /// <summary>
    /// 永不过期的缓存过期方式
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
	public class NoExpireCacheExpireDependency : ICacheExpireDependency
	{
        #region ICacheExpireDependency Members

        public void Reset()
        {
            
        }

        public void AccessNotify()
        {
            
        }

        public bool HasExpired()
        {
            return false;
        }

        #endregion

        public static readonly NoExpireCacheExpireDependency Instance = new NoExpireCacheExpireDependency();
    }
}
