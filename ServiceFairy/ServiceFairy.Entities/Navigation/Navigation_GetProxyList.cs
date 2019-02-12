using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common;
using Common.Communication.Wcf;

namespace ServiceFairy.Entities.Navigation
{
    /// <summary>
    /// 获取代理列表－请求
    /// </summary>
    [Serializable, DataContract]
    public class Navigation_GetProxyList_Request : RequestEntity
    {
        /// <summary>
        /// 最大数量
        /// </summary>
        [DataMember]
        public int MaxCount { get; set; }

        /// <summary>
        /// 所支持的通信方式
        /// </summary>
        [DataMember]
        public CommunicationType CommunicationType { get; set; }

        /// <summary>
        /// 是否支持双向通信
        /// </summary>
        [DataMember]
        public CommunicationDirection Direction { get; set; }
    }

    /// <summary>
    /// 通信方向
    /// </summary>
    [Flags]
    public enum CommunicationDirection
    {
        /// <summary>
        /// 无
        /// </summary>
        [Desc("无")]
        None = 0,

        /// <summary>
        /// 单向通信
        /// </summary>
        [Desc("单向")]
        Unidirectional = 0x01,

        /// <summary>
        /// 双向通信
        /// </summary>
        [Desc("双向")]
        Bidirectional = 0x02,

        /// <summary>
        /// 所有
        /// </summary>
        [Desc("所有")]
        All = Unidirectional | Bidirectional
    }

    /// <summary>
    /// 获取代理列表－应答
    /// </summary>
    [Serializable, DataContract]
    public class Navigation_GetProxyList_Reply : ReplyEntity
    {
        /// <summary>
        /// 通讯列表
        /// </summary>
        [DataMember]
        public CommunicationOption[] CommunicationOptions { get; set; }
    }
}
