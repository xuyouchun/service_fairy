using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 获取服务的监控信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetWatchInfo_Reply : ReplyEntity
    {
        /// <summary>
        /// 服务的监控信息
        /// </summary>
        [DataMember]
        public WatchInfo[] Infos { get; set; }
    }

    /// <summary>
    /// 服务的监控信息
    /// </summary>
    [Serializable, DataContract]
    public class WatchInfo
    {

    }
}
