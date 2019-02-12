using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Collection;
using System.Data;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Collections.Concurrent;
using System.Threading;
using Common.Data.UnionTable.Metadata;

namespace Common.Data
{
    /// <summary>
    /// 数据集合
    /// </summary>
    [Serializable, DataContract]
    [System.Diagnostics.DebuggerDisplay("Count = {Rows.Length}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DebuggerProxy))]
    public class DataList
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columns">列集合</param>
        /// <param name="name">表名</param>
        /// <param name="primaryKey">主键</param>
        public DataList(string name, ColumnInfo[] columns, DataListRow[] rows, string primaryKey)
        {
            Name = name;
            PrimaryKey = primaryKey;

            Columns = columns ?? Array<ColumnInfo>.Empty;
            Rows = rows ?? Array<DataListRow>.Empty;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 列集合
        /// </summary>
        [DataMember]
        public ColumnInfo[] Columns { get; private set; }

        /// <summary>
        /// 行集合
        /// </summary>
        [DataMember]
        public DataListRow[] Rows { get; private set; }

        [NonSerialized, IgnoreDataMember]
        private Dictionary<string, int> _columnIndexCache;

        /// <summary>
        /// 获取列的位置
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public int GetColumnIndex(string columnName, bool throwError = false)
        {
            Dictionary<string, int> cache = (_columnIndexCache ?? (_columnIndexCache = new Dictionary<string, int>()));

            int index;
            if (cache.TryGetValue(columnName, out index))
                return index;

            index = Array.FindIndex(Columns, column => string.Equals(column.Name, columnName, StringComparison.OrdinalIgnoreCase));
            if (index >= 0)
            {
                cache.Add(columnName, index);
                return index;
            }

            if (throwError)
                throw new IndexOutOfRangeException(string.Format("数据集中不存在列“{0}”", columnName));

            return index;
        }

        /// <summary>
        /// 获取主键列的位置
        /// </summary>
        /// <returns></returns>
        public int GetPrimaryKeyColumnIndex()
        {
            if (PrimaryKey == null)
                return -1;

            return GetColumnIndex(PrimaryKey);
        }

        /// <summary>
        /// 设置指定行列的值
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnName">列名</param>
        /// <param name="value">值</param>
        public void SetValue(int rowIndex, string columnName, object value)
        {
            SetValue(rowIndex, GetColumnIndex(columnName), value);
        }

        /// <summary>
        /// 设置指定行列的值
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="value">值</param>
        public void SetValue(int rowIndex, int columnIndex, object value)
        {
            Rows[rowIndex].Cells[columnIndex] = value;
        }

        /// <summary>
        /// 获取指定行列的值
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public object GetValue(int rowIndex, string columnName)
        {
            return GetValue(rowIndex, GetColumnIndex(columnName));
        }

        /// <summary>
        /// 获取指定行列的值
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colunmIndex"></param>
        /// <returns></returns>
        public object GetValue(int rowIndex, int colunmIndex)
        {
            return Rows[rowIndex].Cells[colunmIndex];
        }

        /// <summary>
        /// 获取指定列的所有值
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object[] GetValues(string columnName)
        {
            return GetValues(GetColumnIndex(columnName));
        }

        /// <summary>
        /// 获取指定列的所有值
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public object[] GetValues(int columnIndex)
        {
            object[] values = new object[Rows.Length];
            for (int k = 0, len = Rows.Length; k < len; k++)
            {
                values[k] = GetValue(k, columnIndex);
            }

            return values;
        }

        /// <summary>
        /// 将DataReader转换为DataList
        /// </summary>
        /// <returns></returns>
        public DataList FromDataReader(IDataReader reader)
        {
            Contract.Requires(reader != null);

            DataListBuilder builder = new DataListBuilder();
            int fieldCount = reader.FieldCount;
            for (int k = 0; k < fieldCount; k++)
            {
                string name = reader.GetName(k);
                Type type = reader.GetFieldType(k);

                builder.AddColumn(name, type.ToDbColumnType());
            }

            while (reader.Read())
            {
                object[] values = new object[fieldCount];
                reader.GetValues(values);
                builder.AddRow(values);
            }

            return builder.ToDataList();
        }

        #region Class DebuggerProxy ...

        class DebuggerProxy
        {
            public DebuggerProxy(DataList owner)
            {
                Rows = owner.Rows;
            }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public readonly DataListRow[] Rows;
        }

        #endregion

        public static readonly DataList Empty = new DataList("", Array<ColumnInfo>.Empty, Array<DataListRow>.Empty, "");

        /// <summary>
        /// 从DataReader转化
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="tableName"></param>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public static DataList FromDataReader(IDataReader reader, string tableName = null, string primaryKey = null, Func<string, ColumnInfo> columnInfoProvider = null)
        {
            Contract.Requires(reader != null);

            DataListBuilder db = new DataListBuilder(tableName, primaryKey);
            for (int k = 0, len = reader.FieldCount; k < len; k++)
            {
                string name = reader.GetName(k);
                ColumnInfo columnInfo = ((columnInfoProvider != null) ? columnInfoProvider(name) : null) ?? new ColumnInfo(name);
                db.AddColumn(columnInfo);
            }

            _AppendReaderToRows(db, reader);
            return db.ToDataList();
        }

        private static void _AppendReaderToRows(DataListBuilder db, IDataReader reader)
        {
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);
                db.AddRow(values);
            }
        }

        /// <summary>
        /// 从DataReader转化
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="mtTable"></param>
        /// <param name="tableName"></param>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public static DataList FromDataReader(IDataReader reader, MtTable mtTable)
        {
            Contract.Requires(mtTable != null);

            return FromDataReader(reader, mtTable.Name, mtTable.PrimaryKey,
                (colName) => { MtColumn c = mtTable.Columns[colName]; return c == null ? null : c.ToColumnInfo(); }
            );
        }

        /// <summary>
        /// 是否包含指定的列名
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public bool ContainsColumn(string colName)
        {
            return GetColumnIndex(colName) >= 0;
        }
    }
}
