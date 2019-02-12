using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Common.Contracts.Service;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 安全码的加密与解密
    /// </summary>
    unsafe static class SidUtility
    {
        private static bool _Check(Sid sid, ServiceAddress from)
        {
            return !sid.IsEmpty() && from != null && !string.IsNullOrEmpty(from.Address) && !from.IsLocalHost();
        }

        private static byte[] _GetAddressBytes(string address)
        {
            IPAddress v;
            if (IPAddress.TryParse(address, out v))
            {
                return v.GetAddressBytes();
            }

            return Encoding.UTF8.GetBytes(address);
        }

        // 加密sid
        public static Sid EncryptSid(Sid sid, ServiceAddress from)
        {
            return sid;
            /*
            if (!_Check(sid, from) || sid.Encrypted)
                return sid;

            _Xor((byte*)&sid, sizeof(Sid), _GetAddressBytes(from.Address));
            sid.Encrypted = true;
            return sid;*/
        }

        // 解密sid
        public static Sid DecryptSid(Sid sid, ServiceAddress from)
        {
            return sid;
            /*
            if (!_Check(sid, from) || !sid.Encrypted)
                return sid;

            _Xor((byte*)&sid, sizeof(Sid), _GetAddressBytes(from.Address));
            sid.Encrypted = false;
            return sid;*/
        }

        private static void _Xor(byte* pBuffer1, int length1, byte[] buffer2)
        {
            fixed (byte* p2Fixed = buffer2)
            {
                byte* p1 = pBuffer1, p1End = p1 + length1;
                byte* p2 = p2Fixed, p2End = p2 + buffer2.Length;
                while (p1 < p1End)
                {
                    *p1++ ^= *p2++;
                    if (p2 >= p2End)
                        p2 = p2Fixed;
                }
            }
        }
    }
}
