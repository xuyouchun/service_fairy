using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 下载文件系统中的文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_FsDownloadFile_Request : RequestEntity
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }
    }

    /// <summary>
    /// 下载文件系统中的文件－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_FsDownloadFile_Reply : ReplyEntity
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        [DataMember]
        public FsFileInfo Info { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        [DataMember]
        public byte[] Content { get; set; }
    }
}
