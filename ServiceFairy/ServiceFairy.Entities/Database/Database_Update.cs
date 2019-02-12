using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Data;

namespace ServiceFairy.Entities.Database
{
    /// <summary>
    /// 更新数据－请求
    /// </summary>
    [Serializable, DataContract]
    public class Database_Update_Request : DatabaseRequestEntity
    {
        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public DataList Data { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        [DataMember]
        public object[] RouteKeys { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        [DataMember]
        public string Where { get; set; }
    }

    /// <summary>
    /// 更新数据－应答
    /// </summary>
    [Serializable, DataContract]
    public class Database_Update_Reply : ReplyEntity
    {
        /// <summary>
        /// 受影响的行数
        /// </summary>
        [DataMember]
        public int EffectRowCount { get; set; }
    }
}
