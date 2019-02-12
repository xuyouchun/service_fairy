using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// Addin调用－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_OnAddinCall_Request : RequestEntity
    {
        /// <summary>
        /// 插件描述
        /// </summary>
        [DataMember]
        public AddinDesc AddinDesc { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        [DataMember]
        public string Method { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [DataMember]
        public CommunicateData Argument { get; set; }
    }

    /// <summary>
    /// Addin调用－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_OnAddinCall_Reply : ReplyEntity
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [DataMember]
        public CommunicateData ReturnValue { get; set; }
    }
}
