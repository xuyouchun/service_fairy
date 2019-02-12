using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using ServiceFairy.Client;
using Common.Communication.Wcf;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// Init请求参数
    /// </summary>
    [Serializable, DataContract]
    public class Master_InitClient_Request : ServiceRequestEntity
    {
        /// <summary>
        /// 终端信息
        /// </summary>
        [DataMember]
        public AppClientInfo AppClientInfo { get; set; }

        /// <summary>
        /// 建议的初始服务
        /// </summary>
        [DataMember]
        public ServiceDesc[] InitServices { get; set; }

        /// <summary>
        /// 建议的初始通信方式
        /// </summary>
        [DataMember]
        public CommunicationOption[] InitCommunicationOptions { get; set; }
    }

    /// <summary>
    /// Init应答参数
    /// </summary>
    [Serializable, DataContract]
    public class Master_InitClient_Reply : ReplyEntity
    {
        /// <summary>
        /// Master终端的唯一标识
        /// </summary>
        [DataMember]
        public Guid MasterClientID { get; set; }

        /// <summary>
        /// 一些系统服务的地址，主要是System.Station服务与System.Deploy服务
        /// </summary>
        [DataMember]
        public AppInvokeInfo[] InvokeInfos { get; set; }
    }
}
