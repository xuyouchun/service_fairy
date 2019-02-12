using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 用户注册－请求
    /// </summary>
    [Serializable, DataContract, Summary("用户注册－请求")]
    public class User_Register_Request : RequestEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserName), NewUserName]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember, SysFieldDoc(SysField.Password), NewPassword]
        public string Password { get; set; }

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
