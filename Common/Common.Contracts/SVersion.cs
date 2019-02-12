using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using Common.Utility;

namespace Common.Contracts
{
    /// <summary>
    /// 版本号
    /// </summary>
    [Serializable, DataContract, VersionJsonSerializerAttribute]
    public struct SVersion : IComparable<SVersion>, IComparable, IEqualityComparer<SVersion>
    {
        public SVersion(byte major, byte minor, byte build, byte revision)
        {
            _value = (uint)((major << 24) | (minor << 16) | (build << 8) | revision);
        }

        public SVersion(uint value)
        {
            _value = value;
        }

        public SVersion(SerializationInfo info, StreamingContext context)
        {
            _value = info.GetUInt32("v");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("v", _value);
        }

        private uint _value;
        [DataMember]
        private uint Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 主版本号
        /// </summary>
        [IgnoreDataMember]
        public byte Major { get { return (byte)(_value >> 24); } }

        /// <summary>
        /// 子版本号
        /// </summary>
        [IgnoreDataMember]
        public byte Minor { get { return (byte)(_value >> 16); } }

        /// <summary>
        /// 编译版本号
        /// </summary>
        [IgnoreDataMember]
        public byte Build { get { return (byte)(_value >> 8); } }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        public byte Revision { get { return (byte)_value; } }

        public override string ToString()
        {
            if (IsEmpty)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(Major)
                .Append(".").Append(Minor)
                .Append(".").Append(Build)
                .Append(".").Append(Revision.ToString());

            return sb.ToString();
        }

        public string ToString(int partCount)
        {
            Contract.Requires(partCount > 0 && partCount <= 4);
            StringBuilder sb = new StringBuilder();
            sb.Append(Major);

            if (partCount > 1)
                sb.Append(".").Append(Minor);

            if (partCount > 2)
                sb.Append(".").Append(Build);

            if (partCount > 3)
                sb.Append(".").Append(Revision);

            return sb.ToString();
        }

        public static bool TryParse(string s, out SVersion version)
        {
            if (string.IsNullOrEmpty(s))
            {
                version = Empty;
                return true;
            }

            version = default(SVersion);
            string[] parts;
            if (string.IsNullOrEmpty(s = (s ?? string.Empty).Trim()) || (parts = s.Split('.')).Length > 4)
                return false;

            uint value = 0;
            for (int k = 0; k < parts.Length; k++)
            {
                byte v;
                if (!byte.TryParse(parts[k], out v))
                    return false;

                value |= (uint)(v << ((4 - k - 1) << 3));
            }

            version = new SVersion(value);
            return true;
        }

        public static SVersion Parse(string s)
        {
            SVersion v;
            if (!TryParse(s, out v))
                throw new FormatException("版本号格式不正确:" + s);

            return v;
        }

        public override int GetHashCode()
        {
            return (int)_value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(SVersion))
                return false;

            SVersion v = (SVersion)obj;
            return v._value == _value;
        }

        public static bool operator ==(SVersion vd1, SVersion vd2)
        {
            return object.Equals(vd1, vd2);
        }

        public static bool operator !=(SVersion vd1, SVersion vd2)
        {
            return !object.Equals(vd1, vd2);
        }

        public static bool operator >(SVersion vd1, SVersion vd2)
        {
            return vd1.CompareTo(vd2) > 0;
        }

        public static bool operator <(SVersion vd1, SVersion vd2)
        {
            return vd1.CompareTo(vd2) < 0;
        }

        public static bool operator >=(SVersion vd1, SVersion vd2)
        {
            return vd1.CompareTo(vd2) >= 0;
        }

        public static bool operator <=(SVersion vd1, SVersion vd2)
        {
            return vd1.CompareTo(vd2) <= 0;
        }

        public static explicit operator uint(SVersion version)
        {
            return version._value;
        }

        [IgnoreDataMember]
        public bool IsEmpty
        {
            get { return _value == 0; }
        }

        public static readonly SVersion Empty = new SVersion();

        public int CompareTo(SVersion other)
        {
            return _value.CompareTo(other._value);
        }

        public int CompareTo(object obj)
        {
            if (obj == null || obj.GetType() != typeof(SVersion))
                throw new FormatException("比较的对象必须为SVersion类型");

            return CompareTo((SVersion)obj);
        }

        public static implicit operator SVersion(string version)
        {
            return SVersion.Parse(version);
        }

        public static implicit operator string(SVersion version)
        {
            return version.IsEmpty ? string.Empty : version.ToString();
        }

        public static readonly SVersion Version_1 = "1.0";

        bool IEqualityComparer<SVersion>.Equals(SVersion x, SVersion y)
        {
            return x == y;
        }

        int IEqualityComparer<SVersion>.GetHashCode(SVersion obj)
        {
            return obj.GetHashCode();
        }

        class VersionJsonSerializerAttribute : JsonSerializerAttributeBase
        {
            public override string ToString(object obj)
            {
                return obj.ToString();
            }

            public override object ToObject(string str)
            {
                if (string.IsNullOrEmpty(str))
                    return SVersion.Empty;

                return Parse(str);
            }
        }
    }
}
