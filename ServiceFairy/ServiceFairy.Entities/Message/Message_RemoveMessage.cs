using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Message
{
    /// <summary>
    /// 删除消息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Message_RemoveMessage_Request : RequestEntity
    {
        /// <summary>
        /// 消息索引号
        /// </summary>
        [DataMember]
        public long MsgIndex { get; set; }
    }
}
