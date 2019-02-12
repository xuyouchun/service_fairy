using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Security.Components
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct SidX
    {
        private SidX(int id, SecurityLevel securityLevel, SidProperty property, ushort random, uint secondsFrom2012, int verifyCode)
        {
            _property = (byte)property;
            _securityLevel = (byte)securityLevel;
            _random = random;
            _userId = id;
            _secondsFrom2012 = secondsFrom2012;
            _verifyCode = verifyCode;
        }

        /// <summary>
        /// 创建新的安全码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="securityLevel">安全级别</param>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static SidX New(int id, SecurityLevel securityLevel, SidProperty property = SidProperty.Default)
        {
            ushort random = (ushort)_r.Next(ushort.MaxValue);
            uint secondsFrom2012 = checked((uint)(DateTime.UtcNow.Ticks / 10000000 - _secondsOf2012));

            return new SidX(id, securityLevel, property, random, secondsFrom2012, 0);
        }

        public static SidX FromSid(Sid sid)
        {
            return *(SidX*)&sid;
        }

        public Sid ToSid()
        {
            SidX v = this;
            return *(Sid*)&v;
        }

        public static SidX FromBytes(byte[] bytes)
        {
            Contract.Requires(bytes != null && bytes.Length == 16);

            fixed (byte* p = bytes)
            {
                return *(SidX*)p;
            }
        }

        private byte _property;
        private byte _securityLevel;
        private ushort _random;
        private int _userId;
        private uint _secondsFrom2012;
        private int _verifyCode;

        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        [IgnoreDataMember]
        public DateTime CreateTime
        {
            get { return new DateTime((_secondsFrom2012 + _secondsOf2012) * 10000000); }
            set { _secondsFrom2012 = checked((uint)(value.Ticks / 10000000 - _secondsOf2012)); }
        }

        /// <summary>
        /// 属性
        /// </summary>
        [IgnoreDataMember]
        public SidProperty Property
        {
            get { return (SidProperty)_property; }
            set { _property = (byte)value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [IgnoreDataMember]
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 随机值
        /// </summary>
        [IgnoreDataMember]
        public ushort Random
        {
            get { return _random; }
            set { _random = value; }
        }

        /// <summary>
        /// 校验码
        /// </summary>
        [IgnoreDataMember]
        public int VerifyCode
        {
            get { return _verifyCode; }
            set { _verifyCode = value; }
        }

        /// <summary>
        /// 安全级别
        /// </summary>
        [IgnoreDataMember]
        public SecurityLevel SecurityLevel
        {
            get { return (SecurityLevel)_securityLevel; }
            set { _securityLevel = (byte)value; }
        }

        /// <summary>
        /// 是否已经加密
        /// </summary>
        [IgnoreDataMember]
        public bool IsEncrypted
        {
            get { return _GetProperty(SidProperty.Encrypted); }
            set { _SetProperty(SidProperty.Encrypted, value); }
        }

        /// <summary>
        /// 是否为系统用户
        /// </summary>
        [IgnoreDataMember]
        public bool IsSystemUser
        {
            get { return _GetProperty(SidProperty.SystemUser); }
            set { _SetProperty(SidProperty.SystemUser, value); }
        }

        private void _SetProperty(SidProperty prop, bool value)
        {
            if (value)
                _property |= (byte)prop;
            else
                _property &= (byte)~prop;
        }

        private bool _GetProperty(SidProperty prop)
        {
            return (_property & (byte)prop) != 0;
        }

        /// <summary>
        /// 转换为字节流
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            SidX sid = this;
            byte* p = (byte*)&sid;
            return new byte[] {
                *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++,
                *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p
            };
        }

        private static readonly long _secondsOf2012 = new DateTime(2012, 1, 1, 0, 0, 0).Ticks / 10000000;
        private static readonly Random _r = new Random();

        public static readonly SidX Empty = default(SidX);

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return this.ToSid() == Sid.Empty;
        }

        private static uint _NextUInt()
        {
            return (uint)_r.Next(int.MinValue, int.MaxValue);
        }
    }
}
