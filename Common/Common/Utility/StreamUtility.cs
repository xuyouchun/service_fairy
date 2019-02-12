using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace Common.Utility
{
    //[System.Diagnostics.DebuggerStepThrough]
    public unsafe static class StreamUtility
    {
        public static void Write(this Stream stream, byte[] buffer)
        {
            Contract.Requires(stream != null);

            if (!buffer.IsNullOrEmpty())
                stream.Write(buffer, 0, buffer.Length);
        }

        public static void Write(this Stream stream, ArraySegment<byte> buffer)
        {
            Contract.Requires(stream != null);

            if (!buffer.Array.IsNullOrEmpty())
                stream.Write(buffer.Array, buffer.Offset, buffer.Count);
        }

        public static int ReadBytes(this Stream stream, byte[] buffer, int offset, int length, bool throwWhenNotEnough = false)
        {
            Contract.Requires(stream != null);

            int len, curLen = 0;
            while (curLen < length && (len = stream.Read(buffer, offset + curLen, length - curLen)) > 0)
            {
                curLen += len;
            }

            if (throwWhenNotEnough && curLen < length)
                throw new InvalidDataException("流中没有足够的数据可供读取");

            return curLen;
        }

        private static byte[] _Read(Stream stream, int length)
        {
            byte[] bytes = new byte[length];
            int len = ReadBytes(stream, bytes, 0, bytes.Length, true);

            return bytes;
        }

        /// <summary>
        /// 读取指定数量的字符
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] ReadBytes(this Stream stream, int length)
        {
            Contract.Requires(stream != null && length >= 0);
            return _Read(stream, length);
        }

        public static void Write(this Stream stream, int value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static int ReadInt32(this Stream stream)
        {
            Contract.Requires(stream != null);
            return BufferUtility.ToInt32(_Read(stream, 4));
        }

        public static void Write(this Stream stream, uint value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static uint ReadUInt32(this Stream stream)
        {
            return (uint)ReadInt32(stream);
        }

        public static void Write(this Stream stream, long value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static long ReadInt64(this Stream stream)
        {
            Contract.Requires(stream != null);
            return BufferUtility.ToInt64(_Read(stream, 8));
        }

        public static void Write(this Stream stream, ulong value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static ulong ReadUInt64(this Stream stream)
        {
            return (ulong)ReadInt64(stream);
        }

        public static void Write(this Stream stream, short value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static short ReadInt16(this Stream stream)
        {
            Contract.Requires(stream != null);
            return BufferUtility.ToInt16(_Read(stream, 2));
        }

        public static void Write(this Stream stream, ushort value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static ushort ReadUInt16(this Stream stream)
        {
            return (ushort)ReadInt16(stream);
        }

        public static void Write(this Stream stream, DateTime value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static DateTime ReadDateTime(this Stream stream)
        {
            return new DateTime(ReadInt64(stream));
        }

        public static void Write(this Stream stream, TimeSpan value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static TimeSpan ReadTimeSpan(this Stream stream)
        {
            return TimeSpan.FromTicks(ReadInt64(stream));
        }

        public static void Write(this Stream stream, char value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static char ReadChar(this Stream stream)
        {
            Contract.Requires(stream != null);
            return BufferUtility.ToChar(_Read(stream, 2));
        }

        public static void Write(this Stream stream, bool value)
        {
            stream.WriteByte(value ? (byte)1 : (byte)0);
        }

        public static bool ReadBoolean(this Stream stream)
        {
            Contract.Requires(stream != null);
            return stream.ReadByte() != 0;
        }

        public static void Write(this Stream stream, byte value)
        {
            stream.WriteByte(value);
        }

        public static byte ReadUInt8(this Stream stream)
        {
            Contract.Requires(stream != null);
            int b = stream.ReadByte();
            if (b == -1)
                throw new FormatException("流中没有足够的数据可供读取");

            return (byte)b;
        }

        public static void Write(this Stream stream, sbyte value)
        {
            stream.WriteByte((byte)value);
        }

        public static sbyte ReadSByte(this Stream stream)
        {
            return (sbyte)ReadUInt8(stream);
        }

        public static int Write(this Stream stream, string value)
        {
            byte[] bytes = BufferUtility.ToBytes(value);
            stream.Write(bytes);
            stream.WriteByte((byte)'\0');
            return bytes.Length + 1;
        }

        public static string ReadString(this Stream stream)
        {
            Contract.Requires(stream != null);

            byte[] buffer = ReadTo(stream, (byte)'\0');
            if (buffer.Length == 0)
                return string.Empty;

            if (buffer[buffer.Length - 1] == (byte)'\0')
                return BufferUtility.ToString(buffer, 0, buffer.Length - 1);
            else
                return BufferUtility.ToString(buffer);
        }

        public static void Write(this Stream stream, double value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static double ReadDouble(this Stream stream)
        {
            Contract.Requires(stream != null);
            return BufferUtility.ToDouble(_Read(stream, 8));
        }

        public static void Write(this Stream stream, float value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static float ReadSingle(this Stream stream)
        {
            Contract.Requires(stream != null);
            return BufferUtility.ToSingle(_Read(stream, 4));
        }

        public static void Write(this Stream stream, decimal value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static decimal ReadDecimal(this Stream stream)
        {
            Contract.Requires(stream != null);
            return BufferUtility.ToDecimal(_Read(stream, 16));
        }

        public static void Write(this Stream stream, Guid value)
        {
            stream.Write(BufferUtility.ToBytes(value));
        }

        public static Guid ReadGuid(this Stream stream)
        {
            Contract.Requires(stream != null);
            return BufferUtility.ToGuid(_Read(stream, 16));
        }

        public static void WriteCompressUInt32(this Stream stream, uint value)
        {
            Contract.Requires(stream != null);
            stream.Write(BufferUtility.ToCompressUInt32Bytes(value));
        }

        public static uint ReadCompressUInt32(this Stream stream)
        {
            Contract.Requires(stream != null);

            uint value = 0;

            int shift = 0;
            while (true)
            {
                int b = stream.ReadByte();
                if (b == -1)
                    throw new ArgumentOutOfRangeException("已经读取流结尾");

                if ((b & 0x80) != 0)
                {
                    value |= ((uint)b & 0x7F) << shift;
                }
                else
                {
                    value |= ((uint)b << shift);
                    break;
                }

                shift += 7;
                if (shift > 35)
                    throw new FormatException("格式错误，紧凑UInt32字节长度不允许大于5");
            }

            return value;
        }

        public static object ReadData(this Stream stream, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return ReadBoolean(stream);

                case TypeCode.Byte:
                    return ReadUInt8(stream);

                case TypeCode.Char:
                    return ReadChar(stream);

                case TypeCode.DateTime:
                    return ReadDateTime(stream);

                case TypeCode.Decimal:
                    return ReadDecimal(stream);

                case TypeCode.Double:
                    return ReadDouble(stream);

                case TypeCode.Int16:
                    return ReadInt16(stream);

                case TypeCode.Int32:
                    return ReadInt32(stream);

                case TypeCode.Int64:
                    return ReadInt64(stream);

                case TypeCode.SByte:
                    return ReadSByte(stream);

                case TypeCode.Single:
                    return ReadSingle(stream);

                case TypeCode.UInt16:
                    return ReadUInt16(stream);

                case TypeCode.UInt32:
                    return ReadUInt32(stream);

                case TypeCode.UInt64:
                    return ReadUInt64(stream);

                case (TypeCode)32:
                    return ReadGuid(stream);

                case (TypeCode)33:
                    return ReadTimeSpan(stream);

                default:
                    throw new NotSupportedException("不支持的数据类型：" + typeCode);
            }
        }

        public static void WriteData(this Stream stream, TypeCode typeCode, object data)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    Write(stream, (bool)data);
                    break;

                case TypeCode.Byte:
                    Write(stream, (byte)data);
                    break;

                case TypeCode.Char:
                    Write(stream, (char)data);
                    break;

                case TypeCode.DateTime:
                    Write(stream, (DateTime)data);
                    break;

                case TypeCode.Decimal:
                    Write(stream, (decimal)data);
                    break;

                case TypeCode.Double:
                    Write(stream, (double)data);
                    break;

                case TypeCode.Int16:
                    Write(stream, (short)data);
                    break;

                case TypeCode.Int32:
                    Write(stream, (int)data);
                    break;

                case TypeCode.Int64:
                    Write(stream, (long)data);
                    break;

                case TypeCode.SByte:
                    Write(stream, (sbyte)data);
                    break;

                case TypeCode.Single:
                    Write(stream, (float)data);
                    break;

                case TypeCode.UInt16:
                    Write(stream, (ushort)data);
                    break;

                case TypeCode.UInt32:
                    Write(stream, (uint)data);
                    break;

                case TypeCode.UInt64:
                    Write(stream, (ulong)data);
                    break;

                case (TypeCode)32:
                    Write(stream, (Guid)data);
                    break;

                case (TypeCode)33:
                    Write(stream, (TimeSpan)data);
                    break;

                default:
                    throw new NotSupportedException("不支持的数据类型：" + typeCode);
            }
        }

        /// <summary>
        /// 提取指定长度的字节流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="maxLength"></param>
        /// <param name="atEnd"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Stream stream, int maxLength, out bool atEnd)
        {
            Contract.Requires(stream != null);

            if (stream.CanSeek)
            {
                int rest = (int)(stream.Length - stream.Position);
                byte[] buffer = new byte[Math.Min(rest, maxLength)];
                int len, pos = 0;
                while (pos < buffer.Length && (len = stream.Read(buffer, pos, buffer.Length - pos)) > 0)
                {
                    pos += len;
                }

                atEnd = (stream.Position >= stream.Length - 1);
                return buffer;
            }
            else
            {
                atEnd = false;
                MemoryStream ms = new MemoryStream();
                byte[] buffer = new byte[Math.Min(1024, maxLength)];
                while (ms.Length < maxLength)
                {
                    int len = stream.Read(buffer, 0, Math.Min(maxLength - (int)ms.Length, buffer.Length));
                    if (len > 0)
                        ms.Write(buffer, 0, len);
                    else
                        atEnd = true;
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 转换为字节流
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Stream stream)
        {
            Contract.Requires(stream != null);

            if (stream is MemoryStream)
                return ((MemoryStream)stream).ToArray();

            if (stream.CanSeek)
            {
                byte[] buffer = new byte[stream.Length];
                int pos = 0, len;

                while ((len = stream.Read(buffer, pos, buffer.Length - pos)) > 0)
                {
                    pos += len;
                }

                return buffer;
            }
            else
            {
                byte[] buffer = new byte[1024];
                MemoryStream ms = new MemoryStream();
                int len;

                while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, len);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将一个流中的内容写到另一个流中
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        public static void CopyStream(this Stream dst, Stream src)
        {
            Contract.Requires(dst != null && src != null);

            MemoryStream ms = src as MemoryStream;
            byte[] buffer;
            if (ms != null && (buffer = GetStreamBuffer(ms)) != null)
            {
                dst.Write(buffer, (int)ms.Position, (int)(ms.Length - ms.Position));
            }
            else
            {
                buffer = new byte[1024];
                int len;

                while ((len = src.Read(buffer, 0, buffer.Length)) > 0)
                {
                    dst.Write(buffer, 0, len);
                }
            }
        }

        private static readonly FieldInfo _exposableFieldInfo = typeof(MemoryStream).GetField("_exposable", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        public static byte[] GetStreamBuffer(this Stream stream)
        {
            Contract.Requires(stream != null);

            MemoryStream ms = stream as MemoryStream;
            if (ms == null)
                return null;

            if (_exposableFieldInfo != null && object.Equals(_exposableFieldInfo.GetValue(ms), false))
                return null;

            try
            {
                return ms.GetBuffer();
            }
            catch(UnauthorizedAccessException)
            {
                return null;
            }
        }

        /// <summary>
        /// 将一个流中的内容写到另一个流中
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <param name="length"></param>
        public static void CopyStream(this Stream dst, Stream src, int length)
        {
            Contract.Requires(dst != null && src != null && length > 0);

            byte[] buffer = src.GetStreamBuffer();
            if (buffer != null)
            {
                dst.Write(buffer, (int)src.Position, Math.Min((int)(src.Length - src.Position), length));
            }
            else
            {
                buffer = new byte[1024];
                int len, curLen = 0;

                while ((len = src.Read(buffer, 0, Math.Min(buffer.Length, length - curLen))) > 0 && curLen < length)
                {
                    dst.Write(buffer, 0, len);
                    curLen += len;
                }
            }
        }

        private static byte[] ReadTo(Stream stream, byte b)
        {
            Contract.Requires(stream != null);

            byte[] buffer = stream.GetStreamBuffer();
            if (buffer != null)  // 针对MemorySteram的特殊处理
            {
                fixed (byte* fixedPtr = buffer)
                {
                    byte* p = fixedPtr + stream.Position, p0 = p;
                    if (BufferUtility.JumpTo(ref p, (int)(stream.Length - stream.Position), b))
                        p++;

                    byte[] bytes = BufferUtility.ToBytes(p0, (int)(p - p0));
                    stream.Seek(bytes.Length, SeekOrigin.Current);
                    return bytes;
                }
            }

            if (stream.CanSeek)  // 支持定位的流
            {
                buffer = new byte[16];
                MemoryStream ms = new MemoryStream();

                int len, pos = (int)stream.Position;
                while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    int index = BufferUtility.FindIndex(buffer, len, b);
                    if (index < 0)
                    {
                        ms.Write(buffer);
                    }
                    else
                    {
                        ms.Write(buffer, 0, index + 1);
                        stream.Seek(-(len - index - 1), SeekOrigin.Current);
                        break;
                    }
                }

                return ms.ToArray();
            }
            else  // 不支持定位的流
            {
                MemoryStream ms = new MemoryStream();
                int curByte;
                while ((curByte = stream.ReadByte()) != -1)
                {
                    byte b0 = (byte)curByte;
                    ms.Write(b0);
                    if (b0 == b)
                        break;
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将整个目录压缩为字节数组
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static byte[] CompressDirectory(string directory, Func<string, bool> filter = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                CompressDirectory(ms, directory, filter);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将指定的文件压缩为字节数组
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static byte[] CompressDirectory(IEnumerable<CompressFileItemBase> items)
        {
            Contract.Requires(items != null);
            using (MemoryStream ms = new MemoryStream())
            {
                CompressDirectory(ms, items);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将整个目录压缩到流中
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filter"></param>
        /// <param name="stream"></param>
        public static void CompressDirectory(Stream stream, string directory, Func<string, bool> filter = null)
        {
            Contract.Requires(stream != null && directory != null);

            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("目录不存在：" + directory);

            CompressDirectory(stream, _GetFileItems(directory, filter));
        }

        private static IEnumerable<CompressFileItem> _GetFileItems(string directory, Func<string, bool> filter)
        {
            foreach (string file in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
            {
                if (filter != null && !filter(file))
                    continue;

                string relPath = file.Substring(directory.Length).TruncateFrom('\\');
                yield return new CompressFileItem(file, relPath);
            }
        }

        /// <summary>
        /// 将文件压缩到流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="items"></param>
        public static void CompressDirectory(Stream stream, IEnumerable<CompressFileItemBase> items)
        {
            Contract.Requires(stream != null && items != null);

            using (GZipStream gs = new GZipStream(stream, CompressionMode.Compress, true))
            {
                gs.WriteByte((byte)1); // 第一个字节代表版本号
                foreach (CompressFileItemBase item in items)
                {
                    _WriteString(gs, item.FileName);

                    gs.WriteByte((byte)1);  // 时间
                    _WriteLong(gs, item.LastWriteUtcTime.Ticks);

                    gs.WriteByte((byte)127);  // 文件内容
                    using (Stream fs = item.GetStream())
                    {
                        _WriteStream(gs, fs);
                    }
                }
            }
        }

        private static void _WriteInt(Stream stream, int value)
        {
            stream.Write(new byte[] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) });
        }

        private static int _ReadInt(Stream stream)
        {
            int value;
            if (!_TryReadInt(stream, out value))
                throw new FormatException("流格式错误");

            return value;
        }

        private static bool _TryReadInt(Stream stream, out int value)
        {
            byte[] bytes = new byte[4];
            if (ReadBytes(stream, bytes, 0, bytes.Length) < 4)
            {
                value = default(int);
                return false;
            }

            value = bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24);
            return true;
        }

        private static void _WriteLong(Stream stream, long value)
        {
            stream.Write(new byte[] { 
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24),
                (byte)(value >> 32), (byte)(value >> 40), (byte)(value >> 48), (byte)(value >> 56)
            });
        }

        private static long _ReadLong(Stream stream)
        {
            byte[] bytes = new byte[8];
            if (ReadBytes(stream, bytes, 0, bytes.Length) < 8)
                throw new FormatException("流格式错误");

            return (long)bytes[0] | ((long)bytes[1] << 8) | ((long)bytes[2] << 16) | ((long)bytes[3] << 24)
                | ((long)bytes[4] << 32) | ((long)bytes[5] << 40) | ((long)bytes[6] << 48) | ((long)bytes[7] << 56);
        }

        private static void _WriteString(Stream stream, string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            _WriteInt(stream, bytes.Length);
            stream.Write(bytes);
        }

        private static string _ReadString(Stream stream)
        {
            int length;
            if (!_TryReadInt(stream, out length))
                return null;

            byte[] bytes = new byte[length];
            if (ReadBytes(stream, bytes, 0, length) < length)
                throw new FormatException("流格式错误");

            return Encoding.UTF8.GetString(bytes);
        }

        private static void _WriteStream(Stream stream, Stream stream2)
        {
            _WriteInt(stream, (int)stream2.Length);
            CopyStream(stream, stream2);
        }

        private static void _ReadStream(Stream stream, Stream stream2)
        {
            int length = _ReadInt(stream);
            byte[] buffer = new byte[256];

            int len;
            while ((len = stream.Read(buffer, 0, Math.Min(length, buffer.Length))) > 0)
            {
                if (stream2 != null)
                    stream2.Write(buffer, 0, len);
                length -= len;
            }

            if (length != 0)
                throw new FormatException("流格式错误");
        }

        /// <summary>
        /// 从流中读取指定的长度到指定的位置
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="p"></param>
        /// <param name="length"></param>
        public static void ReadToBuffer(this Stream stream, byte* p, int length)
        {
            Contract.Requires(stream != null && length >=0 );

            int len;
            byte[] bytes = new byte[32];
            fixed (byte* pSrc = bytes)
            {
                while (length > 0 && (len = stream.Read(bytes, 0, Math.Min(length, bytes.Length))) > 0)
                {
                    BufferUtility.Copy(p, pSrc, len);
                    p += len;
                    length -= len;
                }
            }
        }

        /// <summary>
        /// 将压缩文件格式的字节数组解压到指定的目录中
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="directory"></param>
        public static void DecompressToDirectory(byte[] buffer, string directory)
        {
            Contract.Requires(buffer != null && directory != null);
            DecompressToDirectory(new MemoryStream(buffer), directory);
        }

        /// <summary>
        /// 将流解压到指定的目录中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="directory"></param>
        public static void DecompressToDirectory(Stream stream, string directory)
        {
            Contract.Requires(stream != null && directory != null);

            using (GZipStream gs = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                int r = gs.ReadByte(); // 读出版本号

                string filePath;
                while ((filePath = _ReadString(gs)) != null)
                {
                    string fullPath = Path.Combine(directory, filePath);
                    string dir = Path.GetDirectoryName(fullPath);
                    if (!File.Exists(dir))
                        Directory.CreateDirectory(dir);

                    byte b;
                    DateTime dt = default(DateTime);
                    do
                    {
                        switch (b = (byte)gs.ReadByte())
                        {
                            case 1:  // 时间
                                dt = new DateTime(_ReadLong(gs));
                                break;

                            case 127:  // 文件内容
                                using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                                {
                                    _ReadStream(gs, fs);
                                }

                                if (dt != default(DateTime))
                                {
                                    File.SetLastWriteTimeUtc(fullPath, dt);
                                }
                                break;

                            default:
                                throw new FormatException("流格式错误");
                        }
                    } while (b != 127);
                }
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        public static void Serialize(Stream stream, object obj)
        {
            Contract.Requires(obj != null && stream != null);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);
            stream.Flush();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(object obj)
        {
            Contract.Requires(obj != null);

            MemoryStream ms = new MemoryStream();
            Serialize(ms, obj);
            return ms.ToArray();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream)
        {
            return (T)Deserialize(stream);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object Deserialize(Stream stream)
        {
            Contract.Requires(stream != null);

            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(stream);
        }

        /// <summary>
        /// 逆序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] bytes)
        {
            Contract.Requires(bytes != null);

            return (T)Deserialize(bytes);
        }

        /// <summary>
        /// 逆序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object Deserialize(byte[] bytes)
        {
            Contract.Requires(bytes != null);

            return Deserialize(new MemoryStream(bytes));
        }

        /// <summary>
        /// 逆序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static object Deserialize(byte[] bytes, int start, int count)
        {
            Contract.Requires(bytes != null);

            return Deserialize(new MemoryStream(bytes, start, count));
        }

        /// <summary>
        /// 逆序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] bytes, int start, int count)
        {
            return (T)Deserialize(bytes, start, count);
        }
    }

    public abstract class CompressFileItemBase
    {
        public CompressFileItemBase(string filename)
        {
            Contract.Requires(filename != null);

            FileName = filename;
        }

        public string FileName { get; private set; }

        public abstract Stream GetStream();

        public abstract DateTime LastWriteUtcTime { get; }
    }

    /// <summary>
    /// 压缩格式的文件项
    /// </summary>
    public class CompressFileItem : CompressFileItemBase
    {
        public CompressFileItem(string fullFilePath, string fileName)
            : base(fileName)
        {
            Contract.Requires(fullFilePath != null);

            FullFilePath = fullFilePath;
        }

        /// <summary>
        /// 完整的文件路径
        /// </summary>
        public string FullFilePath { get; private set; }

        public override Stream GetStream()
        {
            return new FileStream(FullFilePath, FileMode.Open, FileAccess.Read);
        }

        public override DateTime LastWriteUtcTime
        {
            get
            {
                return File.GetLastWriteTimeUtc(FullFilePath);
            }
        }
    }

    /// <summary>
    /// 基于字节流的压缩文件项
    /// </summary>
    public class BufferCompressFileItem : CompressFileItemBase
    {
        public BufferCompressFileItem(string filename, byte[] buffer, DateTime lastWriteUtcTime = default(DateTime))
            : base(filename)
        {
            Buffer = buffer;
            _lastWriteUtcTime = lastWriteUtcTime == default(DateTime) ? DateTime.UtcNow : lastWriteUtcTime;
        }

        public BufferCompressFileItem(string filename, string content, DateTime lastWriteUtcTime = default(DateTime))
            : this(filename, Encoding.UTF8.GetBytes(content ?? ""), lastWriteUtcTime)
        {

        }

        public byte[] Buffer { get; private set; }

        private readonly DateTime _lastWriteUtcTime;

        public override DateTime LastWriteUtcTime { get { return _lastWriteUtcTime; } }

        public override Stream GetStream()
        {
            if (Buffer.IsNullOrEmpty())
                return Stream.Null;

            return new MemoryStream(Buffer);
        }
    }
}
