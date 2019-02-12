using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Watch
{
    /// <summary>
    /// 获取监控信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Watch_GetWatchInfo_Reply : ReplyEntity
    {
        /// <summary>
        /// 监控信息
        /// </summary>
        [DataMember]
        public WatchInfo Info { get; set; }
    }

    /// <summary>
    /// 监控信息
    /// </summary>
    [Serializable, DataContract]
    public class WatchInfo
    {

    }
}
