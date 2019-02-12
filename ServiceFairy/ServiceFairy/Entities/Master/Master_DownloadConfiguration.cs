using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 下载指定的配置文件
    /// </summary>
    [Serializable, DataContract]
    public class Master_DownloadConfiguration_Request : RequestEntity
    {
        /// <summary>
        /// 服务与更新时间的组合
        /// </summary>
        [DataMember]
        public ServiceLastUpdatePair[] ServiceLastUploadPairs { get; set; }
    }

    /// <summary>
    /// 服务与更新时间的组合
    /// </summary>
    [Serializable, DataContract]
    public class ServiceLastUpdatePair
    {
        /// <summary>
        /// 服务描述
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime LastUpdate { get; set; }
    }

    /// <summary>
    /// 下载指定的配置文件
    /// </summary>
    [Serializable, DataContract]
    public class Master_DownloadConfiguration_Reply : ReplyEntity
    {
        /// <summary>
        /// 更新过的配置文件
        /// </summary>
        [DataMember]
        public ServiceConfigurationPair[] ServiceConfigurationPairs { get; set; }
    }

    /// <summary>
    /// 服务与配置文件的组合
    /// </summary>
    [Serializable, DataContract]
    public class ServiceConfigurationPair
    {
        /// <summary>
        /// 服务描述
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }

        /// <summary>
        /// 配置文件
        /// </summary>
        [DataMember]
        public AppServiceConfiguration Configuration { get; set; }
    }
}
