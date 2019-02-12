using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Data;

namespace ServiceFairy.Entities.Database
{
    /// <summary>
    /// 插入数据－请求
    /// </summary>
    [Serializable, DataContract]
    public class Database_Insert_Request : DatabaseRequestEntity
    {
        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public DataList Data { get; set; }

        /// <summary>
        /// 当数据已经存在时，是否自动更新
        /// </summary>
        [DataMember]
        public bool AutoUpdate { get; set; }
    }

    /// <summary>
    /// 插入数据－应答
    /// </summary>
    [Serializable, DataContract]
    public class Database_Insert_Reply : ReplyEntity
    {
        /// <summary>
        /// 新生成的主键
        /// </summary>
        [DataMember]
        public object[] NewPrimaryKeys { get; set; }

        /// <summary>
        /// 受影响的行数
        /// </summary>
        [DataMember]
        public int EffectRowCount { get; set; }
    }
}
