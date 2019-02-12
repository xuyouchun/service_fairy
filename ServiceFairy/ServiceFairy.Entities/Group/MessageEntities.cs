using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;
using Common;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 用户被加入到群组中
    /// </summary>
    [Serializable, DataContract, Message("Sys.Group/GroupChanged", "群组信息或成员发生变化")]
    [Remarks("当群组信息变化或成员变化时会群组内的成员发送该消息，其中群组的创建者不会接收到该消息。")]
    public class Group_GroupChanged_Message : MessageEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        [DataMember, Summary("群组信息或成员变化的类型")]
        [Remarks("客户端根据该数据来判断信息变化的类型")]
        public GroupChangedType ChangedType { get; set; }

        /// <summary>
        /// 相关人或群组名称
        /// </summary>
        [DataMember, Summary("相关人或群组名称")]
        [Remarks(@"如果是群组成员发生变化，该名称为该成员的名称，如果是群组信息发生变化，该名称为群组名称，如果名称有多个，中间用逗号隔开。
该名称在显示提示信息时使用。")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 群组信息或成员发生变化
    /// </summary>
    public enum GroupChangedType
    {
        /// <summary>
        /// 当前用户被加入到群组中
        /// </summary>
        [Desc("当前用户被加入到群组中")]
        CurrentUserAdded,

        /// <summary>
        /// 其它用户被加入到群组中
        /// </summary>
        [Desc("其它用户被加入到群组中")]
        MemberAdded,

        /// <summary>
        /// 当前用户被从群组中移除
        /// </summary>
        [Desc("当前用户被从群组中移除")]
        CurrentUserRemoved,

        /// <summary>
        /// 其它用户被从群组中移除
        /// </summary>
        [Desc("其它用户被从群组中移除")]
        MemberRemoved,

        /// <summary>
        /// 用户退出群组
        /// </summary>
        [Desc("用户退出群组")]
        ExitGroup,

        /// <summary>
        /// 群组被解散
        /// </summary>
        [Desc("群组被解散")]
        GroupRemoved,

        /// <summary>
        /// 群组信息变化
        /// </summary>
        [Desc("群组信息变化")]
        InfoChanged,
    }

    /// <summary>
    /// 群组会话消息
    /// </summary>
    [Serializable, DataContract, Message("Sys.Group/Message", "群组会话消息"), Remarks("当群组成员调用SendMessage接口时，群组中的其它成员将会收到该消息")]
    public class Group_Message_Message : MessageEntity
    {
        /// <summary>
        /// 发送者
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId), Summary("发送者")]
        public int Sender { get; set; }

        /// <summary>
        /// 组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupMessageContent)]
        public string Content { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [DataMember, SysFieldDoc(SysField.Time), Summary("发送时间")]
        public DateTime Time { get; set; }
    }
}
