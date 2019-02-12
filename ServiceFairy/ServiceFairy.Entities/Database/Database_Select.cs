using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Data;

namespace ServiceFairy.Entities.Database
{
    /// <summary>
    /// 选取数据－请求
    /// </summary>
    [Serializable, DataContract]
    public class Database_Select_Request : DatabaseRequestEntity
    {
        /// <summary>
        /// 路由键
        /// </summary>
        [DataMember]
        public object[] RouteKeys { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        [DataMember]
        public DbSearchParam Param { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        [DataMember]
        public string[] Columns { get; set; }
    }

    /// <summary>
    /// 选取数据－应答
    /// </summary>
    [Serializable, DataContract]
    public class Database_Select_Reply : ReplyEntity
    {
        /// <summary>
        /// 查询结果
        /// </summary>
        [DataMember]
        public DataList Data { get; set; }
    }
}
