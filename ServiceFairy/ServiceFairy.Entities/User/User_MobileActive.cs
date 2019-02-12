using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 激活手机号－请求
    /// </summary>
    [Serializable, DataContract, Summary("激活手机号－请求")]
    public class User_MobileActive_Request : RequestEntity
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [DataMember, SysFieldDoc(SysField.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [DataMember, SysFieldDoc(SysField.VerifyCode)]
        public string VerifyCode { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(PhoneNumber, "PhoneNumber");
            EntityValidate.ValidateVerifyCode(VerifyCode, "VerifyCode");
        }
    }

    /// <summary>
    /// 激活手机号－应答
    /// </summary>
    [Serializable, DataContract, Summary("激活手机号－应答")]
    public class User_MobileActive_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }
    }
}
