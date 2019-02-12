using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 发送消息－请求
    /// </summary>
    [Serializable, DataContract, Summary("发送消息－请求")]
    public class Group_SendMessage_Request : RequestEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupMessageContent)]
        public string Content { get; set; }
    }
}
