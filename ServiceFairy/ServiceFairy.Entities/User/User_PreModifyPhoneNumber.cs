using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 准备修改手机号码－请求
    /// </summary>
    [Serializable, DataContract, Summary("准备修改手机号码－请求")]
    public class User_PreModifyPhoneNumber_Request : UserRequestEntity
    {
        /// <summary>
        /// 原手机密码
        /// </summary>
        [DataMember, SysFieldDoc(SysField.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 新手机号
        /// </summary>
        [DataMember, Summary("新手机号"), Remarks("格式为“+86 13717674043”")]
        public string NewPhoneNumber { get; set; }
    }

    /// <summary>
    /// 准备修改手机号码－应答
    /// </summary>
    [Serializable, DataContract, Summary("准备修改手机号码－应答")]
    public class User_PreModifyPhoneNumber_Reply : ReplyEntity
    {
        /// <summary>
        /// 验证码的Hash校验码
        /// </summary>
        [DataMember]
        public string HashVerifyCode { get; set; }
    }
}
