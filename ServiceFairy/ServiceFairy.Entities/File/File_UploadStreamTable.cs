using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Package.Storage;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 上传StreamTable－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_UploadStreamTable_Request : RequestEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        /// <summary>
        /// 表信息，如果该值不为空，则创建新表
        /// </summary>
        [DataMember]
        public NewStreamTableInfo TableInfo { get; set; }

        /// <summary>
        /// 行
        /// </summary>
        [DataMember]
        public StreamTableRowData[] Rows { get; set; }

        /// <summary>
        /// 是否已经到结尾
        /// </summary>
        [DataMember]
        public bool AtEnd { get; set; }
    }

    /// <summary>
    /// 表信息
    /// </summary>
    [Serializable, DataContract]
    public class NewStreamTableInfo
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
        /// 列
        /// </summary>
        [DataMember]
        public StreamTableColumn[] Columns { get; set; }
    }

    /// <summary>
    /// 行数据
    /// </summary>
    [Serializable, DataContract]
    public class StreamTableRowData
    {
        /// <summary>
        /// 列数据
        /// </summary>
        [DataMember]
        public string[] Datas { get; set; }
    }
}
