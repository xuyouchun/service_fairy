using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using Common.Data.UnionTable;
using Common.Utility;

namespace Common.Data
{
    /// <summary>
    /// 列的信息
    /// </summary>
    [Serializable, DataContract]
    public class ColumnInfo
    {
        public ColumnInfo(string name, DbColumnType type = DbColumnType.AnsiString, int size = 0)
        {
            Contract.Requires(name != null);

            Name = name;
            Type = type;

            DbColumnTypeInfo typeInfo = DbColumnTypeInfo.GetTypeInfo(type);
            if (typeInfo == null || typeInfo.IsVarLen)
                Size = size;
            else
                Size = typeInfo.ElementSize;
        }

        /// <summary>
        /// 列名
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [DataMember]
        public DbColumnType Type { get; private set; }

        /// <summary>
        /// 长度
        /// </summary>
        [DataMember]
        public int Size { get; private set; }

        public override int GetHashCode()
        {
            return CommonUtility.GetHashCode(new object[] { Name, Type, Size });
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Name, Type, Size);
        }
    }
}
