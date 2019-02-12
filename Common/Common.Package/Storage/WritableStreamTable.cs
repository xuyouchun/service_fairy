using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Package.Storage
{
    /// <summary>
    /// 支持写操作的StreamTable
    /// </summary>
    public class WritableStreamTable : StreamTable
    {
        internal WritableStreamTable(string name, StreamTableColumn[] columns, string desc)
            : base(name, desc)
        {
            _columns = columns;
        }

        private readonly StreamTableColumn[] _columns;

        protected override StreamTableColumn[] GetColumns()
        {
            return _columns;
        }

        private readonly List<StreamTableRow> _list = new List<StreamTableRow>();

        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="rowData"></param>
        public StreamTableRow AppendRow()
        {
            StreamTableRow row = StreamTableRow.Create(this, new object[Columns.Length]);
            _list.Add(row);
            return row;
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public StreamTableRow AppendRow(params object[] data)
        {
            Contract.Requires(data != null);

            if (data.Length != Columns.Length)
                throw new ArgumentException("新添加行的列数与实际列数不对应");

            StreamTableRow row = AppendRow();
            for (int k = 0; k < data.Length; k++)
            {
                row[k] = data[k];
            }
            return row;
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public StreamTableRow AppendRow(IDictionary<string, object> data)
        {
            Contract.Requires(data != null);

            StreamTableRow row = AppendRow();
            foreach (KeyValuePair<string, object> item in data)
            {
                row[item.Key] = item.Value;
            }

            return row;
        }

        /// <summary>
        /// 批量添加行
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public StreamTableRow[] AppendRows(object[][] datas)
        {
            Contract.Requires(datas != null);

            StreamTableRow[] rows = new StreamTableRow[datas.Length];
            for (int k = 0; k < datas.Length; k++)
            {
                rows[k] = AppendRow(datas[k]);
            }

            return rows;
        }

        /// <summary>
        /// 获取指定索引处的行
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override StreamTableRow OnGetRow(int index)
        {
            return _list[index];
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <returns></returns>
        protected override int OnGetRowCount()
        {
            return _list.Count;
        }
    }
}
