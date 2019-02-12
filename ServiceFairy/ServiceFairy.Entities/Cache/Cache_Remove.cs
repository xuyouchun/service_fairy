using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Cache
{
    /// <summary>
    /// 删除缓存项－请求
    /// </summary>
    [Serializable, DataContract]
    public class Cache_Remove_Request : CacheRequestEntity
    {
        /// <summary>
        /// 需要删除的缓存键
        /// </summary>
        [DataMember]
        public string[] Keys { get; set; }
    }
}
