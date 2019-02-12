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
    /// 上传服务安装包－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_UploadServiceDeployPackage_Request : RequestEntity
    {
        /// <summary>
        /// 服务安装包的信息
        /// </summary>
        [DataMember]
        public ServiceDeployPackageInfo DeployPackageInfo { get; set; }

        /// <summary>
        /// 安装包的内容
        /// </summary>
        [DataMember]
        public byte[] Content { get; set; }
    }
}
