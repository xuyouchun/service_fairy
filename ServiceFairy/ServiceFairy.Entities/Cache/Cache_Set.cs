using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Cache
{
    /// <summary>
    /// 设置缓存－请求
    /// </summary>
    [Serializable, DataContract]
    public class Cache_Set_Request : CacheRequestEntity
    {
        /// <summary>
        /// 键
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public byte[] Data { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [DataMember]
        public TimeSpan Expired { get; set; }
    }
}
