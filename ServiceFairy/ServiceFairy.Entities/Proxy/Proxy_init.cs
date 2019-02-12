using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts;

namespace ServiceFairy.Entities.Proxy
{
    /// <summary>
    /// 设置设备信息
    /// </summary>
    [Serializable, DataContract]
    public class Proxy_Init_Request : RequestEntity
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        public string DeviceName { get; set; }

        /// <summary>
        /// 操作系统版本号
        /// </summary>
        [DataMember]
        public string OsVersion { get; set; }

        /// <summary>
        /// 支持的编码方式
        /// </summary>
        [DataMember]
        public DataFormat[] SupportedDataFormats { get; set; }
    }
}
