using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Package.Storage
{
    /// <summary>
    /// 基于字节流的读取
    /// </summary>
    class BytesStreamTableBuffer : StreamTableBuffer
    {
        public BytesStreamTableBuffer(byte[] buffer, int offset, int count, int position = 0)
        {
            _buffer = buffer;
            _offset = offset;
            _count = count;

            _position = offset + position;
        }

        public BytesStreamTableBuffer(byte[] buffer, int position = 0)
            : this(buffer, 0, buffer.Length, position)
        {

        }

        private int _position;

        public override int Position
        {
            get { return _position - _offset; }
            set { _position = _offset + value; }
        }

        private readonly byte[] _buffer;
        private readonly int _offset, _count;

        public override StreamTableBuffer CreateFromPosition(int position)
        {
            return new BytesStreamTableBuffer(_buffer, _offset + position, _count - (_offset + _position), 0);
        }

        /// <summary>
        /// 读取指定位置的数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            count = Math.Min(count, _count - _position + _offset);
            BufferUtility.Copy(buffer, _buffer, offset, _offset, count);
            _position += count;
            return count;
        }

        public override string ReadString()
        {
            int index = BufferUtility.FindIndex(_buffer, _position, (int)(_count - _position + _offset), (byte)'\0');
            if (index < 0)
                throw new FormatException("流格式错误，字符串没有以\\0结尾");
            
            string s = Encoding.UTF8.GetString(_buffer, _position, (index - _position));
            _position = index + 1;
            return s;
        }

        public override object ReadData(StreamTableColumnType columnType)
        {
            return _buffer.ToData((TypeCode)columnType, ref _position);
        }

        public override int ReadIndex(IndexType indexType)
        {
            switch (indexType)
            {
                case IndexType.Byte:
                    return _buffer.ToUInt8(ref _position);

                case IndexType.UShort:
                    return _buffer.ToUInt16(ref _position);

                case IndexType.Int:
                    return _buffer.ToInt32(ref _position);

                default:
                    throw new NotSupportedException();
            }
        }

        public override byte ReadByte()
        {
            return _buffer.ToUInt8(ref _position);
        }

        public override int ReadInt32()
        {
            return _buffer.ToInt32(ref _position);
        }

        public override DateTime ReadDateTime()
        {
            return _buffer.ToDateTime(ref _position);
        }
    }
}
