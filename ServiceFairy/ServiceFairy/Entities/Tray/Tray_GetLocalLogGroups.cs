using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Log;
using Common.Contracts;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取本地组日志－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetLocalLogGroups_Request : RequestEntity
    {
        /// <summary>
        /// 组
        /// </summary>
        [DataMember]
        public string ParentGroup { get; set; }
    }

    /// <summary>
    /// 获取本地组日志－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetLocalLogGroups_Reply : ReplyEntity
    {
        /// <summary>
        /// 组信息
        /// </summary>
        [DataMember]
        public LogItemGroup[] Groups { get; set; }
    }
}
