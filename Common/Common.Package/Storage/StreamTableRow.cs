using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Collections;
using System.Diagnostics.Contracts;

namespace Common.Package.Storage
{
    /// <summary>
    /// 行
    /// </summary>
    [System.Diagnostics.DebuggerTypeProxy(typeof(StreamTableRowDebuggerTypeProxy))]
    [System.Diagnostics.DebuggerDisplay("Count = {_GetData().Count}")]
    public abstract class StreamTableRow
    {
        internal StreamTableRow(StreamTable table)
        {
            _table = table;
        }

        private readonly StreamTable _table;
        private IList<object> _data;

        private IList<object> _GetData()
        {
            return _data ?? (_data = GetData());
        }

        /// <summary>
        /// 获取行的数据
        /// </summary>
        /// <returns></returns>
        protected abstract IList<object> GetData();

        private int _GetIndex(string name)
        {
            return _table.GetColumnIndex(name);
        }

        /// <summary>
        /// 列数
        /// </summary>
        public int Count
        {
            get { return _GetData().Count; }
        }

        private object _GetDataAtIndex(int index)
        {
            return _GetData()[index];
        }

        private void _SetDataAtIndex(int index, object data)
        {
            _GetData()[index] = data;
        }

        /// <summary>
        /// 按索引读写字段值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                return _GetData()[index];
            }
            set
            {
                StreamTableColumn column = _table.Columns[index];
                StreamTableColumnInfo columnInfo = StreamTableColumnInfo.FromColumnType(column.ColumnType);

                if (column.StorageModel == StreamTableColumnStorageModel.Element)  // 单个元素
                {
                    if (value == null)
                    {
                        _SetDataAtIndex(index, ReflectionUtility.GetDefaultValue(columnInfo.UnderlyingType));
                    }
                    else if (value.GetType().IsArray)
                    {
                        throw new FormatException(string.Format("字段类型{0}不可以接受数组格式的数据", column.StorageModel));
                    }
                    else if (value.GetType() != columnInfo.UnderlyingType)
                    {
                        _SetDataAtIndex(index, _Convert(value, columnInfo.UnderlyingType));
                    }
                    else
                    {
                        _SetDataAtIndex(index, value);
                    }
                }
                else if (column.StorageModel == StreamTableColumnStorageModel.Array || column.StorageModel == StreamTableColumnStorageModel.DynamicArray)  // 数组与动态数组
                {
                    Type t;
                    if (value == null)
                    {
                        _SetDataAtIndex(index, null);
                    }
                    else if ((t = value.GetType()).IsArray && t.GetElementType() == columnInfo.UnderlyingType)
                    {
                        _SetDataAtIndex(index, value);
                    }
                    else
                    {
                        _SetDataAtIndex(index, _ConvertToArray(value, columnInfo.UnderlyingType));
                    }
                }
                else
                {
                    throw new NotSupportedException("不支持的存储类型：" + column.StorageModel);
                }
            }
        }

        private object _ConvertToArray(object value, Type type)
        {
            IList list = value as IList;
            IEnumerable elements;

            if (list != null)
            {
                object[] arr = new object[list.Count];
                for (int k = 0; k < arr.Length; k++)
                {
                    arr[k] = _Convert(list[k], type);
                }
                return arr;
            }
            else if ((elements = value as IEnumerable) != null)
            {
                List<object> arr = new List<object>();
                foreach (object item in elements)
                {
                    arr.Add(_Convert(item, type));
                }
                return arr.ToArray();
            }
            else
            {
                throw new FormatException("该字段必须为数组或其它集合格式");
            }
        }

        private object _Convert(object value, Type type)
        {
            object result;
            if (ConvertUtility.TryConvert(value, type, out result))
                return result;

            throw new FormatException("无法将“" + value + "”转换为指定的类型:" + type);
        }

        /// <summary>
        /// 按列名读写字段值
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object this[string columnName]
        {
            get { return this[_GetIndex(columnName)]; }
            set { this[_GetIndex(columnName)] = value; }
        }

        /// <summary>
        /// 根据列名读取字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public T Get<T>(string columnName)
        {
            return (T)_Convert(this[columnName], typeof(T));
        }

        /// <summary>
        /// 根据列索引读取字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public T Get<T>(int columnIndex)
        {
            return (T)_Convert(this[columnIndex], typeof(T));
        }

        /// <summary>
        /// 转换为哈希表
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            StreamTableColumn[] cols = _table.Columns;
            for (int k = 0; k < cols.Length; k++)
            {
                StreamTableColumn col = cols[k];
                dict.Add(col.Name, this[k]);
            }

            return dict;
        }

        public override string ToString()
        {
            return string.Join(", ", _GetData());
        }

        internal static StreamTableRow Create(StreamTable table, IList<object> datas)
        {
            return new DefaultStreamTableRow(table, datas);
        }

        #region Class DefaultStreamTableRow ...

        class DefaultStreamTableRow : StreamTableRow
        {
            public DefaultStreamTableRow(StreamTable table, IList<object> datas)
                : base(table)
            {
                _datas = datas;
            }

            private readonly IList<object> _datas;

            protected override IList<object> GetData()
            {
                return _datas;
            }
        }

        #endregion

        class StreamTableRowDebuggerTypeProxy
        {
            public StreamTableRowDebuggerTypeProxy(StreamTableRow owner)
            {
                _owner = owner;

                StreamTableColumn[] columns = _owner._table.Columns;
                for (int k = 0; k < columns.Length; k++)
                {
                    StreamTableColumn column = columns[k];
                    object data = _owner[k];
                    string valueStr = column.IsArray() ? string.Join(",", ((IEnumerable)data).Cast<object>())
                        : data.ToString();

                    valueStr += " [" + column.ColumnType;
                    if (column.StorageModel != StreamTableColumnStorageModel.Element)
                        valueStr += " " + column.StorageModel;
                    valueStr += "]";
                    Values.Add(string.Format("{0}: {1}", column.Name, valueStr));
                }
            }

            private readonly StreamTableRow _owner;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public readonly List<string> Values = new List<string>();
        }

        /// <summary>
        /// 获取值的数组
        /// </summary>
        /// <returns></returns>
        public object[] GetValueArray()
        {
            return _GetData().ToArray();
        }
    }
}
