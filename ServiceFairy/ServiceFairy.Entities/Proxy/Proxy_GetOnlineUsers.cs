using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common;

namespace ServiceFairy.Entities.Proxy
{
    /// <summary>
    /// 获取在线用户－请求
    /// </summary>
    [Serializable, DataContract]
    public class Proxy_GetOnlineUsers_Request : RequestEntity
    {
        /// <summary>
        /// 排序方式
        /// </summary>
        [DataMember]
        public GetOnlineUsersSortField SortField { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        [DataMember]
        public SortType SortType { get; set; }

        /// <summary>
        /// 开始索引
        /// </summary>
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public int Count { get; set; }
    }

    /// <summary>
    /// 排序方式
    /// </summary>
    public enum GetOnlineUsersSortField
    {
        /// <summary>
        /// 登录时间
        /// </summary>
        [Desc("连接时间")]
        ConnectionTime,
    }

    /// <summary>
    /// 在线用户信息
    /// </summary>
    [Serializable, DataContract]
    public class ProxyOnlineUserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        [DataMember]
        public DateTime ConnectionTime { get; set; }
    }

    /// <summary>
    /// 获取在线用户－应答
    /// </summary>
    [Serializable, DataContract]
    public class Proxy_GetOnlineUsers_Reply : ReplyEntity
    {
        /// <summary>
        /// 在线用户信息
        /// </summary>
        [DataMember]
        public ProxyOnlineUserInfo[] Infos { get; set; }
    }
}
