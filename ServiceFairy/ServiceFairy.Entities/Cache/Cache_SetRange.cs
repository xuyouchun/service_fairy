using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Cache
{
    /// <summary>
    /// 批量设置缓存－请求
    /// </summary>
    [Serializable, DataContract]
    public class Cache_SetRange_Request : CacheRequestEntity
    {
        /// <summary>
        /// 需要设置的缓存项
        /// </summary>
        [DataMember]
        public CacheItem[] Items { get; set; }
    }

    /// <summary>
    /// 缓存项
    /// </summary>
    [Serializable, DataContract]
    public class CacheItem
    {
        /// <summary>
        /// 键
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 缓存的内容，以序列化的字节流形式表示
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
