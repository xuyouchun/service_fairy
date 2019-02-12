using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;

namespace Common.Package.Storage
{
    /// <summary>
    /// 头部信息元数据
    /// </summary>
    class StreamTableHeaderMetadata
    {
        /// <summary>
        /// 签名
        /// </summary>
        public uint Sign { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public SVersion Version { get; set; }

        /// <summary>
        /// 堆索引类型
        /// </summary>
        public IndexType IndexType { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 表格数量
        /// </summary>
        public int TableCount { get; set; }

        /// <summary>
        /// 堆起始位置
        /// </summary>
        public int HeapOffset { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        public StreamTableHeaderInfo ToHeaderInfo()
        {
            return new StreamTableHeaderInfo(Version, CreationTime, TableCount, Name, Desc);
        }
    }

    /// <summary>
    /// 表格元数据
    /// </summary>
    class StreamTableMetadata
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// 行长度
        /// </summary>
        public int RowLength { get; set; }

        /// <summary>
        /// 元数据偏移位置
        /// </summary>
        public int MetadataOffset { get; set; }

        /// <summary>
        /// 数据偏移位置
        /// </summary>
        public int DataOffset { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
    }

    /// <summary>
    /// 列的元数据
    /// </summary>
    class StreamTableColumnMetadata
    {
        /// <summary>
        /// 列名 
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// 列类型
        /// </summary>
        public StreamTableColumnType ColumnType { get; private set; }

        /// <summary>
        /// 存储类型
        /// </summary>
        public StreamTableColumnStorageModel StorageModel { get; private set; }

        /// <summary>
        /// 列描述
        /// </summary>
        public string Desc { get; private set; }
    }
}
