using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 获取所有的在线用户－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetAllOnlineUsers_Request : UserCenterRequestEntity
    {

    }

    /// <summary>
    /// 获取所有的在线用户－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetAllOnlineUsers_Reply : UserCenterReplyEntity
    {
        /// <summary>
        /// 所有在线用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }
}
