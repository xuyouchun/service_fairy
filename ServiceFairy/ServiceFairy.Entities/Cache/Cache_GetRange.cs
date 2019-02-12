using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Cache
{
    /// <summary>
    /// 批量获取缓存－请求
    /// </summary>
    [Serializable, DataContract]
    public class Cache_GetRange_Request : CacheRequestEntity
    {
        /// <summary>
        /// 缓存键
        /// </summary>
        [DataMember]
        public string[] Keys { get; set; }

        /// <summary>
        /// 是否同时删除缓存
        /// </summary>
        [DataMember]
        public bool Remove { get; set; }
    }

    /// <summary>
    /// 键值对
    /// </summary>
    [Serializable, DataContract]
    public class CacheKeyValuePair
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
    }

    /// <summary>
    /// 批量获取缓存－应答
    /// </summary>
    [Serializable, DataContract]
    public class Cache_GetRange_Reply : ReplyEntity
    {
        /// <summary>
        /// 缓存项
        /// </summary>
        [DataMember]
        public CacheKeyValuePair[] Datas { get; set; }
    }
}
