using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 用户登录－请求
    /// </summary>
    [Serializable, DataContract, Summary("用户登录－请求")]
    public class User_Login_Request : RequestEntity
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

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(UserName, "UserName");
        }
    }

    /// <summary>
    /// 用户登录－应答
    /// </summary>
    [Serializable, DataContract, Summary("用户登录－应答")]
    public class User_Login_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }
    }
}
