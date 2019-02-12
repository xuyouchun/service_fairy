using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 用户接口的状态
    /// </summary>
    public enum UserStatusCode
    {
        [Desc("Error")]
        Error = SFStatusCodes.User,

        [Desc("用户不存在")]
        InvalidUser = Error | 1,

        [Desc("密码错误")]
        InvalidPassword = Error | 2,

        [Desc("用户已经存在")]
        UserAlreadyExists = Error | 3,

        [Desc("尝试登录的次数过多")]
        TryTooManyTimes = Error | 4,

        [Desc("验证码不正确")]
        InvalidVerifyCode = Error | 20,
    }
}
