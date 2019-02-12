using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Contracts.Service
{
    [Serializable, DataContract]
    public class AppServiceConfiguration
    {
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// 配置文件的内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }
    }
}
