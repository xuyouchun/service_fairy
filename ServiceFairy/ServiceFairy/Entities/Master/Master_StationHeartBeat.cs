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
    /// Station的心跳－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_StationHeartBeat_Request : RequestEntity
    {
        /// <summary>
        /// 终端信息
        /// </summary>
        [DataMember]
        public AppClientInfo[] AppClientInfos { get; set; }

        /// <summary>
        /// 已经断开的终端
        /// </summary>
        [DataMember]
        public Guid[] DisconnectedClientIds { get; set; }
    }
}
