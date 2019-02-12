using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Log
{
    /// <summary>
    /// 获取当前日志分析结果－应答
    /// </summary>
    [Serializable, DataContract]
    public class Log_GetAnalyzeResult_Reply : ReplyEntity
    {
        /// <summary>
        /// 日志分析结果
        /// </summary>
        [DataMember]
        public LogAnalyzeResult Result { get; set; }
    }

    /// <summary>
    /// 日志分析结果
    /// </summary>
    [Serializable, DataContract]
    public class LogAnalyzeResult
    {

    }
}
