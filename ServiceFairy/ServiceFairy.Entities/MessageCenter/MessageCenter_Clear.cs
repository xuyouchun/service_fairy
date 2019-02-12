using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.MessageCenter
{
    /// <summary>
    /// 清空消息－请求
    /// </summary>
    [Serializable, DataContract]
    public class MessageCenter_Clear_Request : RequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserIds, "UserIds");
        }
    }
}
