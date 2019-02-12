using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Common.Utility;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 安全码
    /// </summary>
    [Serializable, SidJsonSerializerAttribute, StructLayout(LayoutKind.Sequential), TypeName("sid")]
    public unsafe struct Sid : IComparable, IComparable<Sid>, IEquatable<Sid>
    {
        private int _v1, _v2, _v3, _v4;

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return this == Empty;
        }

        public static Sid FromBytes(byte[] bytes)
        {
            Contract.Requires(bytes != null && bytes.Length == 16);

            fixed (byte* p = bytes)
            {
                return *(Sid*)p;
            }
        }

        /// <summary>
        /// 转换为字节流
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            Sid sid = this;
            byte* p = (byte*)&sid;
            return new byte[] {
                *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++,
                *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Sid))
                return false;

            Sid sid = (Sid)obj;
            return Equals(sid);
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsEmpty())
                return "";

            byte[] bytes = this.ToBytes();
            return Convert.ToBase64String(bytes);
        }

        public override int GetHashCode()
        {
            Sid sid = this;
            int* p = (int*)&sid;

            return *p++ ^ *p++ ^ *p++ ^ *p;
        }

        /// <summary>
        /// 与指定的对象进行比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Sid))
                throw new ArgumentException("相比较的对象必须为Sid");

            return CompareTo((Sid)obj);
        }

        public int CompareTo(Sid other)
        {
            Sid sid = this;
            int* p1 = (int*)&sid, p2 = (int*)&other;

            if (*p1 != *p2)
                return (*p1).CompareTo(*p2);

            if (*++p1 != *++p2)
                return (*p1).CompareTo(*p2);

            if (*++p1 != *++p2)
                return (*p1).CompareTo(*p2);

            if (*++p1 != *++p2)
                return (*p1).CompareTo(*p2);

            return 0;
        }

        public bool Equals(Sid other)
        {
            Sid sid = this;
            int* p1 = (int*)&sid, p2 = (int*)&other;
            return *p1++ == *p2++ && *p1++ == *p2++ && *p1++ == *p2++ && *p1 == *p2;
        }

        public static bool operator ==(Sid sid1, Sid sid2)
        {
            return sid1.Equals(sid2);
        }

        public static bool operator !=(Sid sid1, Sid sid2)
        {
            return !sid1.Equals(sid2);
        }

        public static bool operator >=(Sid sid1, Sid sid2)
        {
            return sid1.CompareTo(sid2) >= 0;
        }

        public static bool operator <=(Sid sid1, Sid sid2)
        {
            return sid1.CompareTo(sid2) <= 0;
        }

        public static bool operator >(Sid sid1, Sid sid2)
        {
            return sid1.CompareTo(sid2) > 0;
        }

        public static bool operator <(Sid sid1, Sid sid2)
        {
            return sid1.CompareTo(sid2) < 0;
        }

        /// <summary>
        /// 是否已经经过加密
        /// </summary>
        [IgnoreDataMember]
        public bool Encrypted
        {
            get { return (_v1 & (int)SidProperty.Encrypted) != 0; }
            set
            {
                if (value)
                    _v1 |= (int)SidProperty.Encrypted;
                else
                    _v1 &= ~(int)SidProperty.Encrypted;
            }
        }

        /// <summary>
        /// 尝试将字符串转换为Sid对象
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="value">Sid对象</param>
        /// <returns>是否转换成功</returns>
        public static bool TryParse(string s, out Sid value)
        {
            try
            {
                value = Parse(s);
                return true;
            }
            catch
            {
                value = Empty;
                return false;
            }
        }

        /// <summary>
        /// 将字符串转换为Sid对象
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>Sid对象</returns>
        public static Sid Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return Empty;
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(s);
                if (bytes.Length != 16)
                    goto _throw;

                return FromBytes(bytes);
            }
            catch (Exception)
            {
                goto _throw;
            }

        _throw:
            throw new FormatException("错误的Sid格式");
        }

        public static readonly Sid Empty = default(Sid);

        class SidJsonSerializerAttribute : JsonSerializerAttributeBase
        {
            public override string ToString(object obj)
            {
                return ((Sid)obj).ToString();
            }

            public override object ToObject(string str)
            {
                return Sid.Parse(str);
            }
        }
    }

    /// <summary>
    /// 安全码的属性
    /// </summary>
    [Flags]
    public enum SidProperty : byte
    {
        [Desc("默认")]
        Default = 0,

        /// <summary>
        /// 是否已被加密
        /// </summary>
        [Desc("是否已被加密")]
        Encrypted = 1,

        /// <summary>
        /// 是否为系统用户
        /// </summary>
        [Desc("是否为系统用户")]
        SystemUser = 1 << 1,
    }

    /// <summary>
    /// 安全码信息
    /// </summary>
    [Serializable, DataContract]
    public class SecurityInfo
    {
        public SecurityInfo(Sid sid, int userId, SecurityLevel securityLevel)
        {
            Sid = sid;
            UserId = userId;
            SecurityLevel = securityLevel;
        }

        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; private set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; private set; }

        /// <summary>
        /// 安全级别
        /// </summary>
        [DataMember]
        public SecurityLevel SecurityLevel { get; private set; }
    }
}
