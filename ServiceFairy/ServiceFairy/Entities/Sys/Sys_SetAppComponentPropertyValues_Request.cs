using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 设置组件属性的值－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_SetAppComponentPropertyValues_Request : RequestEntity
    {
        /// <summary>
        /// 属性值组
        /// </summary>
        [DataMember]
        public AppComponentPropertyValueGroup[] ValueGroups { get; set; }
    }
}
