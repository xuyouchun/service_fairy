using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ServiceFairy.Entities.Message;

namespace ServiceFairy.Entities.MessageCenter
{
    /// <summary>
    /// 将消息批量持久化存储－请求
    /// </summary>
    [Serializable, DataContract]
    public class MessageCenter_Save_Request : RequestEntity
    {
        /// <summary>
        /// 消息保存项数组
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
