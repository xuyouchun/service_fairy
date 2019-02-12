using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Package.Storage
{
    /// <summary>
    /// 只读的StreamTable
    /// </summary>
    class ReadOnlyStreamTable : StreamTable
    {
        internal ReadOnlyStreamTable(StreamTableMetadata metadata, StreamTableBuffer bodyBuffer, StreamTableHeapReader heapReader, IndexType indexType)
            : base(metadata.TableName, metadata.Desc)
        {
            _metadata = metadata;
            _bodyBuffer = bodyBuffer;
            _heapReader = heapReader;

            _columns = new Lazy<StreamTableColumn[]>(_LoadColumns);
            _rows = new StreamTableRow[metadata.RowCount];
            _indexType = indexType;
        }

        private readonly StreamTableMetadata _metadata;
        private readonly StreamTableBuffer _bodyBuffer;
        private readonly StreamTableHeapReader _heapReader;
        private readonly IndexType _indexType;
        private readonly Lazy<StreamTableColumn[]> _columns;
        private readonly StreamTableRow[] _rows;

        // 加载所有的列
        private StreamTableColumn[] _LoadColumns()
        {
            StreamTableColumn[] columns = new StreamTableColumn[_metadata.ColumnCount];
            _bodyBuffer.Position = _metadata.MetadataOffset;

            for (int k = 0; k < columns.Length; k++)
            {
                StreamTableColumn column = new StreamTableColumn(
                    _heapReader.ReadString(_bodyBuffer),
                    (StreamTableColumnType)_bodyBuffer.ReadByte(),
                    (StreamTableColumnStorageModel)_bodyBuffer.ReadByte(),
                    _bodyBuffer.ReadInt32(),
                    _heapReader.ReadString(_bodyBuffer)
                );

                columns[k] = column;
            }

            return columns;
        }

        // 加载指定的行
        private StreamTableRow _LoadRow(int index)
        {
            StreamTableColumn[] columns = Columns;
            _bodyBuffer.Position = _metadata.DataOffset + (_metadata.RowLength * index);
            object[] data = new object[columns.Length];

            for (int k = 0, length = columns.Length; k < length; k++)
            {
                StreamTableColumn column = columns[k];
                data[k] = _ReadCell(column);
            }

            return StreamTableRow.Create(this, data);
        }

        private object _ReadCell(StreamTableColumn column)
        {
            switch (column.StorageModel)
            {
                case StreamTableColumnStorageModel.Element:
                    return _ReadCellData(column.ColumnType);

                case StreamTableColumnStorageModel.Array:
                    return _ReadCellDataArray(column);

                case StreamTableColumnStorageModel.DynamicArray:
                    return _ReadCellDataDynamicArray(column);
            }

            throw new NotSupportedException("不支持类型:" + column.StorageModel);
        }

        // 读取表格数据
        private object _ReadCellData(StreamTableColumnType columnType)
        {
            if (columnType == StreamTableColumnType.String)
            {
                return _heapReader.ReadString(_bodyBuffer);
            }
            else if (columnType == StreamTableColumnType.Variant)  // 不定类型
            {
                return _heapReader.ReadVarType(_bodyBuffer);
            }
            else
            {
                return _bodyBuffer.ReadData(columnType);
            }
        }

        // 读取表格数组数据
        private object _ReadCellDataArray(StreamTableColumn column)
        {
            StreamTableColumnInfo colInfo = StreamTableColumnInfo.FromColumnType(column.ColumnType);
            Array array = Array.CreateInstance(colInfo.UnderlyingType, (int)column.ElementCount);
            for (int k = 0; k < column.ElementCount; k++)
            {
                array.SetValue(_ReadCellData(column.ColumnType), k);
            }

            return array;
        }

        // 读取表格动态数组数据
        private object _ReadCellDataDynamicArray(StreamTableColumn column)
        {
            int position = _bodyBuffer.ReadIndex(_indexType);
            if (position == 0)
                return Array.CreateInstance(column.GetUnderlyingType(), 0);

            int count = _heapReader.ReadIndex(position);
            position += (int)_indexType;

            Array array = Array.CreateInstance(column.GetUnderlyingType(), count);
            for (int k = 0; k < count; k++)
            {
                array.SetValue(_ReadHeapData(column.ColumnType, ref position), k);
            }

            return array;
        }

        // 读取堆数据
        private object _ReadHeapData(StreamTableColumnType columnType, ref int offset)
        {
            if (columnType == StreamTableColumnType.String)  // 字符串
            {
                int position = _heapReader.ReadIndex(offset);
                offset += (int)_indexType;
                return _heapReader.ReadString(position);
            }
            else if(columnType == StreamTableColumnType.Variant)  // 变长类型
            {
                StreamTableColumnType colType = (StreamTableColumnType)_heapReader.ReadByte(offset++);
                if (colType == StreamTableColumnType.String)
                {
                    int position = _heapReader.ReadIndex(offset);
                    offset += (int)_indexType;
                    return _heapReader.ReadString(position);
                }
                else
                {
                    return _heapReader.ReadData(colType, ref offset);
                }
            }
            else    // 普通数据类型
            {
                return _heapReader.ReadData(columnType, ref offset);
            }
        }

        private readonly List<StreamTableRow> _list = new List<StreamTableRow>();

        // 获取指定位置的行
        protected override StreamTableRow OnGetRow(int index)
        {
            return _rows[index] ?? (_rows[index] = _LoadRow(index));
        }

        // 获取所有的列
        protected override StreamTableColumn[] GetColumns()
        {
            return _columns.Value;
        }

        // 获取行数
        protected override int OnGetRowCount()
        {
            return _metadata.RowCount;
        }
    }
}
