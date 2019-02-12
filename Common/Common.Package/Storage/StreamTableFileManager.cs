using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Utility;
using Common.Package.Cache;
using Common.Package.Cache.CacheExpireDependencies;

namespace Common.Package.Storage
{
    /// <summary>
    /// 具有缓存读取功能的StreamTable管理器
    /// </summary>
    public class StreamTableFileManager : IDisposable
    {
        public StreamTableFileManager()
        {

        }

        private readonly Cache<string, StreamTableReader> _cache = new Cache<string, StreamTableReader>();

        /// <summary>
        /// 加载StreamTable
        /// </summary>
        /// <param name="file"></param>
        /// <param name="expireDependency"></param>
        /// <returns></returns>
        public StreamTableReader Load(string file, ICacheExpireDependency expireDependency = null)
        {
            Contract.Requires(file != null);

            expireDependency = expireDependency ?? new FileModifyTimeCacheExpireDependency(file);
            file = PathUtility.Revise(Path.Combine(Directory.GetCurrentDirectory(), file));
            return _cache.GetOrAdd(file.ToLower(), expireDependency, (key) => _LoadStreamTableReader(file));
        }

        /// <summary>
        /// 加载StreamTable
        /// </summary>
        /// <param name="file"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        public StreamTableReader Load(string file, TimeSpan expired)
        {
            return Load(file, new RelativeCacheExpireDependency(expired));
        }

        /// <summary>
        /// 加载StreamTable
        /// </summary>
        /// <param name="file"></param>
        /// <param name="init"></param>
        /// <param name="step"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public StreamTableReader Load(string file, TimeSpan init, TimeSpan step, TimeSpan max)
        {
            return Load(file, new DynamicCacheExpireDependency(init, step, max));
        }

        private StreamTableReader _LoadStreamTableReader(string file)
        {
            return new StreamTableReader(file);
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}
