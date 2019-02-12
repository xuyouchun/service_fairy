using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Data;

namespace ServiceFairy.Entities.Database
{
    /// <summary>
    /// 删除数据－请求
    /// </summary>
    [Serializable, DataContract]
    public class Database_Delete_Request : DatabaseRequestEntity
    {
        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public DataList Data { get; set; }

        /// <summary>
        /// 删除条件
        /// </summary>
        [DataMember]
        public string Where { get; set; }
    }

    /// <summary>
    /// 删除数据－应答
    /// </summary>
    [Serializable, DataContract]
    public class Database_Delete_Reply : ReplyEntity
    {
        /// <summary>
        /// 受影响的行数
        /// </summary>
        [DataMember]
        public int EffectRowCount { get; set; }
    }
}
