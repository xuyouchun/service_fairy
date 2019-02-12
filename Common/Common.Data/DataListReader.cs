using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics.Contracts;

namespace Common.Data
{
    /// <summary>
    /// DataList读取器
    /// </summary>
    public class DataListReader : IDataReader
    {
        public DataListReader(DataList dataList)
        {
            Contract.Requires(dataList != null);

            DataList = dataList;
        }

        /// <summary>
        /// 数据列表
        /// </summary>
        public DataList DataList { get; private set; }

        private volatile bool _closed;
        private int _rowIndex = -1;

        private void _Validate()
        {
            if (_rowIndex == -1)
                throw new DbException("DataList读取器尚未开始读取");
        }

        private object _Get(int index)
        {
            _Validate();
            return DataList.GetValue(_rowIndex, index);
        }

        private object _Get(string name)
        {
            _Validate();
            return DataList.GetValue(_rowIndex, name);
        }

        private T _Get<T>(int index)
        {
            return (T)Convert.ChangeType(_Get(index), typeof(T));
        }

        private T _Get<T>(string name)
        {
            return (T)Convert.ChangeType(_Get(name), typeof(T));
        }

        private DataListRow _GetCurrentRow()
        {
            _Validate();
            return DataList.Rows[_rowIndex];
        }

        public void Close()
        {
            _closed = true;
        }

        public int Depth
        {
            get { return 1; }
        }

        public DataTable GetSchemaTable()
        {
            return new DataTable();
        }

        public bool IsClosed
        {
            get { return _closed; }
        }

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            if (_rowIndex < DataList.Rows.Length - 1)
            {
                _rowIndex++;
                return true;
            }

            return false;
        }

        public int RecordsAffected
        {
            get { return DataList.Rows.Length; }
        }

        public void Dispose()
        {
            
        }

        public int FieldCount
        {
            get
            {
                return DataList.Columns.Length;
            }
        }

        public bool GetBoolean(int i)
        {
            return _Get<bool>(i);
        }

        public byte GetByte(int i)
        {
            return _Get<byte>(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return _Get<char>(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            return null;
        }

        public string GetDataTypeName(int i)
        {
            return DataList.Columns[i].Type.ToString();
        }

        public DateTime GetDateTime(int i)
        {
            return _Get<DateTime>(i);
        }

        public decimal GetDecimal(int i)
        {
            return _Get<decimal>(i);
        }

        public double GetDouble(int i)
        {
            return _Get<double>(i);
        }

        public Type GetFieldType(int i)
        {
            return DataList.Columns[i].Type.ToType();
        }

        public float GetFloat(int i)
        {
            return _Get<float>(i);
        }

        public Guid GetGuid(int i)
        {
            return _Get<Guid>(i);
        }

        public short GetInt16(int i)
        {
            return _Get<short>(i);
        }

        public int GetInt32(int i)
        {
            return _Get<int>(i);
        }

        public long GetInt64(int i)
        {
            return _Get<long>(i);
        }

        public string GetName(int i)
        {
            return DataList.Columns[i].Name;
        }

        public int GetOrdinal(string name)
        {
            return DataList.GetColumnIndex(name);
        }

        public string GetString(int i)
        {
            return _Get<string>(i);
        }

        public object GetValue(int i)
        {
            return _Get(i);
        }

        public int GetValues(object[] values)
        {
            Contract.Requires(values != null);

            DataListRow row = _GetCurrentRow();
            int count = Math.Min(values.Length, row.Cells.Length);
            Array.Copy(row.Cells, 0, values, 0, count);

            return count;
        }

        public bool IsDBNull(int i)
        {
            object value = _Get(i);
            return value == null || value == DBNull.Value;
        }

        public object this[string name]
        {
            get { return _Get(name); }
        }

        public object this[int i]
        {
            get { return _Get(i); }
        }

        public override string ToString()
        {
            if (_rowIndex < 0)
                return base.ToString();

            return _GetCurrentRow().ToString();
        }
    }
}
