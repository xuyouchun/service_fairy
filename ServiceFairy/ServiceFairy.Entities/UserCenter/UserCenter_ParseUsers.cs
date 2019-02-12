using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 将用户组解析为用户ID－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ParseUsers_Request : RequestEntity
    {
        /// <summary>
        /// 用户组
        /// </summary>
        [DataMember]
        public string Users { get; set; }
    }

    /// <summary>
    /// 将用户组解析为用户ID－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ParseUsers_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }
}
