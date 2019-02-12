using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 部署到指定的终端－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_DeployPlatformPackage_Request : RequestEntity
    {
        /// <summary>
        /// 平台安装包ID
        /// </summary>
        [DataMember]
        public Guid DeployPackageId { get; set; }

        /// <summary>
        /// 终端ID，如果为空则部署到所有的终端
        /// </summary>
        [DataMember]
        public Guid[] ClientIds { get; set; }
    }
}
