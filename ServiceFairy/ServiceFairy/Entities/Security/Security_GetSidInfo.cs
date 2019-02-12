using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Security
{
    /// <summary>
    /// 获取安全码的信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Security_GetSidInfo_Request : RequestEntity
    {
        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; set; }
    }

    /// <summary>
    /// 获取安全码的信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Security_GetSidInfo_Reply : ReplyEntity
    {
        /// <summary>
        /// 信息
        /// </summary>
        [DataMember]
        public SidInfo Info { get; set; }
    }
}
