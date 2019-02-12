using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.File.UnionFile;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 开始下载文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_BeginDownload_Request : RequestEntity
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(Path, "Path");
        }
    }

    /// <summary>
    /// 开始下载文件－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_BeginDownload_Reply : ReplyEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        [DataMember]
        public UnionFileInfo FileInfo { get; set; }
    }
}
