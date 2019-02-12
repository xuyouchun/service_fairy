using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Configuration
{
    /// <summary>
    /// 下载环境变量－请求
    /// </summary>
    [Serializable, DataContract]
    public class Configuration_DownloadEnvironmentValues_Request : RequestEntity
    {
        /// <summary>
        /// 最后更新时间，如果相同则返回空
        /// </summary>
        [DataMember]
        public DateTime LastUpdate { get; set; }
    }

    /// <summary>
    /// 下载环境变量－应答
    /// </summary>
    [Serializable, DataContract]
    public class Configuration_DownloadEnvironmentValues_Reply : ReplyEntity
    {
        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public EnvironmentValues Values { get; set; }
    }
}
