using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Package.Cache;

namespace ServiceFairy.Service.Cache.Components
{
    /// <summary>
    /// 缓存项
    /// </summary>
    class CacheEntity
    {
        public CacheEntity(string key, IValueLoader valueLoader, DateTime expiredTime)
        {
            Key = key;
            ValueLoader = valueLoader;
            ExpiredTime = expiredTime;
            LastAccessTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 字节流
        /// </summary>
        public IValueLoader ValueLoader { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessTime { get; set; }
    }
}
