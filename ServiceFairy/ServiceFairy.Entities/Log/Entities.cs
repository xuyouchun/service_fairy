using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Log
{
    /// <summary>
    /// 日志聚合项
    /// </summary>
    [Serializable, DataContract]
    public class PolyLog
    {
        /// <summary>
        /// 类别
        /// </summary>
        [DataMember]
        public string Category { get; set; }

        /// <summary>
        /// 终端ID
        /// </summary>
        [DataMember]
        public Guid[] ClientIds { get; set; }

        /// <summary>
        /// 源
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        [DataMember]
        public string Detail { get; set; }

        /// <summary>
        /// 发生的次数
        /// </summary>
        [DataMember]
        public int Times { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }
    }
}
