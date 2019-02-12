using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.UnionTable
{

    /// <summary>
    /// 列的数据类型
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 空值
        /// </summary>
        DBNull,

        /// <summary>
        /// 8位整型
        /// </summary>
        Int8,

        /// <summary>
        /// 16整型
        /// </summary>
        Int16,

        /// <summary>
        /// 32整型
        /// </summary>
        Int32,

        /// <summary>
        /// 64位整型
        /// </summary>
        Int64,

        /// <summary>
        /// 单精度浮点
        /// </summary>
        Single,

        /// <summary>
        /// 双精度浮点
        /// </summary>
        Double,

        /// <summary>
        /// Decimal
        /// </summary>
        Decimal,

        /// <summary>
        /// 字符串
        /// </summary>
        String,

        /// <summary>
        /// 布尔型
        /// </summary>
        Boolean,

        /// <summary>
        /// 时间
        /// </summary>
        DateTime,

        /// <summary>
        /// GUID
        /// </summary>
        Guid,
    }
}
