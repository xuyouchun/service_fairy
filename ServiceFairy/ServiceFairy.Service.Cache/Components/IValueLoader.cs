using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;

namespace ServiceFairy.Service.Cache.Components
{
    /// <summary>
    /// 缓存值加载方式
    /// </summary>
    interface IValueLoader : IDisposable
    {
        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        byte[] Load();

        /// <summary>
        /// 闲置
        /// </summary>
        void Disuse();
    }

    /// <summary>
    /// 创建工厂
    /// </summary>
    class ValueLoader : IValueLoader
    {
        public ValueLoader(byte[] buffer)
        {
            _valueLoader = new MemoryValueLoader(buffer);
        }

        private volatile IValueLoader _valueLoader;

        public byte[] Load()
        {
            lock (this)
            {
                byte[] buffer = _valueLoader.Load();

                FileValueLoader loader = _valueLoader as FileValueLoader;
                if (loader != null)
                {
                    _valueLoader = new MemoryValueLoader(buffer);
                    loader.Dispose();
                }

                return buffer;
            }
        }

        public void Disuse()
        {
            lock (this)
            {
                MemoryValueLoader loader = _valueLoader as MemoryValueLoader;
                if (loader != null)
                {
                    try
                    {
                        byte[] buffer = _valueLoader.Load();
                        if (buffer == null || buffer.Length < 1024) // 如果数据为空或太小，则不存储在文件中
                            return;

                        _valueLoader = new FileValueLoader(buffer);
                        loader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                    }
                }
            }
        }

        public void Dispose()
        {
            lock (this)
            {
                _valueLoader.Dispose();
            }
        }
    }
}
