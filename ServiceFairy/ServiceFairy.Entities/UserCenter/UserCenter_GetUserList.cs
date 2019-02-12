using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 获取用户列表－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserList_Request : RequestEntity
    {
        /// <summary>
        /// 起始
        /// </summary>
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public int Count { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        [DataMember]
        public string Order { get; set; }
    }

    /// <summary>
    /// 获取用户列表－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserList_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        [DataMember]
        public UserIdName[] Users { get; set; }

        /// <summary>
        /// 用户总数
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }
    }
}
