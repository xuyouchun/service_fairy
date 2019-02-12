using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data
{
    /// <summary>
    /// DataList创建器
    /// </summary>
    public class DataListBuilder
    {
        public DataListBuilder(string tableName = null, string primaryKey = null)
        {
            _tableName = tableName;
            _primarykey = primaryKey;
        }

        private readonly string _tableName, _primarykey;
        private readonly List<ColumnInfo> _columns = new List<ColumnInfo>();
        private readonly List<DataListRow> _rows = new List<DataListRow>();

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="type">列类型</param>
        /// <param name="size">列长度限制</param>
        public ColumnInfo AddColumn(string columnName, DbColumnType type = DbColumnType.AnsiString, int size = 0)
        {
            _ValidateAddColumn();

            ColumnInfo column = new ColumnInfo(columnName, type, size);
            _columns.Add(column);
            return column;
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public void AddColumn(ColumnInfo column)
        {
            _ValidateAddColumn();

            Contract.Requires(column != null);
            _columns.Add(column);
        }

        private void _ValidateAddColumn()
        {
            if (_rows.Count > 0)
                throw new DbException("在添加行之后不允许再对列进行修改");
        }

        /// <summary>
        /// 批量添加列
        /// </summary>
        /// <param name="columns"></param>
        public void AddColumns(IEnumerable<ColumnInfo> columns)
        {
            _ValidateAddColumn();

            Contract.Requires(columns != null);
            _columns.AddRange(columns);
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <returns></returns>
        public DataListRow AddRow()
        {
            DataListRow row = new DataListRow(_columns.Count);
            _rows.Add(row);
            return row;
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataListRow AddRow(object[] values)
        {
            Contract.Requires(values != null);

            if (values.Length != _columns.Count)
                throw new DbException("新添加的行的列数与DataList的列数不相等");

            DataListRow row = new DataListRow(values);
            _rows.Add(row);
            return row;
        }

        /// <summary>
        /// 批量添加行
        /// </summary>
        /// <param name="values"></param>
        public void AddRows(object[][] values)
        {
            Contract.Requires(values != null);

            foreach (object[] v in values)
            {
                AddRow(v);
            }
        }

        /// <summary>
        /// 添加另一个DataList中的数据
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="forceCopy">是否强制拷贝数据的副本</param>
        public void AddDataList(DataList dataList, bool forceCopy = false)
        {
            Contract.Requires(dataList != null);

            if (ColumnCount == 0)
                AddColumns(dataList.Columns);

            if (dataList.Rows.Length == 0)
                return;

            bool inOrder;
            int[] colIndexes = _FindColumnIndexes(dataList, out inOrder);
            foreach (DataListRow row in dataList.Rows)
            {
                AddRow((inOrder && !forceCopy) ? row.Cells : _GetCellValueArray(row, colIndexes));
            }
        }

        private object[] _GetCellValueArray(DataListRow row, int[] colIndexes)
        {
            object[] values = new object[colIndexes.Length];
            object[] cells = row.Cells;

            for (int k = 0; k < colIndexes.Length; k++)
            {
                int index = colIndexes[k];
                if (index >= 0)
                    values[k] = cells[index];
            }

            return values;
        }

        private int[] _FindColumnIndexes(DataList dataList, out bool inOrder)
        {
            int[] indexes = new int[_columns.Count];
            inOrder = (dataList.Columns.Length == _columns.Count);
            for (int k = 0; k < _columns.Count; k++)
            {
                string name = _columns[k].Name;
                indexes[k] = Array.FindIndex(dataList.Columns, ci => ci.Name.IgnoreCaseEqualsTo(name));

                if (indexes[k] != k)
                    inOrder = false;
            }

            return indexes;
        }

        /// <summary>
        /// 转换为DataList
        /// </summary>
        /// <returns></returns>
        public DataList ToDataList()
        {
            return new DataList(_tableName, _columns.ToArray(), _rows.ToArray(), _primarykey);
        }

        /// <summary>
        /// 相关数据
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount
        {
            get { return _rows.Count; }
        }

        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnCount
        {
            get { return _columns.Count; }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return RowCount == 0 && ColumnCount == 0; }
        }

        public void AddRowsForSingleColumn(object[] values)
        {
            if (ColumnCount != 1)
                throw new InvalidOperationException("必须具有唯一的列");

            for (int k = 0; k < values.Length; k++)
            {
                AddRow(new[] { values[k] });
            }
        }
    }
}
