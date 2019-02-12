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
    /// 合并数据－请求
    /// </summary>
    [Serializable, DataContract]
    public class Database_Merge_Request : DatabaseRequestEntity
    {
        /// <summary>
        /// 路由键
        /// </summary>
        [DataMember]
        public object RouteKey { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public DataList Data { get; set; }

        /// <summary>
        /// 用于比较的列
        /// </summary>
        [DataMember]
        public string[] CompareColumns { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        [DataMember]
        public string Where { get; set; }

        /// <summary>
        /// 合并选项
        /// </summary>
        [DataMember]
        public UtConnectionMergeOption Option { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Data, "Data");
        }
    }

    /// <summary>
    /// 合并数据－应答
    /// </summary>
    [Serializable, DataContract]
    public class Database_Merge_Reply : ReplyEntity
    {
        /// <summary>
        /// 受影响的行数
        /// </summary>
        [DataMember]
        public int EffectRowCount { get; set; }
    }
}
