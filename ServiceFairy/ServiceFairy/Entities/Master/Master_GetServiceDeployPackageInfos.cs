using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using ServiceFairy.Entities.Deploy;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 下载全部安装包的信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetServiceDeployPackageInfos_Request : RequestEntity
    {
        /// <summary>
        /// 是否仅获取当前所用版本的安装包信息
        /// </summary>
        [DataMember]
        public bool OnlyCurrent { get; set; }

        /// <summary>
        /// 需要获取的安装包信息，如果为空则返回全部安装包信息
        /// </summary>
        [DataMember]
        public ServiceDesc[] ServiceDescs { get; set; }

        /// <summary>
        /// 安装包标识，如果为空则返回全部安装包信息
        /// </summary>
        [DataMember]
        public Guid[] DeployPackageIds { get; set; }
    }

    /// <summary>
    /// 下载全部安装包的信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetServiceDeployPackageInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 所有安装包信息
        /// </summary>
        [DataMember]
        public ServiceDeployPackageInfo[] ServiceDescDeployPackageInfos { get; set; }
    }
}
