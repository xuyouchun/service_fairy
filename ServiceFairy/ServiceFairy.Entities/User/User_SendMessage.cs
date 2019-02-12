using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 发送消息－请求
    /// </summary>
    [Serializable, DataContract, Summary("发送消息－请求")]
    public class User_SendMessage_Request : RequestEntity
    {
        /// <summary>
        /// 接收者
        /// </summary>
        [DataMember, SysFieldDoc(SysField.Users)]
        public string To { get; set; }
        
        /// <summary>
        /// 消息内容
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserMessageContent)]
        public string Content { get; set; }

        /// <summary>
        /// 消息属性
        /// </summary>
        [DataMember, SysFieldDoc(SysField.MessageProperty)]
        public MsgProperty Property { get; set; }
    }
}
