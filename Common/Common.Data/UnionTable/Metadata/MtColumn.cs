using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Utility;

namespace Common.Data.UnionTable.Metadata
{
    /// <summary>
    /// 字段的元数据
    /// </summary>
    [Serializable, DataContract]
    public class MtColumn : MtBase<MtTable>
    {
        public MtColumn(string name, string desc, DbColumnType type, int size, DbColumnIndexType indexType)
            : base(name, desc)
        {
            Type = type;

            DbColumnTypeInfo typeInfo = DbColumnTypeInfo.GetTypeInfo(type);
            if (typeInfo != null && !typeInfo.IsVarLen)
                Size = typeInfo.ElementSize;
            else
                Size = size;

            IndexType = indexType;
        }

        internal MtColumn(string name, DbColumnType type, int size = 0, DbColumnIndexType indexType = DbColumnIndexType.None)
            : this(name, "", type, size, indexType)
        {

        }

        internal MtColumn(string name, DbColumnType type, DbColumnIndexType indexType)
            : this(name, "", type, 0, indexType)
        {

        }

        internal MtColumn(ColumnInfo colInfo, DbColumnIndexType indexType)
            : this(colInfo.Name, colInfo.Type, colInfo.Size, indexType)
        {

        }

        /// <summary>
        /// 列类型
        /// </summary>
        [DataMember]
        public DbColumnType Type { get; private set; }

        /// <summary>
        /// 长度限制
        /// </summary>
        [DataMember]
        public int Size { get; private set; }

        /// <summary>
        /// 是否有索引
        /// </summary>
        [DataMember]
        public DbColumnIndexType IndexType { get; private set; }

        public override int GetHashCode()
        {
            return CommonUtility.GetHashCode(new object[] { Name, Desc, Type, Size, IndexType });
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(MtColumn))
                return false;

            MtColumn obj2 = (MtColumn)obj;
            return string.Compare(Name, obj2.Name, true) == 0
                && Size == obj2.Size && Type == obj2.Type && IndexType == obj2.IndexType;
        }

        public override string ToString()
        {
            string s = string.Format("{0} {1}", Name, Type);
            DbColumnTypeInfo typeInfo = DbColumnTypeInfo.GetTypeInfo(Type);
            if (typeInfo != null && typeInfo.IsVarLen)
                s += "(" + (typeInfo.ElementSize == 0 ? "Max" : (Size / typeInfo.ElementSize).ToString()) + ")";

            return s;
        }

        /// <summary>
        /// 转换为TableColumnInfo
        /// </summary>
        /// <returns></returns>
        public ColumnInfo ToColumnInfo()
        {
            return new ColumnInfo(Name, Type, Size);
        }
    }
}
