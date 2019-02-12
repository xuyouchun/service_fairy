using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.Runtime.Serialization;

namespace Common.Package.Storage
{
    /// <summary>
    /// 列
    /// </summary>
    [Serializable, DataContract]
    public class StreamTableColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columnType"></param>
        /// <param name="count"></param>
        /// <param name="desc"></param>
        public StreamTableColumn(string name, StreamTableColumnType columnType,
            StreamTableColumnStorageModel storageModel = StreamTableColumnStorageModel.Auto, int count = 1, string desc = "")
        {
            Contract.Requires(name != null && count >= 1);

            Name = name;
            ColumnType = columnType;
            Desc = desc;
            StorageModel = (storageModel != StreamTableColumnStorageModel.Auto) ? storageModel : (count == 1 ? StreamTableColumnStorageModel.Element : StreamTableColumnStorageModel.Array);
            ElementCount = ((StorageModel == StreamTableColumnStorageModel.Element) ? 1 : count);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columnType"></param>
        /// <param name="desc"></param>
        public StreamTableColumn(string name, StreamTableColumnType columnType, string desc)
            : this(name, columnType, StreamTableColumnStorageModel.Element, 1, desc)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columnType"></param>
        /// <param name="storageModel"></param>
        /// <param name="count"></param>
        /// <param name="desc"></param>
        public StreamTableColumn(string name, StreamTableColumnType columnType, StreamTableColumnStorageModel storageModel, string desc)
            : this(name, columnType, storageModel, 1, desc)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columnType"></param>
        /// <param name="count"></param>
        /// <param name="desc"></param>
        public StreamTableColumn(string name, StreamTableColumnType columnType, int count, string desc)
            : this(name, columnType, StreamTableColumnStorageModel.Element, count, desc)
        {

        }

        /// <summary>
        /// 列名
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 列数据类型
        /// </summary>
        [DataMember]
        public StreamTableColumnType ColumnType { get; private set; }

        /// <summary>
        /// 元素的组织形式
        /// </summary>
        [DataMember]
        public StreamTableColumnStorageModel StorageModel { get; private set; }

        /// <summary>
        /// 是否为数组
        /// </summary>
        /// <returns></returns>
        public bool IsArray()
        {
            return StorageModel == StreamTableColumnStorageModel.Array || StorageModel == StreamTableColumnStorageModel.DynamicArray;
        }

        /// <summary>
        /// 获取内置类型
        /// </summary>
        /// <returns></returns>
        public Type GetUnderlyingType()
        {
            StreamTableColumnInfo info = StreamTableColumnInfo.FromColumnType(ColumnType);
            return info.UnderlyingType;
        }

        /// <summary>
        /// 元素个数
        /// </summary>
        [DataMember]
        public int ElementCount { get; private set; }

        /// <summary>
        /// 获取该列在主体存储中的尺寸
        /// </summary>
        /// <param name="indexType"></param>
        /// <returns></returns>
        public unsafe int GetLength(IndexType indexType)
        {
            if (StorageModel == StreamTableColumnStorageModel.DynamicArray)
                return (int)indexType;

            StreamTableColumnInfo cInfo = StreamTableColumnInfo.FromColumnType(ColumnType);
            int elementSize = cInfo.GetLength(indexType);

            return elementSize * ElementCount;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Desc { get; private set; }

        public override string ToString()
        {
            string s = string.Format("{0} {1}", Name, ColumnType, StorageModel);
            if (StorageModel != StreamTableColumnStorageModel.Element)
                s += " " + StorageModel.GetDesc();

            if (StorageModel == StreamTableColumnStorageModel.Array)
                s += string.Format("[{0}]", ElementCount);

            if (!string.IsNullOrEmpty(Desc))
                s += " " + Desc;

            return s;
        }
    }
}
