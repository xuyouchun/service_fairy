using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 获取用户所在的位置－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserPositions_Request : RequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }

        /// <summary>
        /// 是否允许路由
        /// </summary>
        [DataMember]
        public bool EnableRoute { get; set; }
    }

    /// <summary>
    /// 获取用户所在的位置－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserPositions_Reply : ReplyEntity
    {
        /// <summary>
        /// 所在位置
        /// </summary>
        [DataMember]
        public UserPosition[] Positions { get; set; }
    }

    /// <summary>
    /// 用户所在的位置
    /// </summary>
    [Serializable, DataContract]
    public class UserPosition
    {
        /// <summary>
        /// 终端ID
        /// </summary>
        [DataMember]
        public Guid ClientId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }
}
