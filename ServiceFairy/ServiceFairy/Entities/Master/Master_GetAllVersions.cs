using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 下载所有版本号，应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetAllVersions_Reply : ReplyEntity
    {
        /// <summary>
        /// 部署方式的版本号
        /// </summary>
        [DataMember]
        public DeployVersionPair[] DeployVersionPairs { get; set; }

        /// <summary>
        /// 服务的版本号
        /// </summary>
        [DataMember]
        public ConfigurationVersionPair[] ConfigurationVersionPairs { get; set; }
    }


    [Serializable, DataContract]
    public class DeployVersionPair
    {
        /// <summary>
        /// 客户端的唯一标识
        /// </summary>
        [DataMember]
        public Guid ClientID { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime Version { get; set; }
    }

    [Serializable, DataContract]
    public class ConfigurationVersionPair
    {
        /// <summary>
        /// 服务描述
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime Version { get; set; }
    }
}

