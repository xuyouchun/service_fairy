using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Data.UnionTable.MsSql;
using Common.Data.SqlExpressions;
using System.Collections;
using Common.Data.UnionTable.Metadata;
using Common.Collection;

namespace Common.Data.UnionTable
{
    public static class UtUtility
    {
        /// <summary>
        /// 将完整的字段名解析成组名与字段名的形式
        /// </summary>
        /// <param name="fullColumnName"></param>
        /// <param name="group"></param>
        /// <param name="column"></param>
        public static void SplitFullColumnName(string fullColumnName, out string group, out string column)
        {
            Contract.Requires(fullColumnName != null);

            int index = fullColumnName.IndexOf('.');
            if (index >= 0)
            {
                group = fullColumnName.Substring(0, index).Trim();
                column = fullColumnName.Substring(index + 1).Trim();
            }
            else
            {
                group = string.Empty;
                column = fullColumnName.Trim();
            }
        }

        /// <summary>
        /// 获取组名
        /// </summary>
        /// <param name="fullColumnName"></param>
        /// <returns></returns>
        public static string GetGroupName(string fullColumnName)
        {
            Contract.Requires(fullColumnName != null);
            int index = fullColumnName.IndexOf('.');
            if (index >= 0)
                return fullColumnName.Substring(0, index).Trim();

            return string.Empty;
        }

        /// <summary>
        /// 从列名中提取组名
        /// </summary>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        internal static string[] GetGroupNames(string[] columnNames)
        {
            if (columnNames.IsNullOrEmpty())
                return Array<string>.Empty;

            HashSet<string> hs = new HashSet<string>(IgnoreCaseEqualityComparer.Instance);
            for (int k = 0; k < columnNames.Length; k++)
            {
                string groupName = GetGroupName(columnNames[k]);
                if (!string.IsNullOrEmpty(groupName))
                    hs.Add(groupName);
            }

            return hs.ToArray();
        }

        /// <summary>
        /// 获取字段名
        /// </summary>
        /// <param name="fullColumnName"></param>
        /// <returns></returns>
        public static string GetColumnName(string fullColumnName)
        {
            Contract.Requires(fullColumnName != null);
            int index = fullColumnName.IndexOf('.');
            if (index >= 0)
                return fullColumnName.Substring(index + 1).Trim();

            return fullColumnName.Trim();
        }

        /// <summary>
        /// 将组名与字段名组合在一起
        /// </summary>
        /// <param name="group"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string BuildFullColumnName(string group, string columnName)
        {
            if (string.IsNullOrEmpty(group))
                return columnName;

            return group + "." + columnName.Trim();
        }

        /// <summary>
        /// 获取指定类型指定名称的哈希表，如果不存在则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="loadFunc"></param>
        /// <returns></returns>
        internal static IDictionary<TKey, TValue> Get<TKey, TValue>(this IDictionary dict, string key = null,
            Func<IDictionary<TKey, TValue>> loadFunc = null)
        {
            Contract.Requires(dict != null);

            Tuple<Type, Type, string> tupleKey = new Tuple<Type, Type, string>(typeof(TKey), typeof(TValue), key ?? "");
            IDictionary<TKey, TValue> result = dict[tupleKey] as IDictionary<TKey, TValue>;
            if (result == null)
                dict[tupleKey] = (result = ((loadFunc == null) ? null : loadFunc()) ?? new Dictionary<TKey, TValue>());

            return result;
        }

        /// <summary>
        /// 获取指定名称与类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="loadFunc"></param>
        /// <returns></returns>
        internal static T Get<T>(this IDictionary dict, string key, Func<string, T> loadFunc)
        {
            Contract.Requires(dict != null);

            IDictionary<string, T> cacheDict = Get<string, T>(dict, "");
            return cacheDict.GetOrSet(key ?? "", loadFunc);
        }

        /// <summary>
        /// 生成新的主键
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        internal static object GenerateNewKey(this IUtSchema schema)
        {
            return schema.GenerateNewKeys(1)[0];
        }

        /// <summary>
        /// 判断是否主键与路由键相同
        /// </summary>
        /// <param name="mtTable"></param>
        /// <returns></returns>
        internal static bool IsRootTable(this MtTable mtTable)
        {
            return mtTable.PrimaryKey.IgnoreCaseEqualsTo(mtTable.Owner.RouteKey);
        }

        /// <summary>
        /// 创建“表的用途”字符串
        /// </summary>
        /// <param name="table"></param>
        /// <param name="usage"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static string CreateUsage(MtTable table, DbConnectionUsageType usage, int index)
        {
            return CreateUsage(table.Owner.Name, table.Name, usage, index);
        }

        /// <summary>
        /// 创建“表的用途”字符串
        /// </summary>
        /// <param name="tableGroupName"></param>
        /// <param name="tableName"></param>
        /// <param name="usage"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static string CreateUsage(string tableGroupName, string tableName, DbConnectionUsageType usage, int index)
        {
            return usage + ":" + tableGroupName + "." + tableName + "." + index.ToString().PadLeft(4, '0');
        }

        /// <summary>
        /// 获取指定表的连接点
        /// </summary>
        /// <param name="mgr"></param>
        /// <param name="table"></param>
        /// <param name="index"></param>
        /// <param name="usageType"></param>
        /// <returns></returns>
        internal static IConnectionPoint GetTableConnectionPoint(this IConnectionPointManager mgr, MtTable table, int index, DbConnectionUsageType usageType)
        {
            string fullTableName = table.Owner.Name + "." + table.Name;
            return mgr.GetTableConnectionPoint(fullTableName, index, usageType);
        }

        /// <summary>
        /// 获取指定表的所有连接点
        /// </summary>
        /// <param name="mgr"></param>
        /// <param name="table"></param>
        /// <param name="usageType"></param>
        /// <returns></returns>
        internal static IConnectionPoint[] GetAllTableConnectionPoints(this IConnectionPointManager mgr, MtTable table, DbConnectionUsageType usageType)
        {
            string fullTableName = table.Owner.Name + "." + table.Name;
            return mgr.GetAllTableConnectionPoints(fullTableName, usageType);
        }
    }
}
