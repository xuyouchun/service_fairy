using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;
using Common.Utility;
using System.Data;
using System.Collections;

namespace Common.Package.Storage
{
    /// <summary>
    /// 表格
    /// </summary>
    [System.Diagnostics.DebuggerTypeProxy(typeof(StreamTableDebuggerTypeProxy))]
    [System.Diagnostics.DebuggerDisplay("Count = {RowCount}")]
    public abstract class StreamTable : IEnumerable<StreamTableRow>
    {
        internal StreamTable(string name, string desc)
        {
            Name = name;
            Desc = desc;

            _wrapper = new Lazy<Wrapper>(_LoadWrapper);
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 列信息
        /// </summary>
        public StreamTableColumn[] Columns
        {
            get { return _wrapper.Value.Columns; }
        }

        private class Wrapper
        {
            public StreamTableColumn[] Columns;
            public Dictionary<string, int> ColumnIndexDict;
        }

        private readonly Lazy<Wrapper> _wrapper;

        private Wrapper _LoadWrapper()
        {
            StreamTableColumn[] columns = GetColumns();
            Dictionary<string, int> dict = new Dictionary<string,int>();

            for (int k = 0; k < columns.Length; k++)
            {
                string columnName = columns[k].Name;

                if (dict.ContainsKey(columnName))
                    throw new InvalidOperationException(string.Format("列名“{0}”重复", columnName));

                dict.Add(columnName, k);
            }

            return new Wrapper { Columns = columns, ColumnIndexDict = dict };
        }

        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <returns></returns>
        protected abstract StreamTableColumn[] GetColumns();

        /// <summary>
        /// 行的长度，按字节统计
        /// </summary>
        internal int GetRowLength(StreamTableModelOption option)
        {
            int rowLength = 0;
            for (int k = 0; k < Columns.Length; k++)
            {
                StreamTableColumn column = Columns[k];
                rowLength += column.GetLength(option.HeapIndexType);
            }

            return rowLength;
        }

        internal int GetColumnIndex(string name)
        {
            try
            {
                return _wrapper.Value.ColumnIndexDict[name];
            }
            catch (KeyNotFoundException ex)
            {
                throw new InvalidOperationException("不存在指定的列名:" + name, ex);
            }
        }

        /// <summary>
        /// 表的描述
        /// </summary>
        public string Desc { get; private set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount
        {
            get { return OnGetRowCount(); }
        }

        /// <summary>
        /// 获取指定索引处的行
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected abstract StreamTableRow OnGetRow(int index);

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <returns></returns>
        protected abstract int OnGetRowCount();

        /// <summary>
        /// 按索引读取行
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StreamTableRow this[int index]
        {
            get { return OnGetRow(index); }
        }

        /// <summary>
        /// 按索引读取行
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StreamTableRow GetRow(int index)
        {
            return OnGetRow(index);
        }

        /// <summary>
        /// 按索引批量读取行
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public StreamTableRow[] GetRows(int start, int count)
        {
            Contract.Requires(start >= 0 && count >= 0);

            List<StreamTableRow> rows = new List<StreamTableRow>();
            for (int k = start, end = start + Math.Min(count, RowCount - start); k < end; k++)
            {
                rows.Add(GetRow(k));
            }

            return rows.ToArray();
        }

        public IEnumerator<StreamTableRow> GetEnumerator()
        {
            for (int k = 0, length = RowCount; k < length; k++)
            {
                yield return this[k];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 将StreamTable转换为DataTable
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable(Name);
            StreamTableColumn[] columns = Columns;
            foreach (StreamTableColumn column in columns)
            {
                Type type = column.GetUnderlyingType();
                dt.Columns.Add(column.Name, type);
            }

            foreach (StreamTableRow row in this)
            {
                DataRow dr = dt.NewRow();
                for (int k = 0, length = columns.Length; k < length; k++)
                {
                    StreamTableColumn column = columns[k];
                    object value = row[k];
                    IEnumerable elements;
                    if (column.IsArray() && (elements = value as IEnumerable) != null)
                    {
                        dr[k] = string.Join(",", elements.Cast<object>());
                    }
                    else
                    {
                        dr[k] = value;
                    }
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        #region Class StreamTableDebuggerTypeProxy ...

        class StreamTableDebuggerTypeProxy
        {
            public StreamTableDebuggerTypeProxy(StreamTable owner)
            {
                _owner = owner;

                Rows = owner.ToArray();
            }

            private readonly StreamTable _owner;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public StreamTableRow[] Rows;
        }

        #endregion

    }
}
