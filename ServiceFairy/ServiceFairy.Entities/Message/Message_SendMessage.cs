using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Message
{
    /// <summary>
    /// 发送消息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Message_SendMessage_Request : RequestEntity
    {
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public Msg Message { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        [DataMember]
        public Users ToUsers { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Message, "Message");
        }
    }
}
