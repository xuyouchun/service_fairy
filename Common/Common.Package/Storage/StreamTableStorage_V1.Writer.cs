using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;
using Common.Contracts;
using System.Collections;

namespace Common.Package.Storage
{
	partial class StreamTableStorage_V1
	{
        class Writer
        {
            public Writer(StreamTableWriter writer, StreamTableModelOption option, Stream stream)
            {
                _writer = writer;
                _storageStream = stream;
                _stream = new MemoryStream();
                _option = option;
                _heap = new StreamTableHeap(option);
                _tables = _writer.GetTables();

                _headerRowLength = _GetRowLength(option, _headerColumns);
                _tableMetadataRowLength = _GetRowLength(option, _tableMetadataColumns);
                _columnMetadataRowLength = _GetRowLength(option, _columnMetadataColumns);
            }

            private readonly StreamTableWriter _writer;
            private readonly StreamTableModelOption _option;
            private readonly StreamTable[] _tables;
            private readonly Stream _storageStream;
            private readonly MemoryStream _stream;
            private readonly StreamTableHeap _heap;
            private readonly Dictionary<StreamTable, StreamTableInfo> _tableInfoDict = new Dictionary<StreamTable, StreamTableInfo>();
            private int _tableMetadataOffset, _columnMetadataOffset, _dataOffset, _heapOffset;
            private readonly int _headerRowLength, _tableMetadataRowLength, _columnMetadataRowLength;

            #region Class StreamTableInfo ...

            class StreamTableInfo
            {
                public int MetaDataOffset, DataOffset;
            }

            #endregion

            public void Write()
            {
                _Init();

                _WriteHeader();
                _WriteTablesMetadata();
                _WriteColumnsMetadata();
                _WriteTablesData();

                _stream.Position = 0;
                _stream.CopyTo(_storageStream);

                _WriteHeap();
            }

            private void _Init()
            {
                _tableMetadataOffset = _headerRowLength;
                _columnMetadataOffset = _tableMetadataOffset + _tableMetadataRowLength * _tables.Length;
                _dataOffset = checked(_columnMetadataOffset + _tables.Sum(table => (table.Columns.Length * _columnMetadataRowLength)));
                _heapOffset = checked(_dataOffset + _tables.Sum(table => (table.RowCount * table.GetRowLength(_option))));

                int metaDataOffset = _columnMetadataOffset, dataOffset = _dataOffset;
                foreach (StreamTable table in _tables)
                {
                    _tableInfoDict.Add(table, new StreamTableInfo() { MetaDataOffset = metaDataOffset, DataOffset = dataOffset });
                    metaDataOffset += checked(table.Columns.Length * _columnMetadataRowLength);
                    dataOffset += checked(table.GetRowLength(_option) * table.RowCount);
                }
            }

            // 写入头部元数据
            private void _WriteHeader()
            {
                _stream.Write(StreamTableSettings.SIGN);  // 标识
                _stream.Write((uint)_version);            // 版本号
                _stream.Write((byte)_option.HeapIndexType);  // 堆尺寸索引数据类型长度
                _stream.Write(_heapOffset);              // 堆偏移
                _stream.Write(DateTime.UtcNow);           // 时间
                _stream.Write(_tables.Length);            // 表数量
                _WriteString(_writer.Name);               // 名称
                _WriteString(_writer.Desc);               // 备注
            }

            // 写入表格元数据
            private void _WriteTablesMetadata()
            {
                foreach (StreamTable table in _tables)
                {
                    _WriteTableMetadata(table);
                }
            }

            // 写入表格元数据
            private void _WriteTableMetadata(StreamTable table)
            {
                StreamTableInfo tInfo = _tableInfoDict[table];

                _WriteString(table.Name);
                _stream.Write(table.Columns.Length);
                _stream.Write(table.RowCount);
                _stream.Write(table.GetRowLength(_option));
                _stream.Write(tInfo.MetaDataOffset);
                _stream.Write(tInfo.DataOffset);
                
                _WriteString(table.Desc);
            }

