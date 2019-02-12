using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Utility;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 获取属性值－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppComponentPropertyValues_Request : RequestEntity
    {
        /// <summary>
        /// 属性名称，如果为空则获取全部属性值
        /// </summary>
        [DataMember]
        public string[] ComponentNames { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        [DataMember]
        public string[] Expressions { get; set; }
    }

    /// <summary>
    /// 获取属性值－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppComponentPropertyValues_Reply : ReplyEntity
    {
        /// <summary>
        /// 属性值组
        /// </summary>
        [DataMember]
        public AppComponentPropertyValueGroup[] ValueGroups { get; set; }
    }

    /// <summary>
    /// 名称与组件属性值的组合
    /// </summary>
    [Serializable, DataContract]
    public class AppComponentPropertyValueGroup
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string ComponentName { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        [DataMember]
        public string Expression { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public AppComponentPropertyValue[] Values { get; set; }
    }

    /// <summary>
    /// 组件的属性值
    /// </summary>
    [Serializable, DataContract]
    public class AppComponentPropertyValue
    {
        public AppComponentPropertyValue(string name, string value)
        {
            Contract.Requires(name != null);

            Name = name;
            Value = value;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public string Value { get; private set; }

        public static AppComponentPropertyValue From(ObjectPropertyValue value)
        {
            Contract.Requires(value != null);

            return new AppComponentPropertyValue(value.Name, value.ToStringIgnoreNull());
        }
    }
}
