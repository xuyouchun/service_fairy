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
    public class User_ModifyPassword_Request : UserRequestEntity
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [DataMember, Summary("原密码")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [DataMember, Summary("新密码"), NewPassword]
        public string NewPassword { get; set; }
    }
}
