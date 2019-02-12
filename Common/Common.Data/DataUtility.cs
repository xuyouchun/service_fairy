using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Common.Package.Storage;
using System.Diagnostics.Contracts;
using System.Reflection;
using Common.Utility;
using FieldInfo = System.Reflection.FieldInfo;
using Common.Data.SqlExpressions;

namespace Common.Data
{
    public static class DataUtility
    {
        /// <summary>
        /// 将DataRecord中的数据赋值与指定的实体，使用字段名对应
        /// </summary>
        /// <param name="record"></param>
        /// <param name="entity"></param>
        public static void AssignToEntity(this IDataRecord record, object entity)
        {
            Contract.Requires(record != null && entity != null);
            Type t = entity.GetType();

            for (int k = 0; k < record.FieldCount; k++)
            {
                string name = record.GetName(k);
                MemberInfo[] mInfos = t.GetMember(name, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
                if (!mInfos.IsNullOrEmpty())
                {
                    PropertyInfo pInfo = mInfos[0] as PropertyInfo;
                    FieldInfo fInfo;
                    if (pInfo != null)
                    {
                        if (pInfo.CanWrite)
                        {
                            Type type = pInfo.PropertyType;
                            pInfo.SetValue(entity, record[k].ToTypeWithError(type), null);
                        }
                    }
                    else if ((fInfo = mInfos[0] as FieldInfo) != null)
                    {
                        fInfo.SetValue(entity, record[k].ToType(fInfo.FieldType));
                    }
                }
            }
        }

        /// <summary>
        /// 将单引号替换为两个单引号，防止sql攻击
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string ReviseSql(string sql)
        {
            if (sql == null)
                return "";

            return sql.Replace("'", "''");
        }

        /// <summary>
        /// 转换为实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static IEnumerable<T> Convert<T>(this IDataReader dr, Func<IDataRecord, T> converter)
        {
            Contract.Requires(dr != null && converter != null);

            while (dr.Read())
            {
                yield return converter(dr);
            }
        }

        /// <summary>
        /// 转换为实体数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static T[] ConvertToArray<T>(this IDataReader dr, Func<IDataRecord, T> converter)
        {
            return Convert(dr, converter).ToArray();
        }

        internal static bool IgnoreCaseEqualsTo(this string s, string s2)
        {
            return string.Equals(s, s2, StringComparison.OrdinalIgnoreCase);
        }

        internal static SqlExpression ToSqlExpression(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return SqlExpression.Parse(s);
        }

        private static readonly Dictionary<Type, DbColumnType> _fieldTypeDict = new Dictionary<Type, DbColumnType> {
            { typeof(DBNull), DbColumnType.DBNull }, { typeof(bool), DbColumnType.Boolean },
            { typeof(sbyte), DbColumnType.Int8 }, { typeof(byte), DbColumnType.Int8 },
            { typeof(short), DbColumnType.Int16 }, { typeof(ushort), DbColumnType.Int16 },
            { typeof(int), DbColumnType.Int32 }, { typeof(uint), DbColumnType.Int32 },
            { typeof(long), DbColumnType.Int64 }, { typeof(ulong), DbColumnType.Int64 },
            { typeof(float), DbColumnType.Single }, { typeof(double), DbColumnType.Double },
            { typeof(decimal), DbColumnType.Decimal }, { typeof(DateTime), DbColumnType.DateTime },
            { typeof(string), DbColumnType.String }, { typeof(byte[]), DbColumnType.Binary },
            { typeof(Guid), DbColumnType.Guid },
        };

        /// <summary>
        /// 转换为DbColumnType
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DbColumnType ToDbColumnType(this object value)
        {
            if (value == null)
                return DbColumnType.DBNull;

            return value.GetType().ToDbColumnType();
        }

        /// <summary>
        /// 转换为DbColumnType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DbColumnType ToDbColumnType(this Type type)
        {
            Contract.Requires(type != null);

            DbColumnType fieldType;
            if (_fieldTypeDict.TryGetValue(type, out fieldType))
                return fieldType;

            return DbColumnType.Unknown;
        }

        /// <summary>
        /// 将DbColumnType转换为.NET数据类型
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static Type ToType(this DbColumnType columnType)
        {
            switch (columnType)
            {
                case DbColumnType.Unknown:
                    return null;

                case DbColumnType.DBNull:
                    return typeof(DBNull);

                case DbColumnType.Int8:
                    return typeof(sbyte);

                case DbColumnType.Int16:
                    return typeof(short);

                case DbColumnType.Int32:
                    return typeof(int);

                case DbColumnType.Int64:
                    return typeof(long);

                case DbColumnType.Single:
                    return typeof(float);

                case DbColumnType.Double:
                    return typeof(double);

                case DbColumnType.Decimal:
                    return typeof(decimal);

                case DbColumnType.AnsiString:
                    return typeof(string);

                case DbColumnType.String:
                    return typeof(string);

                case DbColumnType.Boolean:
                    return typeof(bool);

                case DbColumnType.DateTime:
                    return typeof(DateTime);

                case DbColumnType.Guid:
                    return typeof(Guid);

                case DbColumnType.Binary:
                    return typeof(byte[]);

                default:
                    return null;
            }
        }
    }
}
