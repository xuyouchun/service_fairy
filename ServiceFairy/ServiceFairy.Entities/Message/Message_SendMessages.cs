using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Entities.Message
{
    /// <summary>
    /// 批量发送消息
    /// </summary>
    [Serializable, DataContract]
    public class Message_SendMessages_Request : RequestEntity
    {
        /// <summary>
        /// 消息组合
        /// </summary>
        [DataMember]
        public UserMsgArray[] MsgArrs { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(MsgArrs, "MsgArrs");
        }
    }
}
