using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 获取插件的信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAddinsInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 插件信息
        /// </summary>
        [DataMember]
        public AppServiceAddinInfoItem[] AddinInfos { get; set; }
    }

    /// <summary>
    /// 插件信息
    /// </summary>
    [Serializable, DataContract]
    public class AppServiceAddinInfoItem
    {
        /// <summary>
        /// 信息
        /// </summary>
        [DataMember]
        public AppServiceAddinInfo Info { get; set; }

        /// <summary>
        /// 插件类型
        /// </summary>
        [DataMember]
        public AppServiceAddinType AddinType { get; set; }

        /// <summary>
        /// 源
        /// </summary>
        [DataMember]
        public ServiceEndPoint Source { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        [DataMember]
        public ServiceDesc Target { get; set; }
    }

    /// <summary>
    /// 插件类型
    /// </summary>
    public enum AppServiceAddinType
    {
        /// <summary>
        /// 接入
        /// </summary>
        [Desc("接入")]
        In,

        /// <summary>
        /// 接出
        /// </summary>
        [Desc("接出")]
        Out,
    }
}
