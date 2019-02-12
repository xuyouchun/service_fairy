using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ServiceFairy.Entities.Security
{
    /// <summary>
    /// 安全服务状态码
    /// </summary>
    public enum SecurityStatusCode
    {
        [Desc("Error")]
        Error = SFStatusCodes.Security,

        /// <summary>
        /// 用户不存在
        /// </summary>
        [Desc("用户不存在")]
        InvalidUser = Error | 1,

        /// <summary>
        /// 密码错误
        /// </summary>
        [Desc("密码错误")]
        InvalidPassword = Error | 2,

        /// <summary>
        /// 安全码无效
        /// </summary>
        [Desc("安全码无效")]
        InvalidSid = Error | 3,

        /// <summary>
        /// 安全码为空
        /// </summary>
        [Desc("安全码为空")]
        EmptySid = Error | 4,

        /// <summary>
        /// 不允许申请比调用者权限更高的安全级别的安全码
        /// </summary>
        [Desc("不允许申请比调用者权限更高的安全级别的安全码")]
        NoSecurityWhenAcquireSid = Error | 5,

        /// <summary>
        /// 安全码已经被加密，不允许再次加密
        /// </summary>
        [Desc("安全码已经被加密，不允许再次加密")]
        SidAlreadEncrypted = Error | 6,

        /// <summary>
        /// 安全码未加密，不能执行解密
        /// </summary>
        [Desc("安全码未加密，不能执行解密")]
        SidNotEncrypted = Error | 7,

        /// <summary>
        /// 安全码校验错误
        /// </summary>
        [Desc("安全码校验错误")]
        VerifyCodeError = Error | 8,
    }
}
