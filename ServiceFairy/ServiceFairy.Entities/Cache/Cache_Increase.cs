using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Cache
{
    /// <summary>
    /// 将缓存值赋予一个增量－请求
    /// </summary>
    [Serializable, DataContract]
    public class Cache_Increase_Request : CacheRequestEntity
    {
        /// <summary>
        /// 需要进行增量的缓存
        /// </summary>
        [DataMember]
        public CacheIncreaseItem[] Items { get; set; }
    }

    /// <summary>
    /// 缓存增量请求参数
    /// </summary>
    [Serializable, DataContract]
    public class CacheIncreaseItem
    {
        /// <summary>
        /// 缓存键
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 增量值
        /// </summary>
        [DataMember]
        public decimal Increament { get; set; }

        /// <summary>
        /// 是否进行溢出检查
        /// </summary>
        [DataMember]
        public bool Checked { get; set; }
    }

    /// <summary>
    /// 将缓存值赋予一个增量－应答
    /// </summary>
    [Serializable, DataContract]
    public class Cache_Increase_Reply : ReplyEntity
    {
        /// <summary>
        /// 缓存增量结果
        /// </summary>
        [DataMember]
        public CacheIncreaseResult[] Results { get; set; }
    }

    /// <summary>
    /// 缓存增量的结果
    /// </summary>
    [Serializable, DataContract]
    public class CacheIncreaseResult
    {
        /// <summary>
        /// 该值之前是否存在
        /// </summary>
        [DataMember]
        public bool Exists { get; set; }

        /// <summary>
        /// 原值
        /// </summary>
        [DataMember]
        public decimal OldValue { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        [DataMember]
        public decimal NewValue { get; set; }
    }
}
