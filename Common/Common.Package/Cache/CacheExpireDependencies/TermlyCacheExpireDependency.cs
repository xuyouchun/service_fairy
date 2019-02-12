using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Package.Cache.CacheExpireDependencies
{
    /// <summary>
    /// 固定时间的缓存过期方式
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class TermlyCacheExpireDependency : ICacheExpireDependency
    {
        public TermlyCacheExpireDependency(DateTime expireTime)
        {
            _ExpireTime = expireTime;
        }

        public TermlyCacheExpireDependency(TimeSpan interval)
            : this(QuickTime.Now + interval)
        {

        }

        private readonly DateTime _ExpireTime;

        #region ICacheExpireDependency Members

        public void Reset()
        {
            
        }

        public void AccessNotify()
        {
            
        }

        public bool HasExpired()
        {
            return QuickTime.Now > _ExpireTime;
        }

        #endregion
    }
}
