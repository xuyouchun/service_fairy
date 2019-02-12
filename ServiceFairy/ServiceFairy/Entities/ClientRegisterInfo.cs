using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Communication.Wcf;
using Common.Contracts.Service;

namespace ServiceFairy
{
    /// <summary>
    /// 终端的注册信息
    /// </summary>
    [Serializable, DataContract]
    public class ClientRegisterInfo
    {
        /// <summary>
        /// 客户端唯一标识
        /// </summary>
        [DataMember]
        public Guid ClientID { get; set; }

        /// <summary>
        /// 所有的服务
        /// </summary>
        [DataMember]
        public ServiceInfo[] ServiceInfos { get; set; }

        /// <summary>
        /// 所有的通信方式
        /// </summary>
        [DataMember]
        public CommunicationOption[] CommunicateOption { get; set; }

    }
}
