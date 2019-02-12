using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 下载平台安装包－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_DownloadPlatformDeployPackage_Request : RequestEntity
    {
        /// <summary>
        /// 安装包的ID
        /// </summary>
        [DataMember]
        public Guid DeployPackageId { get; set; }
    }

    /// <summary>
    /// 下载平台安装包－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_DownloadPlatformDeployPackage_Reply : ReplyEntity
    {
        /// <summary>
        /// 安装包
        /// </summary>
        [DataMember]
        public DeployPackage DeployPackage { get; set; }
    }
}
