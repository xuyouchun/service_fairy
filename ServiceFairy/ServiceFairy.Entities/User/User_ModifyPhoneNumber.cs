using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 修改密码－请求
    /// </summary>
    [Serializable, DataContract, Summary("修改密码－请求")]
    public class User_ModifyPhoneNumber_Request : UserRequestEntity
    {
        /// <summary>
        /// 新手机号
        /// </summary>
        [DataMember, Summary("新手机号"), Remarks("格式为：+86 13717674043"), NewUserName]
        public string NewPhoneNumber { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [DataMember, Remarks("新密码"), NewPassword]
        public string NewPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [DataMember, SysFieldDoc(SysField.VerifyCode)]
        public string VerifyCode { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(NewPhoneNumber, "NewPhoneNumber");
            EntityValidate.ValidateVerifyCode(VerifyCode, "VerifyCode");
        }
    }
}
