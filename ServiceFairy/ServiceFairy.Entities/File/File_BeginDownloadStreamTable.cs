using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Package.Storage;
using Common.File.UnionFile;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 开始下载StreamTable－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_BeginDownloadStreamTable_Request : RequestEntity
    {
        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }
    }

    /// <summary>
    /// 开始下载StreamTable－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_BeginDownloadStreamTable_Reply : ReplyEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        /// <summary>
        /// 表的基础信息
        /// </summary>
        [DataMember]
        public StreamTableBasicInfo BasicInfo { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        [DataMember]
        public UnionFileInfo FileInfo { get; set; }
    }

    /// <summary>
    /// StreamTable的基础信息
    /// </summary>
    [Serializable, DataContract]
    public class StreamTableBasicInfo
    {
        /// <summary>
        /// 表信息项
        /// </summary>
        [DataMember]
        public StreamTableBasicTableInfo[] TableInfos { get; set; }
    }

    /// <summary>
    /// StreamTable基础信息项
    /// </summary>
    [Serializable, DataContract]
    public class StreamTableBasicTableInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        [DataMember]
        public int RowCount { get; set; }

        /// <summary>
        /// 列信息
        /// </summary>
        [DataMember]
        public StreamTableColumn[] Columns { get; set; }
    }
}
