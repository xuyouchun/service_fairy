using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace Common.Package.Storage
{
    /// <summary>
    /// 
    /// </summary>
    class StreamTableHeap
    {
        public StreamTableHeap(StreamTableModelOption option)
        {
            _option = option;
            _indexType = _option.HeapIndexType;

            _heapStream.Write((int)0);       // 写入一个整形值，该四个位置将作特殊用途，其中0代表空字符串或空数组
            _curPos = (int)_heapStream.Position;
        }

        private readonly StreamTableModelOption _option;
        private readonly IndexType _indexType;
        private readonly Dictionary<string, int> _stringDict = new Dictionary<string, int>();
        private readonly MemoryStream _heapStream = new MemoryStream();
        private int _curPos = 0;

        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="bodyStream"></param>
        /// <param name="s"></param>
        public void WriteString(Stream bodyStream, string s)
        {
            _WriteString(bodyStream, s, _heapStream);
        }

        private void _WriteString(Stream bodyStream, string s, Stream heapStream)
        {
            if (string.IsNullOrEmpty(s)) // 空串都写为0处索引
            {
                bodyStream.WriteIndex(_indexType, 0);
                if (bodyStream == heapStream)
                    _curPos += (int)_indexType;
            }
            else
            {
                int pos;
                if (_stringDict.TryGetValue(s, out pos))
                {
                    bodyStream.WriteIndex(_indexType, pos);
                    if (bodyStream == heapStream)
                        _curPos += (int)_indexType;
                }
                else if (bodyStream != heapStream)
                {
                    bodyStream.WriteIndex(_indexType, _curPos);
                    _stringDict.Add(s, _curPos);
                    _curPos +=  heapStream.Write(s);
                }
                else
                {
                    bodyStream.WriteIndex(_indexType, _curPos += (int)_indexType);
                    _stringDict.Add(s, _curPos);
                    _curPos += heapStream.Write(s);
                }
            }
        }

        /// <summary>
        /// 写入动态数组
        /// </summary>
        /// <param name="bodyStream"></param>
        /// <param name="column"></param>
        /// <param name="array"></param>
        public void WriteDynamicArray(Stream bodyStream, StreamTableColumn column, object[] array)
        {
            if (array.IsNullOrEmpty())
            {
                bodyStream.WriteIndex(_indexType, 0);
                return;
            }

            bodyStream.WriteIndex(_indexType, _curPos);
            _heapStream.WriteIndex(_indexType, array.Length);  // 数组大小
            _curPos += (int)_indexType;

            if (column.ColumnType == StreamTableColumnType.Variant)  // 变长不定类型的数组
            {
                MemoryStream ms = null;
                StreamTableColumnInfo[] columnInfos = new StreamTableColumnInfo[array.Length];
                for (int k = 0; k < array.Length; k++)
                {
                    StreamTableColumnInfo columnInfo = _GetColumnInfoOfObjectType(array[k].GetType());
                    columnInfos[k] = columnInfo;
                    _curPos += columnInfo.GetLength(_indexType) + 1;
                }

                for (int k = 0; k < array.Length; k++)
                {
                    object data = array[k];
                    StreamTableColumnType columnType = columnInfos[k].ColumnType;
                    _heapStream.WriteByte((byte)columnType);
                    if (columnType == StreamTableColumnType.String)
                    {
                        _WriteString(_heapStream, (string)array[k], ms ?? (ms = new MemoryStream()));
                    }
                    else
                    {
                        _heapStream.WriteData(columnType, data);
                    }
                }

                if (ms != null)
                    _heapStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
            }
            else  // 变长普通类型数组
            {
                if (column.ColumnType == StreamTableColumnType.String)  // 字符串数组
                {
                    _curPos += (int)_option.HeapIndexType * array.Length;
                    MemoryStream ms = new MemoryStream();
                    for (int k = 0; k < array.Length; k++)
                    {
                        _WriteString(_heapStream, (string)array[k], ms);
                    }

                    _heapStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
                }
                else        // 其它数据类型的数组
                {
                    StreamTableColumnInfo columnInfo = StreamTableColumnInfo.FromColumnType(column.ColumnType);
                    for (int k = 0; k < array.Length; k++)
                    {
                        _heapStream.WriteData(column.ColumnType, array[k]);
                    }
                    _curPos += (columnInfo.Size * array.Length);
                }
            }
        }

        /// <summary>
        /// 写入不定类型的数据
        /// </summary>
        /// <param name="bodyStream"></param>
        /// <param name="data"></param>
        public void WriteVarType(Stream bodyStream, object data)
        {
            if (data == null)
            {
                bodyStream.WriteByte((byte)StreamTableColumnType.Variant);
                bodyStream.WriteIndex(_indexType, 0);
                return;
            }

            StreamTableColumnType columnType = _GetColumnInfoOfObjectType(data.GetType()).ColumnType;
            bodyStream.Write((byte)columnType);
            bodyStream.WriteIndex(_indexType, _curPos);

            if (columnType == StreamTableColumnType.String)
                _WriteString(_heapStream, (string)data, _heapStream);
            else
                _heapStream.WriteData(columnType, data);

            _curPos = (int)_heapStream.Position;
        }

        public void CopyToStream(Stream stream)
        {
            Contract.Requires(stream != null);
            stream.WriteIndex(_indexType, (int)_heapStream.Length);
            stream.Write(_heapStream.GetBuffer(), 0, (int)_heapStream.Length);
        }

        private StreamTableColumnInfo _GetColumnInfoOfObjectType(Type type)
        {
            StreamTableColumnInfo columnInfo = StreamTableColumnInfo.FromObjectType(type);
            if (columnInfo.ColumnType == StreamTableColumnType.Variant)
                throw new NotSupportedException("不支持数据类型：" + type);

            return columnInfo;
        }
    }
}
