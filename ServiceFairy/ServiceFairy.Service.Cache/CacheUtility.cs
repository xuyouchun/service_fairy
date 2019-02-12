using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.Cache
{
    /// <summary>
    /// 缓存的工具类
    /// </summary>
    static class CacheUtility
    {
        /// <summary>
        /// 获取指定键的哈希码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetRouteHashCode(string key)
        {
            Contract.Requires(key != null);

            CacheKeyParser p = new CacheKeyParser(key);
            return p.RouteHashCode;
        }
    }
}
