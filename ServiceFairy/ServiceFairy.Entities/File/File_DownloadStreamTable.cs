using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 下载StreamTable－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_DownloadStreamTable_Request : RequestEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        [DataMember]
        public string Table { get; set; }

        /// <summary>
        /// 起始行
        /// </summary>
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        [DataMember]
        public int Count { get; set; }
    }

    /// <summary>
    /// 下载StreamTable－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_DownloadStreamTable_Reply : ReplyEntity
    {
        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public StreamTableRowData[] Rows { get; set; }
    }
}
