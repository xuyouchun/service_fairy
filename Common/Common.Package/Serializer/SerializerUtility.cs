using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.IO;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Package.Serializer
{
    public static class SerializerUtility
    {
        /// <summary>
        /// 将对象序列化到流中
        /// </summary>
        /// <param name="format"></param>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        public static void Serialize(DataFormat format, object obj, Stream stream)
        {
            Contract.Requires(stream != null);
            SerializerFactory.CreateSerializer(format).Serialize(obj, stream);
        }

        /// <summary>
        /// 将对象序列化为字节数组
        /// </summary>
        /// <param name="format"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(DataFormat format, object obj)
        {
            MemoryStream ms = new MemoryStream();
            Serialize(format, obj, ms);
            return ms.ToArray();
        }

        /// <summary>
        /// 将对象序列化为字符串
        /// </summary>
        /// <param name="format"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(DataFormat format, object obj)
        {
            byte[] buffer = SerializeToBytes(format, obj);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// 从流中反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format"></param>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object Deserialize(DataFormat format, Type type, Stream stream)
        {
            Contract.Requires(stream != null);
            return SerializerFactory.CreateSerializer(format).Deserialize(type, stream);
        }

        /// <summary>
        /// 从Byte数组中反序列化对象
        /// </summary>
        /// <param name="format"></param>
        /// <param name="type"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static object Deserialize(DataFormat format, Type type, byte[] buffer)
        {
            Contract.Requires(type != null && buffer != null);
            return Deserialize(format, type, new MemoryStream(buffer));
        }

        /// <summary>
        /// 从流中反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T Deserialize<T>(DataFormat format, Stream stream)
        {
            Contract.Requires(stream != null);
            return (T)Deserialize(format, typeof(T), stream);
        }

        /// <summary>
        /// 从Byte数组中反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T Deserialize<T>(DataFormat format, byte[] buffer)
        {
            Contract.Requires(buffer != null);
            return (T)Deserialize(format, typeof(T), buffer);
        }

        /// <summary>
        /// 将对象序列化，将基本数据类型进行优化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public unsafe static byte[] OptimalBinarySerialize(object obj)
        {
            if (obj == null)
                return Array<byte>.Empty;

            Type type = obj.GetType();
            TypeCode typeCode = _GetTypeCode(type);
            byte bType = (byte)(int)typeCode;
            byte *p;

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return new byte[] { bType, ((bool)obj) ? (byte)1 : (byte)0 };

                case TypeCode.Byte:
                    return new byte[] { bType, (byte)obj };

                case TypeCode.Char:
                    char c = (char)obj;
                    return new byte[] { bType, (byte)c, (byte)(c >> 8) };

                case TypeCode.DBNull:
                    return new byte[] { bType };

                case TypeCode.DateTime:
                    long ticks = ((DateTime)obj).Ticks;
                    p = (byte*)&ticks;
                    return new byte[] { bType, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p };

                case TypeCode.Decimal:
                    decimal decimalValue = (decimal)obj;
                    p = (byte*)&decimalValue;
                    return new byte[] { bType,
                        *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++,
                        *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p,
                    };

                case TypeCode.Double:
                    double doubleValue = (double)obj;
                    p = (byte*)&doubleValue;
                    return new byte[] { bType, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p };

                case TypeCode.Empty:
                    return new byte[] { bType };

                case TypeCode.Int16:
                    short shortValue = (short)obj;
                    p = (byte*)&shortValue;
                    return new byte[] { bType, *p++, *p };

                case TypeCode.Int32:
                    int intValue = (int)obj;
                    p = (byte*)&intValue;
                    return new byte[] { bType, *p++, *p++, *p++, *p };

                case TypeCode.Int64:
                    long longValue = (long)obj;
                    p = (byte*)&longValue;
                    return new byte[] { bType, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p };

                case TypeCode.SByte:
                    return new byte[] { bType, (byte)(sbyte)obj };

                case TypeCode.Single:
                    float floatValue = (float)obj;
                    p = (byte*)&floatValue;
                    return new byte[] { bType, *p++, *p++, *p++, *p };

                case TypeCode.String:
                    string stringValue = (string)obj;
                    int bytesCount = Encoding.UTF8.GetByteCount(stringValue);
                    byte[] buffer = new byte[bytesCount + 2];
                    buffer[0] = bType;
                    Encoding.UTF8.GetBytes(stringValue, 0, stringValue.Length, buffer, 1);
                    buffer[buffer.Length - 1] = (byte)'\0';
                    return buffer;

                case TypeCode.UInt16:
                    ushort ushortValue = (ushort)obj;
                    p = (byte*)&ushortValue;
                    return new byte[] { bType, *p++, *p };

                case TypeCode.UInt32:
                    uint uintValue = (uint)obj;
                    p = (byte*)&uintValue;
                    return new byte[] { bType, *p++, *p++, *p++, *p };

                case TypeCode.UInt64:
                    ulong ulongValue = (ulong)obj;
                    p = (byte*)&ulongValue;
                    return new byte[] { bType, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p };

                case TYPECODE_GUID:
                    Guid guidValue = (Guid)obj;
                    p = (byte*)&guidValue;
                    return new byte[] { bType,
                        *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++,
                        *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p
                    };

                case TYPECODE_TIMESPAN:
                    TimeSpan timeSpanValue = (TimeSpan)obj;
                    ticks = timeSpanValue.Ticks;
                    p = (byte*)&ticks;
                    return new byte[] { bType, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p };

                default:
                case TypeCode.Object:
                    MemoryStream ms = new MemoryStream();
                    ms.WriteByte(bType);
                    BinaryObjectSerializer.Instance.Serialize(obj, ms);
                    return ms.ToArray();
            }
        }

        private static TypeCode _GetTypeCode(Type type)
        {
            if (type == typeof(Guid))
                return TYPECODE_GUID;

            if (type == typeof(TimeSpan))
                return TYPECODE_TIMESPAN;

            return Type.GetTypeCode(type);
        }

        const TypeCode TYPECODE_GUID = (TypeCode)32;
        const TypeCode TYPECODE_TIMESPAN = (TypeCode)33;

        /// <summary>
        /// 将优化的字节流反序列化为对象
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static object OptimalBinaryDeserialize(byte[] buffer, ref int offset)
        {
            Contract.Requires(buffer != null);

            TypeCode typeCode = (TypeCode)buffer[offset++];
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return buffer[offset++] != (byte)0;

                case TypeCode.Byte:
                    return buffer[offset++];

                case TypeCode.Char:
                    return BufferUtility.ToChar(buffer, ref offset);

                case TypeCode.DBNull:
                    return DBNull.Value;

                case TypeCode.DateTime:
                    return BufferUtility.ToDateTime(buffer, ref offset);

                case TypeCode.Decimal:
                    return BufferUtility.ToDecimal(buffer, ref offset);

                case TypeCode.Double:
                    return BufferUtility.ToDouble(buffer, ref offset);

                case TypeCode.Empty:
                    return null;

                case TypeCode.Int16:
                    return BufferUtility.ToUInt16(buffer, ref offset);

                case TypeCode.Int32:
                    return BufferUtility.ToInt32(buffer, ref offset);

                case TypeCode.Int64:
                    return BufferUtility.ToInt64(buffer, ref offset);

                case TypeCode.SByte:
                    return BufferUtility.ToSByte(buffer, ref offset);

                case TypeCode.Single:
                    return BufferUtility.ToSingle(buffer, ref offset);

                case TypeCode.String:
                    int index = BufferUtility.FindIndex(buffer, offset, buffer.Length - offset, (byte)'\0');
                    int count = (index < 0) ? buffer.Length - offset : (index - offset);
                    string s = Encoding.UTF8.GetString(buffer, offset, count);
                    offset += count;
                    return s;

                case TypeCode.UInt16:
                    return BufferUtility.ToUInt16(buffer, ref offset);

                case TypeCode.UInt32:
                    return BufferUtility.ToUInt32(buffer, ref offset);

                case TypeCode.UInt64:
                    return BufferUtility.ToUInt64(buffer, ref offset);

                case TYPECODE_GUID:
                    return BufferUtility.ToGuid(buffer, ref offset);

                case TYPECODE_TIMESPAN:
                    return BufferUtility.ToTimeSpan(buffer, ref offset);

                default:
                case TypeCode.Object:
                    return Deserialize(DataFormat.Binary, null, new MemoryStream(buffer, offset, buffer.Length - offset));
            }
        }

        /// <summary>
        /// 将优化的字节流反序列化为对象
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static object OptimalBinaryDeserialize(byte[] buffer, int offset)
        {
            return OptimalBinaryDeserialize(buffer, ref offset);
        }

        /// <summary>
        /// 将优化的字节流反序列化为对象
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static object OptimalBinaryDeserialize(byte[] buffer)
        {
            return OptimalBinaryDeserialize(buffer, 0);
        }

        /// <summary>
        /// 将优化的字节流反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static T OptimalBinaryDeserialize<T>(byte[] buffer, ref int offset)
        {
            return (T)OptimalBinaryDeserialize(buffer, ref offset);
        }

        /// <summary>
        /// 将优化的字节流反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static T OptimalBinaryDeserialize<T>(byte[] buffer, int offset)
        {
            return OptimalBinaryDeserialize<T>(buffer, ref offset);
        }

        /// <summary>
        /// 将优化的字节流反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T OptimalBinaryDeserialize<T>(byte[] buffer)
        {
            return OptimalBinaryDeserialize<T>(buffer, 0);
        }
    }
}
