using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common;
using Common.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 服务器的信息
    /// </summary>
    [Serializable, DataContract]
    public class AppInvokeInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppInvokeInfo()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="serviceDescs"></param>
        /// <param name="communicateOptions"></param>
        public AppInvokeInfo(Guid clientId, ServiceDesc[] serviceDescs, CommunicationOption[] communicateOptions)
        {
            ClientID = clientId;
            ServiceDescs = serviceDescs ?? Array<ServiceDesc>.Empty;
            CommunicateOptions = communicateOptions ?? Array<CommunicationOption>.Empty;
        }

        /// <summary>
        /// 服务器端唯一标识
        /// </summary>
        [DataMember]
        public Guid ClientID { get; set; }

        /// <summary>
        /// 运行在该服务器上的所有服务
        /// </summary>
        [DataMember]
        public ServiceDesc[] ServiceDescs { get; set; }

        /// <summary>
        /// 该服务器上的所有通信方式
        /// </summary>
        [DataMember]
        public CommunicationOption[] CommunicateOptions { get; set; }

        public override string ToString()
        {
            return string.Format("ServerID:{0} Services:{1} Communications:{2}", ClientID,
                string.Join(",", (object[])ServiceDescs.ToArray(si => si)), string.Join(",", (object[])CommunicateOptions));
        }
    }
}
