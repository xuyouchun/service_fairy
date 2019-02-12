using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Data.UnionTable.Metadata
{
    /// <summary>
    /// 表的元数据
    /// </summary>
    [Serializable, DataContract]
    public class MtTable : MtBase<MtTableGroup>
    {
        public MtTable(string name, string desc, string defaultGroup, string primaryKey, DbColumnType primaryKeyColumnType, int partialTableCount)
            : base(name, desc)
        {
            Columns = new MtCollection<MtColumn, MtTable>(this);
            PrimaryKey = primaryKey;
            DefaultGroup = defaultGroup;
            PrimaryKeyColumnType = primaryKeyColumnType;
            PartialTableCount = partialTableCount;
        }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public string PrimaryKey { get; private set; }

        /// <summary>
        /// 默认组名
        /// </summary>
        [DataMember]
        public string DefaultGroup { get; private set; }

        /// <summary>
        /// 主键列类型
        /// </summary>
        [DataMember]
        public DbColumnType PrimaryKeyColumnType { get; private set; }

        /// <summary>
        /// 列
        /// </summary>
        [DataMember]
        public MtCollection<MtColumn, MtTable> Columns { get; private set; }

        /// <summary>
        /// 分表数量
        /// </summary>
        [DataMember]
        public int PartialTableCount { get; private set; }
    }
}
