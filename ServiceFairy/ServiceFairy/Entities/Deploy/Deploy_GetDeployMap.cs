using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Deploy
{
    /// <summary>
    /// 获取指定客户端的部署地图
    /// </summary>
    [Serializable, DataContract]
    public class Deploy_GetDeployMap_Request : RequestEntity
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        [DataMember]
        public Guid[] ClientIDs { get; set; }
    }

    /// <summary>
    /// 获取指定客户端的部署地图
    /// </summary>
    [Serializable, DataContract]
    public class Deploy_GetDeployMap_Reply : ReplyEntity
    {
        [DataMember]
        public AppClientDeployInfo[] DeployInfos { get; set; }
    }
}
