using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Security
{
    /// <summary>
    /// 修改密码－请求
    /// </summary>
    [Serializable, DataContract]
    public class Security_ModifyPassword_Request : RequestEntity
    {
        /// <summary>
        /// 新密码
        /// </summary>
        [DataMember]
        public string NewPassword { get; set; }

        /// <summary>
        /// 原密码
        /// </summary>
        [DataMember]
        public string OldPassword { get; set; }
    }
}
