using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Communication.Wcf;
using Common.Contracts;

namespace ServiceFairy.Entities
{
    [Serializable, DataContract]
    public class AppClientAdjustInfo
    {
        /// <summary>
        /// 服务终端唯一标识
        /// </summary>
        [DataMember]
        public Guid ClientID { get; set; }

        /// <summary>
        /// 需要开启的服务
        /// </summary>
        [DataMember]
        public ServiceDesc[] ServicesToStart { get; set; }

        /// <summary>
        /// 需要关闭的服务
        /// </summary>
        [DataMember]
        public ServiceDesc[] ServicesToStop { get; set; }

        /// <summary>
        /// 需要开启的信道
        /// </summary>
        [DataMember]
        public CommunicationOption[] CommunicationsToOpen { get; set; }

        /// <summary>
        /// 需要关闭的信道
        /// </summary>
        [DataMember]
        public ServiceAddress[] CommunicationsToClose { get; set; }
    }
}
