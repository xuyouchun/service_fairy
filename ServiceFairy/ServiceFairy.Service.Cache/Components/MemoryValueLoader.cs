using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.Service.Cache.Components
{
    /// <summary>
    /// 内存值加载器
    /// </summary>
    class MemoryValueLoader : IValueLoader
    {
        public MemoryValueLoader(byte[] buffer)
        {
            _buffer = buffer;
        }

        private readonly byte[] _buffer;

        public byte[] Load()
        {
            return _buffer;
        }

        public void Disuse()
        {

        }

        public void Dispose()
        {

        }
    }
}
