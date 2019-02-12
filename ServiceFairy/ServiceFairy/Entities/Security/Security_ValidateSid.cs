using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Security
{
    /// <summary>
    /// 验证安全码是否有效－请求
    /// </summary>
    [Serializable, DataContract]
    public class Security_ValidateSid_Request : RequestEntity
    {
        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; set; }
    }

    /// <summary>
    /// 验证安全码是否有效－应答
    /// </summary>
    [Serializable, DataContract]
    public class Security_ValidateSid_Reply : ReplyEntity
    {
        /// <summary>
        /// 安全码所对应的用户ID，如果安全码无效，则返回0
        /// </summary>
        [DataMember]
        public int UserId { get; set; }
    }
}
