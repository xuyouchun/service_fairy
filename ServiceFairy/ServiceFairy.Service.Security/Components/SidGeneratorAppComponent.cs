using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities.Security;

namespace ServiceFairy.Service.Security.Components
{
    /// <summary>
    /// 安全码生成器
    /// </summary>
    [AppComponent("安全码生成器", "生成并验证安全码")]
    unsafe class SidGeneratorAppComponent : AppComponent
    {
        public SidGeneratorAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 创建安全码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="securityLevel">安全级别</param>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public Sid CreateSid(int userId, SecurityLevel securityLevel, SidProperty property = SidProperty.Default)
        {
            SidX sid = SidX.New(userId, securityLevel, property);
            sid.VerifyCode = _GetVerifyCode(sid);

            return _Encrypt(sid).ToSid();
        }

        /// <summary>
        /// 加密安全码
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <param name="throwError">是否在解析错误时抛出异常</param>
        /// <returns>加密之后的安全码</returns>
        private SidX _Encrypt(SidX sid)
        {
            var cryptoSp = _service.CryptoExecutor.GetCryptoServiceProvider(KEY);
            byte* p = (byte*)&sid;
            cryptoSp.Encrypt(p + 1, sizeof(Sid) - 1, "");

            return sid;
        }

        // 解密安全码
        private SidX _Decrypt(SidX sid, bool throwError = false)
        {
            var cryptoSp = _service.CryptoExecutor.GetCryptoServiceProvider(KEY);
            byte* p = (byte*)&sid;
            cryptoSp.Encrypt(p + 1, sizeof(Sid) - 1, "");

            if (_GetVerifyCode(sid) != sid.VerifyCode)
            {
                if (throwError)
                    throw Utility.CreateException(SecurityStatusCode.VerifyCodeError);

                goto _error;
            }

            return sid;

        _error:
            return SidX.Empty;
        }

        /// <summary>
        /// 解密安全码
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <param name="throwError">是否在出现错误时抛出异常</param>
        /// <returns></returns>
        public SidX Decrypt(Sid sid, bool throwError = false)
        {
            SidX sidx = SidX.FromSid(sid);
            return _Decrypt(sidx, throwError);
        }

        private const string KEY = @"123";

        // 将前三个整型转换为字节数组
        private static byte[] _ThreeIntsToBytes(SidX sid)
        {
            byte* p = (byte*)&sid;
            return new byte[] { *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p };  // 12 bytes
        }

        private static int _GetVerifyCode(SidX sid)
        {
            byte[] bytes = SecurityUtility.Md5(_ThreeIntsToBytes(sid));

            int value = 0;
            fixed (byte* pFixed = bytes)
            {
                byte* p = pFixed, pEnd = p + bytes.Length;
                while (p + 4 <= pEnd)
                {
                    value ^= *(int*)p;
                    p += 4;
                }

                byte* p0 = (byte*)&value;
                while (p < pEnd)
                {
                    *p0++ ^= *p++;
                }
            }

            return value;
        }
    }
}
