using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;

namespace Common.Package.Storage
{
    static class StreamTableUtility
    {
        public static void WriteIndex(this Stream stream, IndexType indexType, int index)
        {
            checked
            {
                switch (indexType)
                {
                    case IndexType.Byte:
                        stream.Write((byte)index);
                        break;

                    case IndexType.UShort:
                        stream.Write((ushort)index);
                        break;

                    case IndexType.Int:
                        stream.Write((int)index);
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public static int ReadIndex(this Stream stream, IndexType indexType)
        {
            switch (indexType)
            {
                case IndexType.Byte:
                    return stream.ReadUInt8();

                case IndexType.UShort:
                    return stream.ReadUInt16();

                case IndexType.Int:
                    return stream.ReadInt32();

                default:
                    throw new NotSupportedException();
            }
        }

        public static int ReadIndex(this byte[] buffer, IndexType indexType, int index = 0)
        {
            checked
            {
                switch (indexType)
                {
                    case IndexType.Byte:
                        return buffer.ToUInt8((int)index);

                    case IndexType.UShort:
                        return buffer.ToUInt16((int)index);

                    case IndexType.Int:
                        return buffer.ToInt32((int)index);

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public static int ReadIndex(this byte[] buffer, IndexType indexType, ref int index)
        {
            int value = ReadIndex(buffer, indexType, index);
            index += (int)indexType;
            return value;
        }

        /// <summary>
        /// 读取长度，该长度为紧凑方式存储
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static int ReadLength(this Stream stream)
        {
            return (int)stream.ReadCompressUInt32();
        }

        /// <summary>
        /// 写入紧凑方式存储的长度
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        public static void WriteLength(this Stream stream, int length)
        {
            stream.WriteCompressUInt32((uint)length);
        }

        public static void WriteData(this Stream stream, StreamTableColumnType columnType, object data)
        {
            stream.WriteData((TypeCode)columnType, data);
        }

        public static object ReadData(this Stream stream, StreamTableColumnType columnType)
        {
            return stream.ReadData((TypeCode)columnType);
        }

        public static object ReadData(this byte[] buffer, StreamTableColumnType columnType, ref int offset)
        {
            return buffer.ToData((TypeCode)columnType, ref offset);
        }

        public static object ReadData(this StreamTableBuffer buffer, StreamTableColumnType columnType)
        {
            return buffer.ReadData(columnType);
        }
    }
}
