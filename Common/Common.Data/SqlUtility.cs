using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Data.UnionTable;

namespace Common.Data
{
    static class SqlUtility
    {
        /// <summary>
        /// 连接等于判断的SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="columnName"></param>
        /// <param name="columnType"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static StringBuilder AppendEqualsValues(this StringBuilder sql, string columnName, DbColumnType columnType, object[] values, string defaultGroupName = null)
        {
            if (values.IsNullOrEmpty())
                return sql;

            if (values.Length == 1)
                return sql.AppendColumnName(columnName, defaultGroupName).Append(" = ").AppendValue(values[0], columnType);

            sql.AppendColumnName(columnName).Append(" In (");
            for (int k = 0; k < values.Length; k++)
            {
                if (k > 0)
                    sql.Append(", ");

                sql.AppendValue(values[k], columnType);
            }
            sql.Append(")");
            return sql;
        }

        /// <summary>
        /// 连接列名
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="fullColumnName"></param>
        public static StringBuilder AppendColumnName(this StringBuilder sql, string fullColumnName, string defaultGroupName = null)
        {
            return sql.Append(ReviseFullColumnName(fullColumnName, defaultGroupName));
        }

        /// <summary>
        /// 添加到相等的判断条件
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <param name="columnType"></param>
        /// <param name="defaultGroup"></param>
        /// <returns></returns>
        public static StringBuilder AppendEqualsCondition(this StringBuilder sql, string columnName, object value, DbColumnType columnType, string defaultGroup = null)
        {
            sql.AppendColumnName(columnName, defaultGroup);
            if (value == null || value is DBNull)
                sql.Append(" Is ");
            else
                sql.Append(" = ");

            sql.AppendValue(value, columnType);
            return sql;
        }

        /// <summary>
        /// 修正列名为SQL语句的形式，将全名"GroupName.ColumnName"修正为"[GroupName].[ColumnName]"
        /// </summary>
        /// <param name="fullColumnName"></param>
        /// <returns></returns>
        public static string ReviseFullColumnName(string fullColumnName, string defaultGroup = null)
        {
            string group, column;
            UtUtility.SplitFullColumnName(fullColumnName, out group, out column);

            if (string.IsNullOrEmpty(group) && string.IsNullOrEmpty(group = defaultGroup))
                return "[" + column + "]";
            else
                return "[" + group + "].[" + column + "]";
        }

        /// <summary>
        /// 转换为MsSql值的字符串表示
        /// </summary>
        /// <param name="value"></param>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static string ReviseToMsSqlValue(object value, DbColumnType columnType)
        {
            if (value == null)
                return "Null";

            switch (columnType)
            {
                case DbColumnType.Guid:
                case DbColumnType.AnsiString:
                case DbColumnType.String:
                case DbColumnType.DateTime:
                case DbColumnType.Unknown:
                    return "'" + value.ToString().Replace("'", "''") + "'";

                case DbColumnType.Boolean:
                    return ((bool)value) ? "1" : "0";

                default:
                    return value == null ? "" : value.ToString();
            }
        }

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="value"></param>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static StringBuilder AppendValue(this StringBuilder sql, object value, DbColumnType columnType)
        {
            return sql.Append(ReviseToMsSqlValue(value, columnType));
        }

        /// <summary>
        /// 转换为MsSql值的字符串表示
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReviseToMsSqlValue(object value)
        {
            return ReviseToMsSqlValue(value, value == null ? DbColumnType.DBNull : value.GetType().ToDbColumnType());
        }

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static StringBuilder AppendValue(this StringBuilder sql, object value)
        {
            return sql.Append(ReviseToMsSqlValue(value));
        }

        /// <summary>
        /// 连接sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        internal static void AppendSqlFormat(this StringBuilder sql, string format, params object[] args)
        {
            sql.AppendFormat(format, args.ToArray(arg => ReviseToMsSqlValue(arg)));
        }
    }
}
