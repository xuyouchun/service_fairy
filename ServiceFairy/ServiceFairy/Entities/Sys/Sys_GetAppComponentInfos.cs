using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 获取所有组件信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppComponentInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 组件信息
        /// </summary>
        [DataMember]
        public AppComponentInfo[] AppComponentInfos { get; set; }
    }
}
