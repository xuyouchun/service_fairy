using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Security
{
    /// <summary>
    /// 批量获取安全码的信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Security_GetSidInfos_Request : RequestEntity
    {
        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid[] Sids { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Sids, "Sids");
        }
    }

    /// <summary>
    /// 安全码信息
    /// </summary>
    [Serializable, DataContract]
    public class SidInfo
    {
        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 安全码级别
        /// </summary>
        [DataMember]
        public SecurityLevel SecurityLevel { get; set; }
    }

    /// <summary>
    /// 获取安全码的信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Security_GetSidInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 安全码信息
        /// </summary>
        [DataMember]
        public SidInfo[] Infos { get; set; }
    }
}
