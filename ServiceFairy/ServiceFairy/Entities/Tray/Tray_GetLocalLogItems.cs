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
    /// 获取指定组的日志－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetLocalLogItems_Request : RequestEntity
    {
        /// <summary>
        /// 日志组名
        /// </summary>
        [DataMember]
        public string Group { get; set; }
    }

    /// <summary>
    /// 获取指定组的日志－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetLocalLogItems_Reply : ReplyEntity
    {
        /// <summary>
        /// 字符串表
        /// </summary>
        [DataMember]
        public string[] StringTable { get; set; }

        /// <summary>
        /// 日志项
        /// </summary>
        [DataMember]
        public StLogItem[] LogItems { get; set; }
    }
}
