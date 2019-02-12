using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Utility
{
    [System.Diagnostics.DebuggerStepThrough]
    public unsafe static class BufferUtility
    {
        /// <summary>
        /// 寻找指定字节所在的位置
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int FindIndex(this byte[] buffer, byte b)
        {
            Contract.Requires(buffer != null);

            return FindIndex(buffer, 0, buffer.Length, b);
        }

        /// <summary>
        /// 寻找指定字节所在的位置
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int FindIndex(this byte[] buffer, int length, byte b)
        {
            Contract.Requires(buffer != null);

            return FindIndex(buffer, 0, length, b);
        }

        /// <summary>
        /// 寻找指定字节所在的位置
        /// </summary>
        /// <param name="start"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int FindIndex(this byte[] buffer, int start, int length, byte b)
        {
            Contract.Requires(buffer != null && length > 0 && length < buffer.Length);

            fixed (byte* p = buffer)
            {
                int index = FindIndex(p + start, length, b);
                if (index < 0)
                    return index;

                return index + start;
            }
        }

        /// <summary>
        /// 寻找指定字节所在的位置
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int FindIndex(byte* p, int length, byte b)
        {
            //Contract.Requires(p != null && length >= 0);

            byte* pStart = p;
            byte* pEnd = p + length;

            while (p < pEnd)
            {
                if (*p == b)
                    return (int)(p - pStart);

                p++;
            }

            return -1;
        }

        /// <summary>
        /// 寻找指定byte数组所在的位置
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int FindIndex(byte* p, int length, byte[] bytes)
        {
            fixed (byte* p2 = bytes)
            {
                return FindIndex(p, length, p2, bytes.Length);
            }
        }

        /// <summary>
        /// 寻找指定的流所在的位置
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static int FindIndex(byte* p, int length, byte* p2, int length2)
        {
            //Contract.Requires(p != null && length != null && p2 != null && length2 != null);

            if (length2 == 1)
                return FindIndex(p, length, *p2);

            byte* pStart = p;
            byte* pEnd = p + length - length2;
            byte b = *p2;

            while (p < pEnd)
            {
                if (*p == b && Equals(p, p2, length2))
                    return (int)(p - pStart);

                p++;
            }

            return -1;
        }

        /// <summary>
        /// 判断两个字节数组是否相同
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p2"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool Equals(byte* p, byte* p2, int length)
        {
            byte* pEnd = p + length;
            while (p < pEnd)
            {
                if (*p++ != *p2++)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 跳到指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool JumpTo(ref byte* p, int length, byte b)
        {
            if (*p == b)
                return true;

            byte* pEnd = p + length;
            p++;
            while (p < pEnd)
            {
                if (*p == b)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳到指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool JumpTo(ref byte* p, int length, byte[] bytes)
        {
            if (bytes.Length < 4)
            {
                if (bytes.Length == 1)
                    return JumpTo(ref p, length, bytes[0]);

                if (bytes.Length == 2)
                    return JumpTo(ref p, length, bytes[0], bytes[1]);

                if (bytes.Length == 3)
                    return _JumpTo(ref p, length, bytes[0], bytes[1], bytes[2]);

                return _JumpTo(ref p, length, bytes[0], bytes[1], bytes[2], bytes[3]);
            }

            byte* pEnd = p + length;
            byte b0 = bytes[0], b1 = bytes[1], b2 = bytes[2], b3 = bytes[3], b4 = bytes[4];
            if (*p == b0 || *p == b1 || *p == b2 || *p == b3 || *p == b4)
                return true;

            p++;
            fixed (byte* p2Start = bytes)
            {
                byte* p2End = p2Start + bytes.Length;
                while (p < pEnd)
                {
                    if (*p == b0 || *p == b1 || *p == b2 || *p == b3 || *p == b4)
                    {
                        return true;
                    }

                    for (byte* p2 = p2Start + 5; p2 < p2End; p2++)
                    {
                        if (*p == *p2)
                            return true;
                    }

                    p++;
                }
            }

            return false;
        }

        public static bool JumpTo(ref byte* p, int length, byte b0, byte b1)
        {
            byte b = *p;
            if (b == b0 || b == b1)
                return true;

            byte* pEnd = p + length;
            p++;
            while (p < pEnd)
            {
                b = *p;
                if (b == b0 || b == b1)
                    return true;

                p++;
            }

            return false;
        }

        private static bool _JumpTo(ref byte* p, int length, byte b0, byte b1, byte b2)
        {
            byte b = *p;
            if (b == b0 || b == b1 || b == b2)
                return true;

            byte* pEnd = p + length;
            p++;
            while (p < pEnd)
            {
                b = *p;
                if (b == b0 || b == b1 || b == b2)
                    return true;

                p++;
            }

            return false;
        }

        private static bool _JumpTo(ref byte* p, int length, byte b0, byte b1, byte b2, byte b3)
        {
            byte b = *p;
            if (b == b0 || b == b1 || b == b2 || b == b3)
                return true;

            byte* pEnd = p + length;
            p++;
            while (p < pEnd)
            {
                b = *p;
                if (b == b0 || b == b1 || b == b2 || b == b3)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳过指定的字符，指针将停留在指定的字符的后面
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Skip(ref byte* p, int length, byte b)
        {
            if (*p != b)
                return true;

            byte* pEnd = p + length;

            p++;
            while (p < pEnd)
            {
                if (*p != b)
                    return true;

                p++;
            }

            return false;
        }

        private static bool _Skip(ref byte* p, int length, byte b0, byte b1)
        {
            if (*p != b0 && *p != b1)
                return true;

            byte* pEnd = p + length;

            p++;
            while (p < pEnd)
            {
                if (*p != b0 && *p != b1)
                    return true;

                p++;
            }

            return false;
        }

        private static bool _Skip(ref byte* p, int length, byte b0, byte b1, byte b2)
        {
            if (*p != b0 && *p != b1 && *p != b2)
                return true;

            byte* pEnd = p + length;

            p++;
            while (p < pEnd)
            {
                if (*p != b0 && *p != b1 && *p != b2)
                    return true;

                p++;
            }

            return false;
        }

        private static bool _Skip(ref byte* p, int length, byte b0, byte b1, byte b2, byte b3)
        {
            if (*p != b0 && *p != b1 && *p != b2 && *p != b3)
                return true;

            byte* pEnd = p + length;

            p++;
            while (p < pEnd)
            {
                if (*p != b0 && *p != b1 && *p != b2 && *p != b3)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳过指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool Skip(ref byte* p, int length, byte[] bytes)
        {
            if (bytes.Length <= 4)
            {
                if (bytes.Length == 1)
                    return Skip(ref p, length, bytes[0]);

                if (bytes.Length == 2)
                    return _Skip(ref p, length, bytes[0], bytes[1]);

                if (bytes.Length == 3)
                    return _Skip(ref p, length, bytes[0], bytes[1], bytes[2]);

                return _Skip(ref p, length, bytes[0], bytes[1], bytes[2], bytes[3]);
            }

            byte* pEnd = p + length;
            byte b0 = bytes[0], b1 = bytes[1], b2 = bytes[2], b3 = bytes[3], b4 = bytes[4];
            if (*p != b0 && *p != b1 && *p != b2 && *p != b3 && *p != b4)
                return false;

            p++;
            fixed (byte* p2Start = bytes)
            {
                byte *p2End = p2Start + bytes.Length;
                while (p < pEnd)
                {
                    if (*p == b0 || *p == b1 || *p == b2 || *p == b3 || *p == b4)
                    {
                        p++;
                        continue;
                    }

                    for (byte* p2 = p2Start + 5; p2 < p2End; p2++)
                    {
                        if (*p == *p2)
                            goto _continue;
                    }

                    return true;

                _continue:
                    p++;
                }
            }

            return false;
        }

        /// <summary>
        /// 跳过空白字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool SkipWhiteSpace(ref byte* p, int length)
        {
            return Skip(ref p, length, _whiteSpaces);
        }

        private static readonly byte[] _whiteSpaces = new byte[] { (byte)' ', (byte)'\t', (byte)'\r', (byte)'\n' };

        /// <summary>
        /// 转换为字节数组
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] ToBytes(byte* p, int length)
        {
            byte[] buffer = new byte[length];
            fixed (byte* p0 = buffer)
            {
                Copy(p0, p, length);
            }

            return buffer;
        }

        public static void Copy(byte[] dst, byte[] src, int length)
        {
            Contract.Requires(dst != null && src != null);

            fixed (byte* pDst = dst, pSrc = src)
            {
                Copy(pDst, pSrc, length);
            }
        }

        public static void Copy(byte[] dst, byte[] src, int dstOffset, int srcOffset, int length)
        {
            Contract.Requires(dst != null && src != null);

            fixed (byte* pDst = dst, pSrc = src)
            {
                Copy(pDst + dstOffset, pSrc + srcOffset, length);
            }
        }

        public static void Copy(byte* pDst, byte[] src, int length)
        {
            Contract.Requires(src != null);

            fixed (byte* pSrc = src)
            {
                Copy(pDst, pSrc, length);
            }
        }

        public static void Copy(byte[] dst, byte* pDesc, int length)
        {
            Contract.Requires(dst != null);

            fixed (byte* pDst = dst)
            {
                Copy(pDst, pDesc, length);
            }
        }

        public static void Copy(byte* pDst, byte* pSrc, int length)
        {
            int* p1 = (int*)pDst, p2 = (int*)pSrc;
            if (length >= 16)
            {
                for (int k = 0, len = (length >> 4); k < len; k++)
                {
                    *p1++ = *p2++;
                    *p1++ = *p2++;
                    *p1++ = *p2++;
                    *p1++ = *p2++;
                }

                length &= 0x0F;
            }

            if (length >= 4)
            {
                for (int k = 0, len = (length >> 2); k < len; k++)
                {
                    *p1++ = *p2++;
                }

                length &= 0x03;
            }

            if (length > 0)
            {
                pDst = (byte*)p1;
                pSrc = (byte*)p2;

                for (int k = 0; k < length; k++)
                {
                    *pDst++ = *pSrc++;
                }
            }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetString(byte* p, int length, Encoding encoding)
        {
            return encoding.GetString(ToBytes(p, length));
        }

        /// <summary>
        /// 向前返回到指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="count"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool BackSkipWhiteSpace(ref byte* p, int count)
        {
            byte* p0 = p - count;
            while (p > p0)
            {
                if (IsWhiteSpace(*p))
                {
                    p--;
                    continue;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否为空白字符
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsWhiteSpace(byte b)
        {
            for (int k = 0, len = _whiteSpaces.Length; k < len; k++)
            {
                if (b == _whiteSpaces[k])
                    return true;
            }

            return false;
        }

        public static byte[] ToBytes(this uint value)
        {
            return new byte[] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) };
        }

        public static byte[] ToBytes(this int value)
        {
            return ToBytes((uint)value);
        }

        public static byte[] ToBytes(this short value)
        {
            return new byte[] { (byte)value, (byte)(value >> 8) };
        }

        public static byte[] ToBytes(this ushort value)
        {
            return ToBytes((short)value);
        }

        public static byte[] ToBytes(this byte value)
        {
            return new byte[] { value };
        }

        public static byte[] ToBytes(this sbyte value)
        {
            return ToBytes((byte)value);
        }

        public static byte[] ToBytes(this bool value)
        {
            return ToBytes(value ? (byte)1 : (byte)0);
        }

        public static byte[] ToBytes(this DateTime dateTime)
        {
            return ToBytes(dateTime.Ticks);
        }

        public static byte[] ToBytes(this TimeSpan timeSpan)
        {
            return ToBytes(timeSpan.Ticks);
        }

        public static byte[] ToBytes(this long value)
        {
            return new byte[] {
                (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24),
                (byte)(value >> 32), (byte)(value >> 40), (byte)(value >> 48), (byte)(value >> 56),
            };
        }

        public static byte[] ToBytes(this ulong value)
        {
            return ToBytes((long)value);
        }

        public static byte[] ToBytes(this char value)
        {
            return new byte[] { (byte)value, (byte)(value >> 8) };
        }

        public static byte[] ToBytes(this string value)
        {
            if (value == null)
                return new byte[0];

            return Encoding.UTF8.GetBytes(value);
        }

        public static byte[] ToBytes(this double value)
        {
            return ToBytes(*(long*)(&value));
        }

        public static byte[] ToBytes(this float value)
        {
            return ToBytes(*(int*)(&value));
        }

        public static byte[] ToBytes(this decimal value)
        {
            byte* p = (byte*)&value;
            return new byte[] {
                *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++,
                *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++,
            };
        }

        public static byte[] ToBytes(this Guid value)
        {
            return value.ToByteArray();
        }

        public static byte ToUInt8(this byte[] bytes)
        {
            return ToByte(bytes);
        }

        public static byte ToUInt8(this byte[] bytes, int offset)
        {
            return ToByte(bytes, offset);
        }

        public static byte ToUInt8(this byte[] bytes, ref int offset)
        {
            byte v = ToUInt8(bytes, offset);
            offset += sizeof(byte);
            return v;
        }

        public static int ToInt32(this byte[] bytes)
        {
            Contract.Requires(bytes != null && bytes.Length >= 4);
            return bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24);
        }

        public static int ToInt32(this byte[] bytes, int offset)
        {
            Contract.Requires(bytes != null && offset + 3 < bytes.Length);
            return bytes[offset] | (bytes[offset + 1] << 8) | (bytes[offset + 2] << 16) | (bytes[offset + 3] << 24);
        }

        public static int ToInt32(this byte[] bytes, ref int offset)
        {
            int v = ToInt32(bytes, offset);
            offset += sizeof(int);
            return v;
        }

        public static uint ToUInt32(this byte[] bytes)
        {
            return (uint)ToInt32(bytes);
        }

        public static uint ToUInt32(this byte[] bytes, ref int offset)
        {
            uint v = ToUInt32(bytes, offset);
            offset += sizeof(uint);
            return v;
        }

        public static uint ToUInt32(this byte[] bytes, int offset)
        {
            return (uint)ToInt32(bytes, offset);
        }

        public static short ToInt16(this byte[] bytes)
        {
            Contract.Requires(bytes != null && bytes.Length >= 2);
            return (short)(bytes[0] | (bytes[1] << 8));
        }

        public static short ToInt16(this byte[] bytes, int offset)
        {
            Contract.Requires(bytes != null && offset + 1 < bytes.Length);
            return (short)(bytes[offset] | (bytes[offset + 1] << 8));
        }

        public static short ToInt16(this byte[] bytes, ref int offset)
        {
            short v = ToInt16(bytes, offset);
            offset += sizeof(short);
            return v;
        }

        public static ushort ToUInt16(this byte[] bytes)
        {
            return (ushort)ToInt16(bytes);
        }

        public static ushort ToUInt16(this byte[] bytes, int offset)
        {
            return (ushort)ToInt16(bytes, offset);
        }

        public static ushort ToUInt16(this byte[] bytes, ref int offset)
        {
            ushort v = ToUInt16(bytes, offset);
            offset += sizeof(ushort);
            return v;
        }

        public static long ToInt64(this byte[] bytes)
        {
            return (long)ToUInt64(bytes);
        }

        public static long ToInt64(this byte[] bytes, int offset)
        {
            return (long)ToUInt64(bytes, offset);
        }

        public static long ToInt64(this byte[] bytes, ref int offset)
        {
            long v = ToInt64(bytes, offset);
            offset += sizeof(long);
            return v;
        }

        public static ulong ToUInt64(this byte[] bytes)
        {
            Contract.Requires(bytes != null && bytes.Length >= 8);
            uint v1 = (uint)(bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24));
            uint v2 = (uint)(bytes[4] | (bytes[5] << 8) | (bytes[6] << 16) | (bytes[7] << 24));
            return v1 + (((ulong)v2) << 32);
        }

        public static ulong ToUInt64(this byte[] bytes, int offset)
        {
            Contract.Requires(bytes != null && offset + 7 < bytes.Length);
            uint v1 = (uint)(bytes[offset] | (bytes[offset + 1] << 8) | (bytes[offset + 2] << 16) | (bytes[offset + 3] << 24));
            uint v2 = (uint)(bytes[offset + 4] | (bytes[offset + 5] << 8) | (bytes[offset + 6] << 16) | (bytes[offset + 7] << 24));
            return v1 + (((ulong)v2) << 32);
        }

        public static ulong ToUInt64(this byte[] bytes, ref int offset)
        {
            ulong v = ToUInt64(bytes, offset);
            offset += sizeof(ulong);
            return v;
        }

        public static byte ToByte(this byte[] bytes)
        {
            Contract.Requires(bytes != null && bytes.Length >= 1);
            return bytes[0];
        }

        public static byte ToByte(this byte[] bytes, int offset)
        {
            Contract.Requires(bytes != null && offset < bytes.Length);
            return bytes[offset];
        }

        public static byte ToByte(this byte[] bytes, ref int offset)
        {
            byte v = ToByte(bytes, offset);
            offset += sizeof(byte);
            return v;
        }

        public static sbyte ToSByte(this byte[] bytes)
        {
            return (sbyte)ToByte(bytes);
        }

        public static sbyte ToSByte(this byte[] bytes, int offset)
        {
            return (sbyte)ToByte(bytes, offset);
        }

        public static sbyte ToSByte(this byte[] bytes, ref int offset)
        {
            sbyte v = ToSByte(bytes, offset);
            offset += sizeof(sbyte);
            return v;
        }

        public static char ToChar(this byte[] bytes)
        {
            Contract.Requires(bytes != null && bytes.Length >= 2);

            return (char)(bytes[0] | (bytes[1] << 8));
        }

        public static char ToChar(this byte[] bytes, int offset)
        {
            Contract.Requires(bytes != null && offset + 1 < bytes.Length);

            return (char)(bytes[offset] | (bytes[offset + 1] << 8));
        }

        public static char ToChar(this byte[] bytes, ref int offset)
        {
            char v = ToChar(bytes, offset);
            offset += sizeof(char);
            return v;
        }

        public static DateTime ToDateTime(this byte[] bytes)
        {
            return new DateTime(ToInt64(bytes));
        }

        public static DateTime ToDateTime(this byte[] bytes, int offset)
        {
            return new DateTime(ToInt64(bytes, offset));
        }

        public static DateTime ToDateTime(this byte[] bytes, ref int offset)
        {
            DateTime v = ToDateTime(bytes, offset);
            offset += sizeof(DateTime);
            return v;
        }

        public static TimeSpan ToTimeSpan(this byte[] bytes)
        {
            return new TimeSpan(ToInt64(bytes));
        }

        public static TimeSpan ToTimeSpan(this byte[] bytes, int offset)
        {
            return new TimeSpan(ToInt64(bytes, offset));
        }

        public static TimeSpan ToTimeSpan(this byte[] bytes, ref int offset)
        {
            TimeSpan v = ToTimeSpan(bytes, offset);
            offset += sizeof(DateTime);
            return v;
        }

        public static string ToString(this byte[] bytes)
        {
            Contract.Requires(bytes != null);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ToString(this byte[] bytes, int offset, int count)
        {
            Contract.Requires(bytes != null);
            return Encoding.UTF8.GetString(bytes, offset, count);
        }

        public static double ToDouble(this byte[] bytes)
        {
            long v = ToInt64(bytes);
            return *(double*)&v;
        }

        public static double ToDouble(this byte[] bytes, int offset)
        {
            long v = ToInt64(bytes, offset);
            return *(double*)&v;
        }

        public static double ToDouble(this byte[] bytes, ref int offset)
        {
            double v = ToDouble(bytes, offset);
            offset += sizeof(double);
            return v;
        }

        public static decimal ToDecimal(this byte[] bytes)
        {
            Contract.Requires(bytes != null && bytes.Length >= 16);
            fixed (byte* p = bytes)
            {
                return *(decimal*)p;
            }
        }

        public static decimal ToDecimal(this byte[] bytes, int offset)
        {
            Contract.Requires(bytes != null && offset + 15 <= bytes.Length);
            fixed (byte* p = bytes)
            {
                return *(decimal*)(p + offset);
            }
        }

        public static decimal ToDecimal(this byte[] bytes, ref int offset)
        {
            decimal v = ToDecimal(bytes, offset);
            offset += sizeof(decimal);
            return v;
        }

        public static float ToSingle(this byte[] bytes)
        {
            int v = ToInt32(bytes);
            return *(float*)&v;
        }

        public static float ToSingle(this byte[] bytes, int offset)
        {
            int v = ToInt32(bytes, offset);
            return *(float*)&v;
        }

        public static float ToSingle(this byte[] bytes, ref int offset)
        {
            float v = ToSingle(bytes, offset);
            offset += sizeof(float);
            return v;
        }

        public static bool ToBoolean(this byte[] bytes)
        {
            return ToUInt8(bytes) != 0;
        }

        public static bool ToBoolean(this byte[] bytes, int offset)
        {
            return ToUInt8(bytes, offset) != 0;
        }


        public static bool ToBoolean(this byte[] bytes, ref int offset)
        {
            bool v = ToBoolean(bytes, offset);
            offset += sizeof(bool);
            return v;
        }

        public static Guid ToGuid(this byte[] bytes)
        {
            return new Guid(bytes);
        }

        public static Guid ToGuid(this byte[] bytes, int offset)
        {
            Contract.Requires(bytes != null && offset + 15 < bytes.Length);

            byte[] buffer = new byte[16];
            Copy(buffer, bytes, 0, offset, 16);
            return new Guid(buffer);
        }

        public static Guid ToGuid(this byte[] bytes, ref int offset)
        {
            Guid v = ToGuid(bytes, offset);
            offset += sizeof(Guid);
            return v;
        }

        /// <summary>
        /// 转换为紧缩的32位整型字节流
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToCompressUInt32Bytes(this uint value)
        {
            byte[] buffer = new byte[5];
            int count = ToCompressUInt32Bytes(value, buffer, 0);
            if (count == buffer.Length)
                return buffer;

            byte[] newBuffer = new byte[count];
            Buffer.BlockCopy(buffer, 0, newBuffer, 0, newBuffer.Length);
            return newBuffer;
        }

        /// <summary>
        /// 转换为紧凑的32位整型字节流
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        public static int ToCompressUInt32Bytes(this uint value, byte[] bytes, int offset)
        {
            int count = 0;

            do
            {
                count++;
                byte b = (byte)(value & 0x0000007F);
                if ((value = (value >> 7)) > 0)
                    b |= 0x80;

                bytes[offset++] = b;

            } while (value > 0);

            return count;
        }

        /// <summary>
        /// 将紧凑UInt32字节流转换UInt32
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        public static uint ToCompressionUInt32(this byte[] buffer, ref int offset)
        {
            uint value = 0;

            int offsetEnd = offset + 5;
            int shift = 0;
            for (; offset < offsetEnd; offset++)
            {
                byte b = buffer[offset];
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
            }

            if (offset >= offsetEnd)
                throw new FormatException("格式错误，紧凑UInt32字节长度不允许大于5");

            return value;
        }

        /// <summary>
        /// 将紧凑的UInt32字节流转换为UInt32
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static uint ToCompressionUInt32(this byte[] buffer, int offset = 0)
        {
            return ToCompressionUInt32(buffer, ref offset);
        }

        /// <summary>
        /// 获取紧凑32位整型的所占的字节数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetCompressUInt32BufferSize(uint value)
        {
            int num = 1;
            while ((value & 0xffffff80L) != 0)
            {
                num++;
                value = value >> 7;
            }

            return num;
        }

        public static object ToData(this byte[] bytes, TypeCode typeCode, int offset)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return ToBoolean(bytes, offset);

                case TypeCode.Byte:
                    return ToByte(bytes, offset);

                case TypeCode.Char:
                    return ToChar(bytes, offset);

                case TypeCode.DateTime:
                    return ToDateTime(bytes, offset);

                case TypeCode.Decimal:
                    return ToDecimal(bytes, offset);

                case TypeCode.Double:
                    return ToDouble(bytes, offset);

                case TypeCode.Int16:
                    return ToInt16(bytes, offset);

                case TypeCode.Int32:
                    return ToInt32(bytes, offset);

                case TypeCode.Int64:
                    return ToInt64(bytes, offset);

                case TypeCode.SByte:
                    return ToSByte(bytes, offset);

                case TypeCode.Single:
                    return ToSingle(bytes, offset);

                case TypeCode.UInt16:
                    return ToUInt16(bytes, offset);

                case TypeCode.UInt32:
                    return ToUInt32(bytes, offset);

                case TypeCode.UInt64:
                    return ToUInt64(bytes, offset);

                case (TypeCode)32:
                    return ToGuid(bytes, offset);

                default:
                    throw new NotSupportedException("不支持的数据类型：" + typeCode);
            }
        }

        public static object ToData(this byte[] bytes, TypeCode typeCode, ref int offset)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return ToBoolean(bytes, ref offset);

                case TypeCode.Byte:
                    return ToByte(bytes, ref offset);

                case TypeCode.Char:
                    return ToChar(bytes, ref offset);

                case TypeCode.DateTime:
                    return ToDateTime(bytes, ref offset);

                case TypeCode.Decimal:
                    return ToDecimal(bytes, ref offset);

                case TypeCode.Double:
                    return ToDouble(bytes, ref offset);

                case TypeCode.Int16:
                    return ToInt16(bytes, ref offset);

                case TypeCode.Int32:
                    return ToInt32(bytes, ref offset);

                case TypeCode.Int64:
                    return ToInt64(bytes, ref offset);

                case TypeCode.SByte:
                    return ToSByte(bytes, ref offset);

                case TypeCode.Single:
                    return ToSingle(bytes, ref offset);

                case TypeCode.UInt16:
                    return ToUInt16(bytes, ref offset);

                case TypeCode.UInt32:
                    return ToUInt32(bytes, ref offset);

                case TypeCode.UInt64:
                    return ToUInt64(bytes, ref offset);

                case (TypeCode)32:
                    return ToGuid(bytes, ref offset);

                default:
                    throw new NotSupportedException("不支持的数据类型：" + typeCode);
            }
        }
    }
}
