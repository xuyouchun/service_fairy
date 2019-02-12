using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Common.Collection;
using Common.Data.SqlExpressions;

namespace Common.Data.UnionTable.MsSql
{
	partial class MsSqlUtConnection
	{
        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="columns">列</param>
        /// <param name="settings">调用设置</param>
        /// <returns>查询结果</returns>
        public override DataList Select(object[] routeKeys, string[] columns, DbSearchParam param, UtInvokeSettings settings)
        {
            if (routeKeys != null && routeKeys.Length == 0)
                return CreateEmptyDataList(columns);

            string sql = _CreateSelectSql(routeKeys, param, columns, settings);
            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                using (IDataReader reader = dbQuery.ExecuteReader(sql))
                {
                    return DataList.FromDataReader(reader, MtTable);
                }
            }
        }

        private string _CreateSelectSql(object[] routeKeys, DbSearchParam param, string[] columns, UtInvokeSettings settings)
        {
            IDictionary<string, ColumnInfo[]> colGroups = SplitColumnsByGroup(columns);
            ColumnInfo[] commonColumnInfos;
            colGroups.TryGetValue("", out commonColumnInfos);
            string[] allGroups = colGroups.Keys.Where(key => !string.IsNullOrEmpty(key))
                .Union(SearchGroupNames(param), IgnoreCaseEqualityComparer.Instance).ToArray();

            GetDefaultGroup(ref allGroups);

            StringBuilder sql = new StringBuilder("Select ");
            bool isTop = (param != null && param.Count >= 0);
            if (isTop)
                sql.Append("Top ").Append(param.Count);

            int columnCount = 0;

            string defaultGroup = GetDefaultGroup(ref allGroups);
            if (commonColumnInfos != null)
            {
                foreach (ColumnInfo cInfo in commonColumnInfos)
                {
                    if (columnCount++ > 0)
                        sql.Append(", ");

                    sql.Append("[").Append(defaultGroup).Append("].[").Append(cInfo.Name).Append("]");
                }
            }

            foreach (var item in colGroups.Where(item => !string.IsNullOrEmpty(item.Key)))
            {
                string groupName = item.Key;
                foreach (ColumnInfo cInfo in item.Value)
                {
                    if (columnCount++ > 0)
                        sql.Append(", ");

                    sql.Append("[").Append(groupName).Append("].[").Append(UtUtility.GetColumnName(cInfo.Name))
                        .Append("] As [").Append(cInfo.Name).Append("]");
                }
            }

            sql.Append(" From");
            for (int k = 0; k < allGroups.Length; k++)
            {
                string table = GetFullTableName(allGroups[k]);

                if (k > 0)
                    sql.Append(" Full Join");

                sql.Append(" [").Append(table).Append("] As [").Append(allGroups[k]).Append("]");

                if (k > 0)
                    sql.Append(" On [").Append(allGroups[k - 1]).Append("].[").Append(PrimaryKey)
                        .Append("] = [").Append(allGroups[k]).Append("].[").Append(PrimaryKey).Append("]");
            }

            string keyword = " Where ";

            if (routeKeys != null)
            {
                sql.Append(keyword).Append("(");
                keyword = " And ";
                sql.AppendEqualsValues(defaultGroup + "." + RouteKey, RouteKeyColumnType, routeKeys);
                sql.Append(")");
            }

            if (param != null && !string.IsNullOrEmpty(param.Where))
            {
                sql.Append(keyword).Append("(");
                keyword = " And ";

                SqlExpression sqlExp = SqlExpression.Parse(param.Where);
                string where = sqlExp.ToString(new SqlExpressionContext(this, defaultGroup));
                sql.Append(where);
                sql.Append(")");
            }

            if (isTop && !string.IsNullOrEmpty(param.Order))
            {
                OrderExpression orderExp = OrderExpression.Parse(param.Order);
                string order = orderExp.ToString(new SqlExpressionContext(this, defaultGroup));
                sql.Append(" Order By ").Append(order);
            }

            sql.Append(";");
            return sql.ToString();
        }
	}
}
