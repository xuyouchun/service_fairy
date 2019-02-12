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
    public class Security_AcquireSids_Request : RequestEntity
    {
        /// <summary>
        /// 数据项
        /// </summary>
        [DataMember]
        public AcquireSidItem[] Items { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Items, "Items");
        }
    }

    /// <summary>
    /// 申请安全码的数据项
    /// </summary>
    [Serializable, DataContract]
    public class AcquireSidItem
    {
        /// <summary>
        /// 安全级别
        /// </summary>
        [DataMember]
        public SecurityLevel SecurityLevel { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        [DataMember]
        public int[] Ids { get; set; }
    }

    /// <summary>
    /// 申请安全码－应答
    /// </summary>
    [Serializable, DataContract]
    public class Security_AcquireSids_Reply : ReplyEntity
    {
        /// <summary>
        /// 安全码与ID的组合
        /// </summary>
        [DataMember]
        public AcquiredSidPair[] Sids { get; set; }
    }

    /// <summary>
    /// 申请到的安全码
    /// </summary>
    [Serializable, DataContract]
    public class AcquiredSidPair
    {
        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        [DataMember]
        public int Id { get; set; }
    }
}
