using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Package.Cache.CacheExpireDependencies
{
    /// <summary>
    /// 动态过期时间的缓存，依赖于请求的频率，自动延长过期时间
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class DynamicCacheExpireDependency : ICacheExpireDependency
    {
        public DynamicCacheExpireDependency(TimeSpan init, TimeSpan step, TimeSpan max)
            : this(() => { TimeSpan t = init; init += step; if (init > max) init = max; return t; })
        {

        }

        public DynamicCacheExpireDependency(Func<TimeSpan> nextFunc)
        {
            _NextFunc = nextFunc;
            _ExpireTime = QuickTime.Now + nextFunc();
        }

        private readonly Func<TimeSpan> _NextFunc;

        private DateTime _ExpireTime;

        #region ICacheExpireDependency Members

        public void Reset()
        {
            
        }

        public void AccessNotify()
        {
            _ExpireTime = QuickTime.Now + _NextFunc();
        }

        public bool HasExpired()
        {
            return QuickTime.Now >= _ExpireTime;
        }

        #endregion
    }
}
