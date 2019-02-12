using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Client;
using System.Runtime.Serialization;
using ServiceFairy.Entities.Master;
using Common.Contracts;

namespace ServiceFairy.Entities.Station
{
    /// <summary>
    /// 基站服务的心跳－请求
    /// </summary>
    [Serializable, DataContract]
    public class Station_ClientHeartBeat_Request : ServiceRequestEntity
    {
        /// <summary>
        /// 客户端信息
        /// </summary>
        [DataMember]
        public AppClientInfo AppClientInfo { get; set; }

        /// <summary>
        /// 服务的Cookie集合
        /// </summary>
        [DataMember]
        public AppServiceCookieCollection Cookies { get; set; }
    }

    /// <summary>
    /// 基站服务的心跳－应答
    /// </summary>
    [Serializable, DataContract]
    public class Station_ClientHeartBeat_Reply : ReplyEntity
    {
        /// <summary>
        /// 服务部署方式的最后更新时间
        /// </summary>
        [DataMember]
        public DateTime DeployVersion { get; set; }

        /// <summary>
        /// 服务配置文件的最后更新时间
        /// </summary>
        [DataMember]
        public ConfigurationVersionPair[] ConfigurationVersionPairs { get; set; }

        /// <summary>
        /// 服务的应答Cookie
        /// </summary>
        [DataMember]
        public AppServiceCookieCollection Cookies { get; set; }
    }
}
