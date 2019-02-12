using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 密码重置－请求
    /// </summary>
    [Serializable, DataContract, Summary("密码重置－请求")]
    public class User_ResetPassword_Request : RequestEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserName), NewUserName]
        public string UserName { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [DataMember, Summary("新密码"), NewPassword]
        public string NewPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [DataMember, SysFieldDoc(SysField.VerifyCode)]
        public string VerifyCode { get; set; }

        /// <summary>
        /// 是否自动登录
        /// </summary>
        [DataMember, SysFieldDoc(SysField.AutoLogin)]
        public bool AutoLogin { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(UserName, "UserName");
            EntityValidate.ValidateVerifyCode(VerifyCode, "VerifyCode");
        }
    }
}
