using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 获取Framework安装包信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetPlatformDeployPackageInfos_Request : RequestEntity
    {
        /// <summary>
        /// 安装包ID
        /// </summary>
        [DataMember]
        public Guid[] Ids { get; set; }
    }

    /// <summary>
    /// 获取Framework安装包信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetPlatformDeployPackageInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 所有安装包大小
        /// </summary>
        [DataMember]
        public PlatformDeployPackageInfo[] PackageInfos { get; set; }
    }
}
