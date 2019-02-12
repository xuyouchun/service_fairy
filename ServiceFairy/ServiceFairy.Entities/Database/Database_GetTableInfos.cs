using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Data;

namespace ServiceFairy.Entities.Database
{
    /// <summary>
    /// 获取表信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Database_GetTableInfos_Request : RequestEntity
    {
        /// <summary>
        /// 表名，若为空引用，则获取全部表信息
        /// </summary>
        [DataMember]
        public string[] TableNames { get; set; }
    }

    /// <summary>
    /// 获取表信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Database_GetTableInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 表信息
        /// </summary>
        [DataMember]
        public TableInfo[] TableInfos { get; set; }
    }
}