            // 写入列元数据
            private void _WriteColumnsMetadata()
            {
                foreach (StreamTable table in _tables)
                {
                    _WriteColumnMetadata(table);
                }
            }

            // 写入列元数据
            private void _WriteColumnMetadata(StreamTable table)
            {
                foreach (StreamTableColumn column in table.Columns)
                {
                    _WriteColumnMetadata(column);
                }
            }

            // 写入列元数据
            private void _WriteColumnMetadata(StreamTableColumn column)
            {
                _WriteString(column.Name);
                _stream.WriteByte((byte)column.ColumnType);
                _stream.WriteByte((byte)column.StorageModel);
                _stream.Write(column.ElementCount);
                _WriteString(column.Desc);
            }

            // 写入表数据
            private void _WriteTablesData()
            {
                foreach (StreamTable table in _tables)
                {
                    _WriteTableData(table);
                }
            }

            // 写入表数据
            private void _WriteTableData(StreamTable table)
            {
                foreach (StreamTableRow row in table)
                {
                    _WriteTableDataRow(table, row);
                }
            }

            // 写入行数据
            private void _WriteTableDataRow(StreamTable table, StreamTableRow row)
            {
                StreamTableColumn[] columns = table.Columns;
                for (int k = 0, length = columns.Length; k < length; k++)
                {
                    StreamTableColumn column = columns[k];
                    object data = row[k];
                    _WriteTableDataCell(column, data);
                }
            }

            // 写入单元格数据
            private void _WriteTableDataCell(StreamTableColumn column, object data)
            {
                switch (column.StorageModel)
                {
                    case StreamTableColumnStorageModel.Element:
                        _WriteTableDataCellElement(column, data);
                        break;

                    case StreamTableColumnStorageModel.Array:
                        _WriteTableDataCellElementArray(column, data);
                        break;

                    case StreamTableColumnStorageModel.DynamicArray:
                        _WriteTableDataCellElementDynamicArray(column, data);
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }

            // 写入一个单元格数据
            private void _WriteTableDataCellElement(StreamTableColumn column, object data)
            {
                if (column.ColumnType == StreamTableColumnType.String)
                {
                    _heap.WriteString(_stream, (string)data);
                }
                else if (column.ColumnType == StreamTableColumnType.Variant)
                {
                    _heap.WriteVarType(_stream, data);
                }
                else
                {
                    StreamTableUtility.WriteData(_stream, column.ColumnType, data);
                }
            }

            // 写入单元格数组数据
            private void _WriteTableDataCellElementArray(StreamTableColumn column, object data)
            {
                IEnumerable elements = data as IEnumerable;
                int count = column.ElementCount;
                if (elements != null)
                {
                    foreach (object e in elements)
                    {
                        if (count-- <= 0)
                            return;

                        _WriteTableDataCellElement(column, e);
                    }
                }

                StreamTableColumnInfo cInfo = StreamTableColumnInfo.FromColumnType(column.ColumnType);
                object def = ReflectionUtility.GetDefaultValue(cInfo.UnderlyingType);
                for (int k = 0; k < count; k++)
                {
                    _WriteTableDataCellElement(column, def);
                }
            }

            // 写入单元格动态数组数据
            private void _WriteTableDataCellElementDynamicArray(StreamTableColumn column, object data)
            {
                IEnumerable elements = data as IEnumerable;
                object[] array = (elements == null) ? null : elements.Cast<object>().ToArray();
                _heap.WriteDynamicArray(_stream, column, array);
            }

            private void _WriteHeap()
            {
                _heap.CopyToStream(_storageStream);
            }

            private void _WriteIndex(int index)
            {
                _stream.WriteIndex(_option.HeapIndexType, index);
            }

            private void _WriteString(string s)
            {
                _heap.WriteString(_stream, s);
            }
        }
	}
}
