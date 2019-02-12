using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 新用户注册消息
    /// </summary>
    [Serializable, DataContract, Message("Sys.User/NewUser", "新用户注册"), Remarks("新用户注册时会发出该消息通知所有的在线粉丝，不在线的粉丝上线时也会收到该消息。")]
    public class User_NewUser_Message : MessageEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds)]
        public int[] UserIds { get; set; }
    }

    /// <summary>
    /// 用户自定义的消息
    /// </summary>
    [Serializable, DataContract, Message("Sys.User/Message", "用户自定义消息"), Remarks("消息的内容为字符串，格式自定义，接收者需要知道消息的格式。")]
    public class User_Message_Message : MessageEntity
    {
        /// <summary>
        /// 来源
        /// </summary>
        [DataMember, Summary("来源"), SysFieldDoc(SysField.UserId)]
        public int From { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserMessageContent)]
        public string Content { get; set; }
    }

    /// <summary>
    /// 状态变化消息
    /// </summary>
    [Serializable, DataContract, Message("Sys.User/StatusChanged", "用户状态变化"), Remarks("用户在线状态及个性签名发生变化，会通知所有的在线粉丝，不在线的粉丝待其上线时也会接收到该消息")]
    public class User_StatusChanged_Message : MessageEntity
    {
        /// <summary>
        /// 用户状态
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds)]
        public int[] UserIds { get; set; }

        public static User_StatusChanged_Message Create(int userId)
        {
            return Create(new[] { userId });
        }

        public static User_StatusChanged_Message Create(int[] userIds)
        {
            Contract.Requires(userIds != null);
            return new User_StatusChanged_Message { UserIds = userIds };
        }
    }

    /// <summary>
    /// 用户信息变化消息
    /// </summary>
    [Serializable, DataContract, Message("Sys.User/InfoChanged", "用户信息变化"), Remarks("用户的信息变化时会通知所有的在线粉丝，不在线粉丝在上线时也会接收到该消息")]
    public class User_InfoChanged_Message : MessageEntity
    {
        /// <summary>
        /// 发生变化的用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds)]
        public int[] UserIds { get; set; }
    }
}
