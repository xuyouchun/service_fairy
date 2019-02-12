using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
//using Common.Data.UnionTable.MsSql;

namespace Common.Data.UnionTable
{
    public static class UnionTableUtility
    {
        /// <summary>
        /// 将完整的字段名解析成组名与字段名的形式
        /// </summary>
        /// <param name="fullFieldName"></param>
        /// <param name="group"></param>
        /// <param name="field"></param>
        public static void SplitFullFieldName(string fullFieldName, out string group, out string field)
        {
            Contract.Requires(fullFieldName != null);

            int index = fullFieldName.IndexOf('.');
            if (index >= 0)
            {
                group = fullFieldName.Substring(0, index).Trim();
                field = fullFieldName.Substring(index + 1).Trim();
            }
            else
            {
                group = string.Empty;
                field = fullFieldName.Trim();
            }
        }

        /// <summary>
        /// 获取组名
        /// </summary>
        /// <param name="fullFieldName"></param>
        /// <returns></returns>
        public static string GetGroupName(string fullFieldName)
        {
            Contract.Requires(fullFieldName != null);
            int index = fullFieldName.IndexOf('.');
            if (index >= 0)
                return fullFieldName.Substring(0, index).Trim();

            return string.Empty;
        }

        /// <summary>
        /// 获取字段名
        /// </summary>
        /// <param name="fullFieldName"></param>
        /// <returns></returns>
        public static string GetFieldName(string fullFieldName)
        {
            Contract.Requires(fullFieldName != null);
            int index = fullFieldName.IndexOf('.');
            if (index >= 0)
                return fullFieldName.Substring(index + 1).Trim();

            return fullFieldName.Trim();
        }

        /// <summary>
        /// 将组名与字段名组合在一起
        /// </summary>
        /// <param name="group"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string BuildFullFieldName(string group, string fieldName)
        {
            if (group == null || (group = group.Trim()).Length == 0)
                return fieldName;

            return group + "." + fieldName.Trim();
        }

        /// <summary>
        /// 转换为FieldType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FieldType ToFieldType(this Type type)
        {
            Contract.Requires(type != null);

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.DBNull:
                    return FieldType.DBNull;

                case TypeCode.Boolean:
                    return FieldType.Boolean;

                case TypeCode.SByte:
                case TypeCode.Byte:
                    return FieldType.Int8;

                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return FieldType.Int16;

                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return FieldType.Int32;

                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return FieldType.Int64;

                case TypeCode.Single:
                    return FieldType.Single;

                case TypeCode.Decimal:
                    return FieldType.Decimal;

                case TypeCode.Double:
                    return FieldType.Double;

                case TypeCode.DateTime:
                    return FieldType.DateTime;

                case TypeCode.String:
                    return FieldType.String;

                default:
                    return FieldType.Unknown;
            }
        }

        /// <summary>
        /// 验证数据类型是否相符
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnType"></param>
        public static void ValidateType(Type type, FieldType columnType)
        {
            Contract.Requires(type != null);

            if (type.ToFieldType() != columnType)
                throw new DbException(string.Format("数据格式错误，{0}类型与{1}不符", type, columnType));
        }

        /// <summary>
        /// 转换为MsSql值的字符串表示
        /// </summary>
        /// <param name="value"></param>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static string ReviseToMsSqlValue(object value, FieldType columnType)
        {
            if (value == null)
                return "Null";

            UnionTableUtility.ValidateType(value.GetType(), columnType);

            switch (columnType)
            {
                case FieldType.Guid:
                case FieldType.String:
                case FieldType.DateTime:
                case FieldType.Unknown:
                    return "'" + value.ToString().Replace("'", "''") + "'";

                case FieldType.Boolean:
                    return ((bool)value) ? "1" : "0";

                default:
                    return value == null ? "" : value.ToString();
            }
        }
    }
}
