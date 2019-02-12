using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Cache
{
    /// <summary>
    /// 获取缓存－请求
    /// </summary>
    [Serializable, DataContract]
    public class Cache_Get_Request : CacheRequestEntity
    {
        /// <summary>
        /// 键名
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 是否同时删除这些缓存
        /// </summary>
        [DataMember]
        public bool Remove { get; set; }
    }

    /// <summary>
    /// 获取缓存－应答
    /// </summary>
    [Serializable, DataContract]
    public class Cache_Get_Reply : ReplyEntity
    {
        /// <summary>
        /// 缓存值
        /// </summary>
        [DataMember]
        public byte[] Data { get; set; }
    }
}
