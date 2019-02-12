using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Message
{
    /// <summary>
    /// 将指定用户的已持久化的消息从消息中心提取出来重新发送－请求
    /// </summary>
    [Serializable, DataContract]
    public class Message_ResendMessage_Request : RequestEntity
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
