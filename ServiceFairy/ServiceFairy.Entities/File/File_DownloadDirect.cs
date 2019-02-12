using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.File.UnionFile;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 直接下载文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_DownloadDirect_Request : RequestEntity
    {
        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 每次下载的最大长度
        /// </summary>
        [DataMember]
        public int MaxSize { get; set; }
    }

    /// <summary>
    /// 直接下载文件－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_DownloadDirect_Reply : ReplyEntity
    {
        /// <summary>
        /// 文件信息（只在第一次请求时返回）
        /// </summary>
        [DataMember]
        public UnionFileInfo FileInfo { get; set; }

        /// <summary>
        /// 事务标识，只有当不能一次下载完成时返回该标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        /// <summary>
        /// 字节流
        /// </summary>
        [DataMember]
        public byte[] Bytes { get; set; }
    }
}
