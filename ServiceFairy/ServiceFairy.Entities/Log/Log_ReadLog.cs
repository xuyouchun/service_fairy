using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Log
{
    /// <summary>
    /// 读取日志－请求
    /// </summary>
    [Serializable, DataContract]
    public class Log_ReadLog_Request : RequestEntity
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
        /// 起始索引
        /// </summary>
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public int Count { get; set; }
    }

    /// <summary>
    /// 读取日志－应答
    /// </summary>
    [Serializable, DataContract]
    public class Log_ReadLog_Reply : ReplyEntity
    {
        /// <summary>
        /// 日志项
        /// </summary>
        [DataMember]
        public PolyLog[] Logs { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }
    }
}
