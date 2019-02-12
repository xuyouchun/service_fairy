using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.File.UnionFile;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 获取指定目录的信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_GetDirectoryInfos_Request : RequestEntity
    {
        /// <summary>
        /// 目录
        /// </summary>
        [DataMember]
        public string[] Paths { get; set; }

        /// <summary>
        /// 子目录过滤符
        /// </summary>
        [DataMember]
        public string Pattern { get; set; }

        /// <summary>
        /// 选项
        /// </summary>
        [DataMember]
        public GetDirectoryInfosOption Option { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(Paths, "Path");
        }
    }

    public enum GetDirectoryInfosOption
    {
        /// <summary>
        /// 不需要返回子目录或文件的信息
        /// </summary>
        None = 0x00,

        /// <summary>
        /// 需要返回所包含的文件信息
        /// </summary>
        File = 0x01,

        /// <summary>
        /// 需要返回子目录信息
        /// </summary>
        Directory = 0x02,

        /// <summary>
        /// 全部
        /// </summary>
        All = -1,
    }

    /// <summary>
    /// 获取指定目录的信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_GetDirectoryInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 目录信息的集合
        /// </summary>
        [DataMember]
        public DirectoryInfoItem[] Items { get; set; }  
    }

    /// <summary>
    /// 目录的信息
    /// </summary>
    [Serializable, DataContract]
    public class DirectoryInfoItem
    {
        /// <summary>
        /// 目录信息
        /// </summary>
        [DataMember]
        public UnionDirectoryInfo DirectoryInfo { get; set; }

        /// <summary>
        /// 所包含的文件信息
        /// </summary>
        [DataMember]
        public UnionFileInfo[] FileInfos { get; set; }

        /// <summary>
        /// 所包含的目录信息
        /// </summary>
        [DataMember]
        public UnionDirectoryInfo[] SubDirectoryInfos { get; set; }
    }
}
