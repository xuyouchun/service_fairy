using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using ServiceFairy.Entities.Master;
using Common;
using Common.Contracts;

namespace ServiceFairy.Entities.Deploy
{
    /// <summary>
    /// 下载安装包，请求
    /// </summary>
    [Serializable, DataContract]
    public class Deploy_DownloadDeployPackage_Request : RequestEntity
    {
        /// <summary>
        /// 服务描述
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }
    }

    /// <summary>
    /// 下载安装包，应答
    /// </summary>
    [Serializable, DataContract]
    public class Deploy_DownloadDeployPackage_Reply : ReplyEntity
    {
        /// <summary>
        /// 安装包
        /// </summary>
        [DataMember]
        public DeployPackage DeployPackage { get; set; }
    }
}
