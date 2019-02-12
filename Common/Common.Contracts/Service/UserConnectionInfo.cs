using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 用户连接状态
    /// </summary>
    [Serializable, DataContract]
    public class UserConnectionInfo
    {
        public UserConnectionInfo()
        {
            LastAccessTime = ConnectionTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 所在终端
        /// </summary>
        [DataMember]
        public Guid ClientId { get; set; }

        /// <summary>
        /// 用户基础信息
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        [DataMember]
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        [DataMember]
        public DateTime ConnectionTime { get; set; }
    }

    /// <summary>
    /// 用户连接断开的信息
    /// </summary>
    [Serializable, DataContract]
    public class UserDisconnectedInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 断开时所在的终端
        /// </summary>
        [DataMember]
        public Guid ClientId { get; set; }

        /// <summary>
        /// 断开时间
        /// </summary>
        [DataMember]
        public DateTime DisconnectedTime { get; set; }
    }

    /// <summary>
    /// 好友关系
    /// </summary>
    [Serializable, DataContract]
    public class Friendship
    {
        /// <summary>
        /// 全部好友ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }

        public static readonly Friendship Empty = new Friendship() { UserIds = Array<int>.Empty };
    }
}
