using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common;
using Common.Data.UnionTable;

namespace ServiceFairy.Entities.Database
{
    /// <summary>
    /// 数据库请求实体
    /// </summary>
    [Serializable, DataContract]
    public class DatabaseRequestEntity : RequestEntity
    {
        /// <summary>
        /// 表名
        /// </summary>
        [DataMember]
        public string TableName { get; set; }

        /// <summary>
        /// 调用设置
        /// </summary>
        [DataMember]
        public UtInvokeSettings Settings { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrEmpty(TableName, "TableName");
        }
    }

    /// <summary>
    /// 数据库状态码
    /// </summary>
    public enum DatabaseStatusCode
    {
        [Desc("Error")]
        Error = SFStatusCodes.Database,

        /// <summary>
        /// 表不存在
        /// </summary>
        [Desc("表不存在")]
        TableNotExist = Error | 1,
    }
}
