using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 获取组件的所有属性－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppComponentProperties_Request : RequestEntity
    {
        /// <summary>
        /// 组件名称
        /// </summary>
        [DataMember]
        public string[] ComponentNames { get; set; }
    }

    /// <summary>
    /// 获取组件的所有属性－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppComponentProperties_Reply : ReplyEntity
    {
        /// <summary>
        /// 组件名称与属性的组合
        /// </summary>
        [DataMember]
        public ObjectPropertyGroup[] PropertyGroups { get; set; }
    }

    /// <summary>
    /// 属性组合
    /// </summary>
    [Serializable, DataContract]
    public class ObjectPropertyGroup
    {
        /// <summary>
        /// 组件
        /// </summary>
        [DataMember]
        public string ComponentName { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        [DataMember]
        public ObjectProperty[] Properties { get; set; }
    }
}
