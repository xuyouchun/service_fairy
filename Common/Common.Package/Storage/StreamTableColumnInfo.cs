using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace Common.Package.Storage
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class StreamTableColumnInfo
    {
        internal StreamTableColumnInfo(StreamTableColumnType columnType, int size, Type underlyingType, bool isRefType)
        {
            ColumnType = columnType;
            Size = size;
            UnderlyingType = underlyingType;
            IsRefType = isRefType;
        }

        /// <summary>
        /// 列字段类型
        /// </summary>
        public StreamTableColumnType ColumnType { get; private set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 内置类型
        /// </summary>
        public Type UnderlyingType { get; private set; }

        /// <summary>
        /// 是否为固定长度的数组
        /// </summary>
        public bool IsRefType { get; private set; }

        /// <summary>
        /// 获取该字段类型的长度
        /// </summary>
        /// <param name="indexType"></param>
        /// <returns></returns>
        public int GetLength(IndexType indexType)
        {
            if (ColumnType == StreamTableColumnType.Variant)
                return (int)indexType + 1;

            if (IsRefType)
                return (int)indexType;

            return Size;
        }

        /// <summary>
        /// 从列类型创建
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static StreamTableColumnInfo FromColumnType(StreamTableColumnType columnType)
        {
            StreamTableColumnInfo info = StreamTableColumnAttribute.Infos[(int)columnType];
            if (info == null)
                throw new NotSupportedException("未知的列类型：" + columnType);

            return info;
        }

        /// <summary>
        /// 从对象类型对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static StreamTableColumnInfo FromObjectType(Type type)
        {
            Contract.Requires(type != null);

            StreamTableColumnInfo columnInfo;
            if (StreamTableColumnAttribute.Dict.TryGetValue(type, out columnInfo))
                return columnInfo;

            return FromColumnType(StreamTableColumnType.Variant);
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    class StreamTableColumnAttribute : Attribute
    {
        static StreamTableColumnAttribute()
        {
            Type t = typeof(StreamTableColumnType);
            int max = Enum.GetValues(t).Cast<byte>().Max();
            Infos = new StreamTableColumnInfo[max + 1];
            foreach (FieldInfo fInfo in t.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                StreamTableColumnAttribute attr = fInfo.GetAttribute<StreamTableColumnAttribute>();
                if (attr != null)
                {
                    byte value = (byte)fInfo.GetValue(null);
                    StreamTableColumnInfo columnInfo = new StreamTableColumnInfo((StreamTableColumnType)value, attr.Size, attr.UnderlyingType, attr.IsRefType);
                    Infos[value] = columnInfo;
                    Dict.Add(attr.UnderlyingType, columnInfo);
                }
            }
        }

        public StreamTableColumnAttribute(int size, Type underlyingType, bool isRefType = false)
        {
            Size = size;
            UnderlyingType = underlyingType;
            IsRefType = isRefType;
        }

        public StreamTableColumnAttribute(Type underlyingType, bool isRefType)
            : this(0, underlyingType, isRefType)
        {

        }

        /// <summary>
        /// 尺寸
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 真实的数据类型
        /// </summary>
        public Type UnderlyingType { get; private set; }

        /// <summary>
        /// 是否为固定长度的数组
        /// </summary>
        public bool IsRefType { get; private set; }

        internal static readonly StreamTableColumnInfo[] Infos;

        internal static readonly Dictionary<Type, StreamTableColumnInfo> Dict = new Dictionary<Type, StreamTableColumnInfo>();
    }

    /// <summary>
    /// 列的类型
    /// </summary>
    public enum StreamTableColumnType : byte
    {
        /// <summary>
        /// 整型
        /// </summary>
        [StreamTableColumn(4, typeof(System.Int32))]
        Int = TypeCode.Int32,

        /// <summary>
        /// 无符号整型
        /// </summary>
        [StreamTableColumn(4, typeof(System.UInt32))]
        UInt = TypeCode.UInt32,

        /// <summary>
        /// 短整型
        /// </summary>
        [StreamTableColumn(2, typeof(System.Int16))]
        Short = TypeCode.Int16,

        /// <summary>
        /// 无符号短整型
        /// </summary>
        [StreamTableColumn(2, typeof(System.UInt16))]
        UShort = TypeCode.UInt16,

        /// <summary>
        /// 字节
        /// </summary>
        [StreamTableColumn(1, typeof(System.Byte))]
        Byte = TypeCode.Byte,

        /// <summary>
        /// 有符号字节
        /// </summary>
        [StreamTableColumn(1, typeof(System.SByte))]
        SByte = TypeCode.SByte,

        /// <summary>
        /// 长整型
        /// </summary>
        [StreamTableColumn(8, typeof(System.Int64))]
        Long = TypeCode.Int64,

        /// <summary>
        /// 无符号长整型
        /// </summary>
        [StreamTableColumn(8, typeof(System.UInt64))]
        ULong = TypeCode.UInt64,

        /// <summary>
        /// UNICODE字节
        /// </summary>
        [StreamTableColumn(2, typeof(System.Char))]
        Char = TypeCode.Char,

        /// <summary>
        /// 布尔型
        /// </summary>
        [StreamTableColumn(1, typeof(System.Boolean))]
        Boolean = TypeCode.Boolean,

        /// <summary>
        /// 双精度浮点
        /// </summary>
        [StreamTableColumn(8, typeof(System.Double))]
        Double = TypeCode.Double,

        /// <summary>
        /// Decimal
        /// </summary>
        [StreamTableColumn(16, typeof(System.Decimal))]
        Decimal = TypeCode.Decimal,

        /// <summary>
        /// 单精度浮点
        /// </summary>
        [StreamTableColumn(4, typeof(System.Single))]
        Float = TypeCode.Single,

        /// <summary>
        /// GUID
        /// </summary>
        [StreamTableColumn(16, typeof(System.Guid))]
        Guid = 32,

        /// <summary>
        /// 不定数据类型，可以存储任意数据
        /// </summary>
        [StreamTableColumn(typeof(System.Object), true)]
        Variant = TypeCode.Object,

        /// <summary>
        /// 时间
        /// </summary>
        [StreamTableColumn(8, typeof(System.DateTime))]
        DateTime = TypeCode.DateTime,

        /// <summary>
        /// 字符串
        /// </summary>
        [StreamTableColumn(typeof(System.String), true)]
        String = TypeCode.String,
    }
}
