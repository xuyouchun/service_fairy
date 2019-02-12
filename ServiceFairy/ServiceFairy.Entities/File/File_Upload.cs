using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 上传文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_Upload_Request : RequestEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        [DataMember]
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 是否已经结束
        /// </summary>
        [DataMember]
        public bool AtEnd { get; set; }
    }

    /// <summary>
    /// 上传文件－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_Upload_Reply : ReplyEntity
    {

    }
}
