using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Package;
using Common.Utility;

namespace ServiceFairy.Service.Cache.Components
{
    /// <summary>
    /// 基于文件存储的缓存值
    /// </summary>
    class FileValueLoader : IValueLoader
    {
        public FileValueLoader(byte[] buffer)
        {
            string file = _GetPath(_id);
            PathUtility.CreateDirectoryForFile(file);
            File.WriteAllBytes(file, buffer);
        }

        private readonly Guid _id = Guid.NewGuid();

        public byte[] Load()
        {
            string file = _GetPath(_id);
            if (!File.Exists(file))
                return null;

            try
            {
                return File.ReadAllBytes(file);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return null;
            }
        }

        private static string _GetPath(Guid id)
        {
            string s = id.ToString();
            return Path.Combine(Settings.CacheBasePath, s.Substring(0, 2), s.Substring(2, 2), s.Substring(4));
        }

        public void Disuse()
        {

        }

        public void Dispose()
        {
            string file = _GetPath(_id);

            try
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }
    }
}
