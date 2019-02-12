using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Security
{
    /// <summary>
    /// 申请安全码－请求
    /// </summary>
    [Serializable, DataContract]
    public class Security_AcquireSid_Request : RequestEntity
    {
        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 安全级别
        /// </summary>
        [DataMember]
        public SecurityLevel SecurityLevel { get; set; }
    }

    /// <summary>
    /// 申请安全码－应答
    /// </summary>
    [Serializable, DataContract]
    public class Security_AcquireSid_Reply : ReplyEntity
    {
        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; set; }
    }
}
