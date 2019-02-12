using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Message
{
    /// <summary>
    /// 获取用户的消息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Message_GetMessages_Request : RequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserIds, "UserIds");
        }
    }

    /// <summary>
    /// 获取用户的消息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Message_GetMessages_Reply : ReplyEntity
    {
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public Msg[] Msgs { get; set; }
    }
}
