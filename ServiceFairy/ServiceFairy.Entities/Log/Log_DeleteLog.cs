using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Log
{
    /// <summary>
    /// 删除日志－请求
    /// </summary>
    [Serializable, DataContract]
    public class Log_DeleteLog_Request : RequestEntity
    {
        /// <summary>
        /// 起始时间（如果留空，则删除EndTime之前的所有日志）
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间（如果留空，则删除StartTime之后的所有日志）
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }
    }
}
