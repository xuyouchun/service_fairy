using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取指定时间范围内的日志－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetLocalLogItemsByTime_Request : RequestEntity
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 最大数量
        /// </summary>
        [DataMember]
        public int MaxCount { get; set; }
    }

    /// <summary>
    /// 获取指定时间范围内的日志－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetLocalLogItemsByTime_Reply : ReplyEntity
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
