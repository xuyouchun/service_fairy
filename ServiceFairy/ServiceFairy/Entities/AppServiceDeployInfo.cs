using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 服务的部署信息
    /// </summary>
    [Serializable, DataContract]
    public class AppServiceDeployInfo
    {
        public AppServiceDeployInfo()
        {

        }

        public AppServiceDeployInfo(ServiceDesc serviceDesc)
        {
            ServiceDesc = serviceDesc;
        }

        /// <summary>
        /// 服务
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }
    }
}
