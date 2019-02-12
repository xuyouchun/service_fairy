using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ServiceFairy.Entities.Message;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Addins.MesssageSubscript
{
    /// <summary>
    /// 消息通知插件－接收消息－请求
    /// </summary>
    [Serializable, DataContract]
    public class MesasgeSubscript_ApplyMessage_Request : RequestEntity
    {
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public UserMsgItem[] UserMsgs { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserMsgs, "UserMsgs");
        }
    }

    /// <summary>
    /// 消息与接收者的组合
    /// </summary>
    [Serializable, DataContract]
    public class UserMsgItem
    {
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public Msg Msg { get; set; }

        /// <summary>
        /// 消息索引号
        /// </summary>
        [DataMember]
        public long MsgIndex { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        [DataMember]
        public int[] To { get; set; }
    }

    /// <summary>
    /// 消息通知插件－接收消息－应答
    /// </summary>
    [Serializable, DataContract]
    public class MesasgeSubscript_ApplyMessage_Reply : ReplyEntity
    {
        
    }
}
