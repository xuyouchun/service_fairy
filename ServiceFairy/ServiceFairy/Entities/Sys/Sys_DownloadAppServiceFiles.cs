using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 下载AppService的文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_DownloadAppServiceFiles_Request : RequestEntity
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [DataMember]
        public string[] FileNames { get; set; }
    }

    /// <summary>
    /// 下载AppService的文件－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_DownloadAppServiceFiles_Reply : ReplyEntity
    {
        /// <summary>
        /// 文件内容
        /// </summary>
        [DataMember]
        public AppServiceFileData[] Files { get; set; }
    }

    /// <summary>
    /// AppService文件
    /// </summary>
    [Serializable, DataContract]
    public class AppServiceFileData
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public byte[] Content { get; set; }
    }
}
