using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Navigation
{
    /// <summary>
    /// 获取代理地址－请求
    /// </summary>
    [Serializable, DataContract]
    public class Navigation_GetProxy_Request : RequestEntity
    {
        /// <summary>
        /// 通信类型
        /// </summary>
        [DataMember]
        public CommunicationType CommunicationType { get; set; }

        /// <summary>
        /// 是否支持双向通信
        /// </summary>
        [DataMember]
        public bool Duplex { get; set; }
    }

    /// <summary>
    /// 获取代理地址－应答
    /// </summary>
    [Serializable, DataContract]
    public class Navigation_GetProxy_Reply : ReplyEntity
    {
        /// <summary>
        /// 代理地址
        /// </summary>
        [DataMember]
        public string Proxy { get; set; }
    }
}
