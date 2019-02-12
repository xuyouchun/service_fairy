using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 环境变量
    /// </summary>
    [Serializable, DataContract]
    public class EnvironmentValue
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// 环境变量的的集合
    /// </summary>
    [Serializable, DataContract]
    public class EnvironmentValues
    {
        /// <summary>
        /// 环境变量
        /// </summary>
        [DataMember]
        public EnvironmentValue[] Values { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime LastUpdate { get; set; }
    }
}
