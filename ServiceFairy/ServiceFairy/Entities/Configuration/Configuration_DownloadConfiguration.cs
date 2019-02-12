using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Configuration
{
    /// <summary>
    /// 下载配置信息，请求
    /// </summary>
    [Serializable, DataContract]
    public class Configuration_DownloadConfiguration_Request : RequestEntity
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

        public override void Validate()
        {
            base.Validate();

            if (ServiceDesc == null)
                throw new ServiceException(ServerErrorCode.ArgumentError, "未指定服务描述信息");
        }
    }

    /// <summary>
    /// 下载配置信息，应答
    /// </summary>
    [Serializable, DataContract]
    public class Configuration_DownloadConfiguration_Reply : ReplyEntity
    {
        /// <summary>
        /// 配置，如果返回空，则表示不需要更新
        /// </summary>
        [DataMember]
        public AppServiceConfiguration Configuration { get; set; }
    }
}
