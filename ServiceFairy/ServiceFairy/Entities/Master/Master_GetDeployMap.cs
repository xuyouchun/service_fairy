using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 获取部署地图－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetDeployMap_Request : ServiceRequestEntity
    {
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime LastUpdate { get; set; }
    }

    /// <summary>
    /// 获取部署地图－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetDeployMap_Reply : ReplyEntity
    {
        /// <summary>
        /// 部署地图
        /// </summary>
        [DataMember]
        public AppClientDeployInfo[] DeployInfos { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime LastUpdate { get; set; }
    }
}
