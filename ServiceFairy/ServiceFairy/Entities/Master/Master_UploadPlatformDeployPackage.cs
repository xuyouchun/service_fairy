using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ServiceFairy.Entities.Deploy;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 上传Framework安装包－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_UploadPlatformDeployPackage_Request : RequestEntity
    {
        /// <summary>
        /// 安装包信息
        /// </summary>
        [DataMember]
        public PlatformDeployPackageInfo DeployPackageInfo { get; set; }

        /// <summary>
        /// 安装包的内容
        /// </summary>
        [DataMember]
        public byte[] Content { get; set; }
    }
}
