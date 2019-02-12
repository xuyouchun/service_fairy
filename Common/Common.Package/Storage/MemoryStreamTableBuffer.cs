using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;

namespace Common.Package.Storage
{
    /// <summary>
    /// 基于MemoryStream的Buffer
    /// </summary>
    class MemoryStreamTableBuffer : StreamTableBuffer
    {
        public MemoryStreamTableBuffer(MemoryStream stream, int offset, int count, int position = 0)
        {
            _ms = stream;
            _offset = offset;
            _count = count;
        }

        private readonly MemoryStream _ms;
        private readonly int _offset, _count;

        public override int Position
        {
            get { return (int)_ms.Position - _offset; }
            set { _ms.Position = _offset + value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            count = Math.Min(count, _count - (int)_ms.Position + _offset);
            _ms.Read(buffer, offset, count);
            return count;
        }

        public override string ReadString()
        {
            return _ms.ReadString();
        }

        public override StreamTableBuffer CreateFromPosition(int position)
        {
            return new MemoryStreamTableBuffer(_ms, position + _offset, _count - position, 0);
        }

        public override object ReadData(StreamTableColumnType columnType)
        {
            if (columnType == StreamTableColumnType.String)
                return ReadString();

            return _ms.ReadData((TypeCode)columnType);
        }

        public override int ReadIndex(IndexType indexType)
        {
            switch (indexType)
            {
                case IndexType.Byte:
                    return _ms.ReadUInt8();

                case IndexType.UShort:
                    return _ms.ReadUInt16();

                case IndexType.Int:
                    return _ms.ReadInt32();

                default:
                    throw new NotSupportedException();
            }
        }

        public override byte ReadByte()
        {
            return _ms.ReadUInt8();
        }

        public override int ReadInt32()
        {
            return _ms.ReadInt32();
        }

        public override DateTime ReadDateTime()
        {
            return _ms.ReadDateTime();
        }
    }
}
