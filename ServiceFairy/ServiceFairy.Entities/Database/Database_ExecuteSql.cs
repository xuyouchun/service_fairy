using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Data;
using Common.Data.UnionTable;

namespace ServiceFairy.Entities.Database
{
    /// <summary>
    /// 执行Sql语句－请求
    /// </summary>
    [Serializable, DataContract]
    public class Database_ExecuteSql_Request : RequestEntity
    {
        /// <summary>
        /// Sql语句
        /// </summary>
        [DataMember]
        public string Sql { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [DataMember]
        public object[] Parameters { get; set; }

        /// <summary>
        /// 调用设置
        /// </summary>
        [DataMember]
        public UtInvokeSettings Settings { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Sql, "Sql");
        }
    }

    /// <summary>
    /// 执行Sql语句－应答
    /// </summary>
    [Serializable, DataContract]
    public class Database_ExecuteSql_Reply : ReplyEntity
    {
        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public DataList Data { get; set; }
    }
}
