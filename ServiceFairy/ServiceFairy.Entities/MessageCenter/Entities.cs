using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.MessageCenter
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
        public int[] To { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public Msg[] Msgs { get; set; }
    }
}
