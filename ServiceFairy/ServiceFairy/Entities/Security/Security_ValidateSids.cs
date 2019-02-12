using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Security
{
    /// <summary>
    /// 批量验证安全码的有效性－请求
    /// </summary>
    [Serializable, DataContract]
    public class Security_ValidateSids_Request : RequestEntity
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
    /// 批量验证安全码的有效性－应答
    /// </summary>
    [Serializable, DataContract]
    public class Security_ValidateSids_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户ID，与传入的安全码对应，如果遇到无效的安全码，则返回0
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }
}
