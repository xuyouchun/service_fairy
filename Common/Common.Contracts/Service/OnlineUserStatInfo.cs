using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 在线用户统计数据
    /// </summary>
    [Serializable, DataContract]
    public class OnlineUserStatInfo
    {
        /// <summary>
        /// 当前在线用户数量
        /// </summary>
        [DataMember]
        public int CurrentOnlineUserCount { get; set; }

        /// <summary>
        /// 最大在线用户数量
        /// </summary>
        [DataMember]
        public int MaxOnlineUserCount { get; set; }

        /// <summary>
        /// 最大在线用户发生的时间
        /// </summary>
        [DataMember]
        public DateTime MaxOnlineUserCountTime { get; set; }
    }
}
