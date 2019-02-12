using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common;
using Common.Utility;

namespace ServiceFairy.Entities.MessageCenter
{
    /// <summary>
    /// 读取持久化的消息－请求
    /// </summary>
    [Serializable, DataContract]
    public class MessageCenter_Read_Request : RequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 读取持久化的消息
    /// </summary>
    [Serializable, DataContract]
    public class MessageCenter_Read_Reply : ReplyEntity
    {
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public Msg[] Msgs { get; set; }
    }
}
