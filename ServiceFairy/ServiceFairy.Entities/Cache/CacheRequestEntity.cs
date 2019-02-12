using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Cache
{
    /// <summary>
    /// 缓存请求实体基类
    /// </summary>
    [Serializable, DataContract]
    public class CacheRequestEntity : RequestEntity
    {
        /// <summary>
        /// 是否允许路由
        /// </summary>
        [DataMember]
        public bool EnableRoute { get; set; }
    }
}
