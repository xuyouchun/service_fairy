using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Reflection;

namespace Common.Data
{
    /// <summary>
    /// 列的数据类型
    /// </summary>
    public enum DbColumnType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 空值
        /// </summary>
        DBNull = 1,

        /// <summary>
        /// 8位整型
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Numerbic, 1, false)]
        Int8,

        /// <summary>
        /// 16整型
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Numerbic, 2, false)]
        Int16,

        /// <summary>
        /// 32整型
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Numerbic, 4, false)]
        Int32,

        /// <summary>
        /// 64位整型
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Numerbic, 8, false)]
        Int64,

        /// <summary>
        /// 单精度浮点
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Numerbic, 4, false)]
        Single,

        /// <summary>
        /// 双精度浮点
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Numerbic, 8, false)]
        Double,

        /// <summary>
        /// Decimal
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Numerbic, 16, false)]
        Decimal,

        /// <summary>
        /// 字符串
        /// </summary>
        [DbColumnType(DbColumnTypeCode.String, 1, true)]
        AnsiString,

        /// <summary>
        /// Unicode字符串
        /// </summary>
        [DbColumnType(DbColumnTypeCode.String, 2, true)]
        String,

        /// <summary>
        /// 布尔型
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Boolean, 1, false)]
        Boolean,

        /// <summary>
        /// 时间
        /// </summary>
        [DbColumnType(DbColumnTypeCode.DateTime, 8, false)]
        DateTime,

        /// <summary>
        /// GUID
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Guid, 16, false)]
        Guid,

        /// <summary>
        /// 二进制
        /// </summary>
        [DbColumnType(DbColumnTypeCode.Binary, 1, true)]
        Binary,
    }

    public enum DbColumnTypeCode
    {
        /// <summary>
        /// 数值类型
        /// </summary>
        Numerbic,

        /// <summary>
        /// 字符串
        /// </summary>
        String,

        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime,

        /// <summary>
        /// 布尔型
        /// </summary>
        Boolean,

        /// <summary>
        /// Guid
        /// </summary>
        Guid,

        /// <summary>
        /// 二进制
        /// </summary>
        Binary,
    }

    public class DbColumnTypeInfo
    {
        static DbColumnTypeInfo()
        {
            _dict = typeof(DbColumnType).SearchMembers<DbColumnType, DbColumnTypeInfo, DbColumnTypeAttribute>(
                (attrs, mInfo) => (DbColumnType)((FieldInfo)mInfo).GetValue(null),
                (attrs, mInfo) => new DbColumnTypeInfo() { ElementSize = attrs[0].ElementSize, TypeCode = attrs[0].TypeCode, IsVarLen = attrs[0].IsVarLen },
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        private static readonly Dictionary<DbColumnType, DbColumnTypeInfo> _dict;

        private DbColumnTypeInfo()
        {

        }

        /// <summary>
        /// 获取列类型信息
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static DbColumnTypeInfo GetTypeInfo(DbColumnType columnType)
        {
            return _dict.GetOrDefault(columnType);
        }

        /// <summary>
        /// 类型
        /// </summary>
        public DbColumnTypeCode TypeCode { get; private set; }

        /// <summary>
        /// 元素长度
        /// </summary>
        public int ElementSize { get; private set; }

        /// <summary>
        /// 是否为可变长度
        /// </summary>
        public bool IsVarLen { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class DbColumnTypeAttribute : Attribute
    {
        public DbColumnTypeAttribute(DbColumnTypeCode typeCode, int elementSize, bool isVarLen)
        {
            TypeCode = typeCode;
            ElementSize = elementSize;
            IsVarLen = isVarLen;
        }

        public DbColumnTypeCode TypeCode { get; set; }

        public int ElementSize { get; set; }

        public bool IsVarLen { get; set; }
    }
}
