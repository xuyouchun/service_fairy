using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Package.Storage
{
    /// <summary>
    /// 缓存区
    /// </summary>
    abstract class StreamTableBuffer
    {
        /// <summary>
        /// 当前读取位置
        /// </summary>
        public abstract int Position { get; set; }

        public abstract int Read(byte[] buffer, int offset, int count);

        public abstract byte ReadByte();

        public abstract int ReadInt32();

        public abstract DateTime ReadDateTime();

        public abstract string ReadString();

        public abstract object ReadData(StreamTableColumnType columnType);

        public abstract int ReadIndex(IndexType indexType);

        public abstract StreamTableBuffer CreateFromPosition(int position);
    }
}
