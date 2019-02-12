using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Package.Cache.CacheExpireDependencies
{
    /// <summary>
    /// 相对时间过期方式
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class RelativeCacheExpireDependency : ICacheExpireDependency
    {
        public RelativeCacheExpireDependency(TimeSpan interval)
        {
            _Interval = interval;
            _ExpireTime = QuickTime.Now + _Interval;
        }

        private readonly TimeSpan _Interval;
        private DateTime _ExpireTime;

        #region ICacheExpireDependency Members

        public void Reset()
        {
            _ExpireTime = QuickTime.Now + _Interval;
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
