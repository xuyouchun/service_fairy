using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;

namespace Common.Package.Storage
{
    /// <summary>
    /// 堆的读取器
    /// </summary>
    class StreamTableHeapReader
    {
        public StreamTableHeapReader(StreamTableBuffer buffer, IndexType indexType)
        {
            _buffer = buffer;
            _indexType = indexType;
        }

        private readonly StreamTableBuffer _buffer;
        private readonly IndexType _indexType;

        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="bodyBuffer"></param>
        /// <returns></returns>
        public string ReadString(StreamTableBuffer bodyBuffer)
        {
            int position = bodyBuffer.ReadIndex(_indexType);
            return ReadString(position);
        }

        /// <summary>
        /// 读取指定位置的字符串
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public string ReadString(int position)
        {
            if (position == 0)
                return string.Empty;

            _buffer.Position = position;
            return _buffer.ReadString();
        }

        /// <summary>
        /// 读取一个索引位置或长度
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int ReadIndex(int position)
        {
            _buffer.Position = position;
            return _buffer.ReadIndex(_indexType);
        }

        /// <summary>
        /// 读取堆数据
        /// </summary>
        /// <param name="columnType"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public object ReadData(StreamTableColumnType columnType, int position)
        {
            _buffer.Position = position;
            return _buffer.ReadData(columnType);
        }

        /// <summary>
        /// 读取堆数据
        /// </summary>
        /// <param name="columnType"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public object ReadData(StreamTableColumnType columnType, ref int position)
        {
            object result = ReadData(columnType, position);
            position = _buffer.Position;
            return result;
        }

        /// <summary>
        /// 读取不定类型的数据
        /// </summary>
        /// <param name="bodyBuffer"></param>
        /// <returns></returns>
        public object ReadVarType(StreamTableBuffer bodyBuffer)
        {
            StreamTableColumnType columnType = (StreamTableColumnType)bodyBuffer.ReadByte();
            int position = bodyBuffer.ReadIndex(_indexType);
            if (position == 0)
                return null;

            if (columnType == StreamTableColumnType.String)
            {
                position = _buffer.ReadIndex(_indexType);
                return ReadString(position);
            }

            return _buffer.ReadData(columnType);
        }

        /// <summary>
        /// 读取一个字节
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte ReadByte(int offset)
        {
            _buffer.Position = offset;
            return _buffer.ReadByte();
        }

        public byte ReadByte(ref int offset)
        {
            byte r = ReadByte(offset);
            offset = _buffer.Position;
            return r;
        }
    }
}
