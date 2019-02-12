using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.File.UnionFile;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 下载文件的全部内容－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_DownloadAll_Request : RequestEntity
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }
    }

    /// <summary>
    /// 下载文件的全部内容－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_DownloadAll_Reply : ReplyEntity
    {
        /// <summary>
        /// 文件内容
        /// </summary>
        [DataMember]
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        [DataMember]
        public UnionFileInfo FileInfo { get; set; }
    }
}
