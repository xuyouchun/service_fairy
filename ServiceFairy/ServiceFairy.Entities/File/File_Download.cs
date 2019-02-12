using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 下载文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_Download_Request : RequestEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        /// <summary>
        /// 下载文件的最大长度
        /// </summary>
        [DataMember]
        public int MaxSize { get; set; }
    }

    /// <summary>
    /// 下载文件－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_Download_Reply : ReplyEntity
    {
        /// <summary>
        /// 文件内容
        /// </summary>
        [DataMember]
        public byte[] Buffer { get; set; }
    }
}
