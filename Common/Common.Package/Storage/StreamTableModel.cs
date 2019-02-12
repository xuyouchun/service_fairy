using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Package.Storage
{
    /// <summary>
    /// StreamTable的尺寸模型
    /// </summary>
    public enum StreamTableModel
    {
        /// <summary>
        /// 小模式
        /// </summary>
        Small = 0,

        /// <summary>
        /// 中模式
        /// </summary>
        Middle = 10,

        /// <summary>
        /// 大模式
        /// </summary>
        Large = 20,
    }

    /// <summary>
    /// 索引的类型
    /// </summary>
    public enum IndexType : byte
    {
        /// <summary>
        /// 单个字节
        /// </summary>
        Byte = 1,

        /// <summary>
        /// 双字节长度
        /// </summary>
        UShort = 2,

        /// <summary>
        /// 四个字节长度
        /// </summary>
        Int = 4,
    }

    /// <summary>
    /// 模式配置参数
    /// </summary>
    public class StreamTableModelOption
    {
        public StreamTableModelOption(uint maxHeapSize)
        {
            MaxHeapSize = maxHeapSize;
            HeapIndexType = GetIndexType(maxHeapSize);
        }

        /// <summary>
        /// 最大堆尺寸
        /// </summary>
        public uint MaxHeapSize { get; private set; }

        /// <summary>
        /// 堆索引的数据类型
        /// </summary>
        internal IndexType HeapIndexType { get; private set; }

        /// <summary>
        /// 小模式配置
        /// </summary>
        public static readonly StreamTableModelOption Small = new StreamTableModelOption(ushort.MaxValue);

        /// <summary>
        /// 大模式配置
        /// </summary>
        public static readonly StreamTableModelOption Large = new StreamTableModelOption(uint.MaxValue);

        /// <summary>
        /// 自动
        /// </summary>
        public static readonly StreamTableModelOption Auto = new StreamTableModelOption(0);

        internal static bool IsAuto(StreamTableModelOption op)
        {
            return op == null || op.MaxHeapSize == 0;
        }

        public static StreamTableModelOption GetByModel(StreamTableModel model)
        {
            switch (model)
            {
                case StreamTableModel.Small:
                    return Small;

                case StreamTableModel.Large:
                    return Large;

                default:
                    throw new NotSupportedException("未找到该模式的配置：" + model);
            }
        }

        internal static IndexType GetIndexType(uint value)
        {
            if (value <= byte.MaxValue)
                return IndexType.Byte;

            if (value <= ushort.MaxValue)
                return IndexType.UShort;

            return IndexType.Int;
        }
    }
}
