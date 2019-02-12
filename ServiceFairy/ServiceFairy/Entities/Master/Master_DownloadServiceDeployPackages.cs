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
    /// 下载安装包，请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_DownloadServiceDeployPackage_Request : RequestEntity
    {
        /// <summary>
        /// 服务
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }

        /// <summary>
        /// 安装包ID
        /// </summary>
        [DataMember]
        public Guid DeployPackageId { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (ServiceDesc == null && DeployPackageId == default(Guid))
                throw EntityValidate.CreateArgumentException("下载服务安装包时，需要指定服务描述或安装包ID");

            if (ServiceDesc != null && DeployPackageId != default(Guid))
                throw EntityValidate.CreateArgumentException("下载服务的安装包时，服务描述与安装包ID不可以同时指定");
        }
    }

    /// <summary>
    /// 下载安装包，应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_DownloadServiceDeployPackage_Reply : ReplyEntity
    {
        /// <summary>
        /// 安装包
        /// </summary>
        [DataMember]
        public DeployPackage DeployPackage { get; set; }
    }

    /// <summary>
    /// 安装包
    /// </summary>
    [Serializable, DataContract]
    public class DeployPackage
    {
        /// <summary>
        /// 信息
        /// </summary>
        [DataMember]
        public DeployPackageFormat Format { get; set; }

        /// <summary>
        /// 安装包内容
        /// </summary>
        [DataMember]
        public byte[] Content { get; set; }
    }
}
