using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Cache
{
    /// <summary>
    /// 获取缓存键－请求
    /// </summary>
    [Serializable, DataContract]
    public class Cache_GetKeys_Request : CacheRequestEntity
    {
        /// <summary>
        /// 键
        /// </summary>
        [DataMember]
        public string[] Keys { get; set; }
    }

    /// <summary>
    /// 获取缓存键－应答
    /// </summary>
    [Serializable, DataContract]
    public class Cache_GetKeys_Reply : ReplyEntity
    {
        /// <summary>
        /// 键
        /// </summary>
        public string[] Keys { get; set; }
    }
}
