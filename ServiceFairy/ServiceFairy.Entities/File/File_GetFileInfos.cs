using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.File.UnionFile;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 获取文件信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_GetFileInfos_Request : RequestEntity
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string[] Paths { get; set; }
    }

    /// <summary>
    /// 获取文件信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_GetFileInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        [DataMember]
        public UnionFileInfo[] Infos { get; set; }
    }
}
