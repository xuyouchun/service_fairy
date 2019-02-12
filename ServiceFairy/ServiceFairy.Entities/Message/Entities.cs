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
    /// 用户的消息
    /// </summary>
    [Serializable, DataContract]
    public class UserMsgArray
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public Users ToUsers { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public Msg[] Msgs { get; set; }

        public static UserMsgArray Create(Msg msg, Users toUsers)
        {
            Contract.Requires(msg != null && toUsers != null);

            return new UserMsgArray { Msgs = new[] { msg }, ToUsers = toUsers };
        }

        public static UserMsgArray Create(Msg[] msgs, Users toUsers)
        {
            Contract.Requires(msgs != null && toUsers != null);

            return new UserMsgArray { Msgs = msgs, ToUsers = toUsers };
        }
    }
}
