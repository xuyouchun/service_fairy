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
    /// <summary>
    /// 1.0版本
    /// </summary>
    partial class StreamTableStorage_V1 : StreamTableStorageBase
    {
        private static readonly SVersion _version = "1.0";

        public override void Write(StreamTableWriter writer, StreamTableModelOption option, Stream stream)
        {
            Writer w = new Writer(writer, option, stream);
            w.Write();
        }

        // 表头列信息
        private static StreamTableColumn[] _headerColumns = new StreamTableColumn[] {
            new StreamTableColumn("sign", StreamTableColumnType.UInt),
            new StreamTableColumn("version", StreamTableColumnType.UInt),
            new StreamTableColumn("heap_index_size", StreamTableColumnType.Byte),
            new StreamTableColumn("stack_offset", StreamTableColumnType.UInt),
            new StreamTableColumn("creation_time", StreamTableColumnType.DateTime),
            new StreamTableColumn("table_count", StreamTableColumnType.UInt),
            new StreamTableColumn("name", StreamTableColumnType.String),
            new StreamTableColumn("desc", StreamTableColumnType.String),
        };

        // 表元数据列信息
        private static StreamTableColumn[] _tableMetadataColumns = new StreamTableColumn[] {
            new StreamTableColumn("table_name", StreamTableColumnType.String),
            new StreamTableColumn("column_count", StreamTableColumnType.UInt),
            new StreamTableColumn("row_count", StreamTableColumnType.UInt),
            new StreamTableColumn("row_length", StreamTableColumnType.UInt),
            new StreamTableColumn("metadata_offset", StreamTableColumnType.UInt),
            new StreamTableColumn("data_offset", StreamTableColumnType.UInt),
            new StreamTableColumn("table_desc", StreamTableColumnType.String),
        };

        // 列元数据列信息
        private static StreamTableColumn[] _columnMetadataColumns = new StreamTableColumn[] {
            new StreamTableColumn("column_name", StreamTableColumnType.String),
            new StreamTableColumn("column_type", StreamTableColumnType.Byte),
            new StreamTableColumn("column_size_type", StreamTableColumnType.Byte),
            new StreamTableColumn("column_size", StreamTableColumnType.UInt),
            new StreamTableColumn("column_desc", StreamTableColumnType.String),
        };

        private static int _GetRowLength(StreamTableModelOption option, IEnumerable<StreamTableColumn> columns)
        {
            return columns.Sum(column => column.GetLength(option.HeapIndexType));
        }

        public override StreamTable[] Read(Stream stream, out StreamTableHeaderInfo headerInfo)
        {
            Reader r = new Reader(stream);
            return r.Read(out headerInfo);
        }

        private static readonly int _creationTimeOffset = 13;  // creation_time的起始位置
    }
}
