using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Collection;
using System.Runtime.Serialization;
using Common.Data.UnionTable;
using Common.Utility;

namespace Common.Data
{
    /// <summary>
    /// 表的信息
    /// </summary>
    [Serializable, DataContract]
    public class TableInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="name">表名</param>
        /// <param name="fieldInfos">字段信息</param>
        /// <param name="partialTableCount">分表数量</param>
        public TableInfo(string group, string name, ColumnInfo[] fieldInfos, int partialTableCount)
        {
            Contract.Requires(group != null && name != null && fieldInfos != null);

            Group = group;
            Name = name;
            ColumnInfos = fieldInfos;
            PartialTableCount = partialTableCount;
        }

        /// <summary>
        /// 表名
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 组名
        /// </summary>
        [DataMember]
        public string Group { get; private set; }

        /// <summary>
        /// 列的集合
        /// </summary>
        [DataMember]
        public ColumnInfo[] ColumnInfos { get; private set; }

        /// <summary>
        /// 分表数量
        /// </summary>
        [DataMember]
        public int PartialTableCount { get; private set; }

        /// <summary>
        /// 所有的组名
        /// </summary>
        [IgnoreDataMember]
        public string[] AllGroups
        {
            get { return _allGroups ?? (_allGroups = UtUtility.GetGroupNames(ColumnInfos.ToArray(ci => ci.Name))); }
        }

        [NonSerialized, IgnoreDataMember]
        private string[] _allGroups;

        public override string ToString()
        {
            return Name;
        }
    }
}
