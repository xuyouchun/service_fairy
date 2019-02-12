using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Client;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 获取客户端列表－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetClientList_Request : RequestEntity
    {
        /// <summary>
        /// 是否包含详细信息
        /// </summary>
        [DataMember]
        public bool IncludeDetail { get; set; }

        /// <summary>
        /// 需要获取的客户端列表ID，如果为空则返回全部列表
        /// </summary>
        [DataMember]
        public Guid[] ClientIds { get; set; }
    }

    /// <summary>
    /// 获取客户端列表－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetClientList_Reply : ReplyEntity
    {
        /// <summary>
        /// 客户端列表
        /// </summary>
        [DataMember]
        public AppClientInfo[] AppClientInfos { get; set; }
    }
}
