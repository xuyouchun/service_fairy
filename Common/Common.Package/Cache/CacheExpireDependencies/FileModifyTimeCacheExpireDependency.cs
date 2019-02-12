using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Utility;

namespace Common.Package.Cache.CacheExpireDependencies
{
    /// <summary>
    /// 基于文件修改时间的过期方式
    /// </summary>
    public class FileModifyTimeCacheExpireDependency : ICacheExpireDependency
    {
        public FileModifyTimeCacheExpireDependency(string file)
        {
            Contract.Requires(file != null);
            _file = file;

            _lastModifyTime = _ReadLastModifyTime();
        }

        private readonly string _file;
        private DateTime _lastModifyTime;

        private DateTime _ReadLastModifyTime()
        {
            try
            {
                return File.GetLastWriteTime(_file);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return default(DateTime);
            }
        }

        public void Reset()
        {
            _lastModifyTime = _ReadLastModifyTime();
        }

        public void AccessNotify()
        {
            
        }

        public bool HasExpired()
        {
            DateTime dt = _ReadLastModifyTime();
            if (dt != _lastModifyTime)
            {
                _lastModifyTime = dt;
                return true;
            }

            return false;
        }

        public static ICacheExpireDependency FromMutiFiles(string[] files)
        {
            if (files.IsNullOrEmpty())
                return NoExpireCacheExpireDependency.Instance;

            if (files.Length == 1)
                return new FileModifyTimeCacheExpireDependency(files[0]);

            return CacheExpireDependency.Proxy(() => CacheExpireDependency.Or(files.ToArray(file => new FileModifyTimeCacheExpireDependency(file))));
        }
    }
}
