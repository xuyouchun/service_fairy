using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取所有服务－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetAllServices_Reply : ReplyEntity
    {
        /// <summary>
        /// 服务
        /// </summary>
        [DataMember]
        public ServiceDesc[] Services { get; set; }
    }
}
