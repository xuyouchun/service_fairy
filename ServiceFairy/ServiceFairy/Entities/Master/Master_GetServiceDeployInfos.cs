using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 获取服务的部署信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetServiceDeployInfos_Request : RequestEntity
    {
        /// <summary>
        /// 服务
        /// </summary>
        [DataMember]
        public ServiceDesc[] ServiceDescs { get; set; }
    }

    /// <summary>
    /// 获取服务的部署信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetServiceDeployInfos_Reply : ReplyEntity
    {
        [DataMember]
        public ServiceDeployInfo[] ServiceDeployInfos { get; set; }
    }

    /// <summary>
    /// 服务的部署信息
    /// </summary>
    [Serializable, DataContract]
    public class ServiceDeployInfo
    {
        /// <summary>
        /// 服务
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }

        /// <summary>
        /// 部署位置
        /// </summary>
        [DataMember]
        public Guid[] ClientIDs { get; set; }
    }
}
