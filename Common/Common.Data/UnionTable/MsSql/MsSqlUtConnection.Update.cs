using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Collection;
using Common.Utility;
using Common.Data.SqlExpressions;

namespace Common.Data.UnionTable.MsSql
{
    partial class MsSqlUtConnection
    {
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public override int Update(DataList data, object[] routeKeys, string where, UtInvokeSettings settings)
        {
            Contract.Requires(data != null);
            if (data.Rows.Length == 0)
                return 0;

            string sql = _CreateUpdateSql(data, routeKeys, where, settings);
            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                return dbQuery.ExecuteScalar(sql).ToType<int>();
            }
        }

        // 前提：如果routeKeys不为空，则data中只允许包含一行，且不能包含路由键
        private string _CreateUpdateSql(DataList data, object[] routeKeys, string where, UtInvokeSettings settings)
        {
            StringBuilder sql = new StringBuilder();

            GroupInfo[] gInfos = AnalyzeGroupInfos(data);
            string[] updateGroups = gInfos.ToArray(g => g.GroupName);
            string[] filterGroups = SearchGroupNames(where);
            string defaultGroup = GetDefaultGroup(updateGroups);
            GroupInfo defGInfo = gInfos.First(g => g.GroupName.IgnoreCaseEqualsTo(defaultGroup));
            int pkIndex = data.GetColumnIndex(PrimaryKey), rtIndex = data.GetColumnIndex(RouteKey);
            bool isSingleGroup = !gInfos.Any(gInfo => !gInfo.GroupName.IgnoreCaseEqualsTo(defaultGroup));
            int cmdCount = 0;

            string whereSql = string.IsNullOrEmpty(where) ? null : SqlExpression.Parse(where).ToString(new SqlExpressionContext(this, defaultGroup));

            if (!isSingleGroup)
            {
                sql.Append("Declare @Ids As Table ([").Append(PrimaryKey).Append("] ")
                    .Append(MsSqlUtility.ToColumnTypeName(PrimaryKeyColumnType)).Append(" Primary Key");

                if (pkIndex != rtIndex)
                {
                    sql.Append(", [").Append(RouteKey).Append("] ")
                        .Append(MsSqlUtility.ToColumnTypeName(RouteKeyColumnType));
                }

                sql.AppendLine(");");
            }

            sql.AppendLine("Declare @TotalCount As Int;");
            sql.AppendLine("Declare @Count As Int;");
            sql.AppendLine("Set @TotalCount = 0;\r\n");

            for (int rowIndex = 0; rowIndex < data.Rows.Length; rowIndex++)
            {
                if (rowIndex > 0 & !isSingleGroup)
                    sql.AppendLine("\r\n\r\nDelete From @Ids;");

                DataListRow row = data.Rows[rowIndex];
                string updateGroup = defGInfo.GroupName;
                sql.Append("Update [").Append(updateGroup).Append("] Set ");

                _AppendUpdateSetValuesSql(sql, defGInfo, pkIndex, rtIndex, row);

                if (!isSingleGroup)
                {
                    sql.Append(" Output inserted.[").Append(PrimaryKey).Append("]");
                    if (rtIndex != pkIndex)
                        sql.Append(", inserted.[").Append(RouteKey).Append("]");
                    sql.Append(" Into @Ids");
                }

                sql.Append(" From [").Append(GetFullTableName(updateGroup)).Append("] As [").Append(updateGroup).Append("]");
                string lastGroup = updateGroup;
                foreach (string filterGroup in filterGroups.Except(new[] { updateGroup }, IgnoreCaseEqualityComparer.Instance))
                {
                    sql.Append(" Full Join [").Append(GetFullTableName(filterGroup)).Append("] As [").Append(filterGroup).Append("] On [")
                        .Append(lastGroup).Append("].[").Append(PrimaryKey).Append("] = [").Append(filterGroup).Append("].[").Append(PrimaryKey).Append("]");

                    lastGroup = filterGroup;
                }

                const string DEF_KEYWORD = " Where ";
                string keyword = DEF_KEYWORD;

                // 数据集中的路由键
                object v;
                if (rtIndex >= 0 && (v = row.Cells[rtIndex]) != null)
                {
                    sql.Append(keyword).Append("([").Append(updateGroup).Append("].[").Append(RouteKey).Append("] = ").AppendValue(v, RouteKeyColumnType).Append(")");
                    keyword = " And ";
                }

                // 明确指定的路由键
                if (!routeKeys.IsNullOrEmpty())
                {
                    sql.Append(keyword).AppendEqualsValues(RouteKey, RouteKeyColumnType, routeKeys);
                    keyword = " And ";
                }

                // 主键
                if (rtIndex != pkIndex && pkIndex >= 0 && (v = row.Cells[pkIndex]) != null)
                {
                    sql.Append(keyword).Append("([").Append(updateGroup).Append("].[").Append(PrimaryKey).Append("] = ").AppendValue(v, PrimaryKeyColumnType).Append(")");
                    keyword = " And ";
                }

                // Where语句
                if (!string.IsNullOrEmpty(whereSql))
                {
                    sql.Append(keyword).Append("(").Append(whereSql).Append(")");
                    keyword = " And ";
                }

                if (keyword == DEF_KEYWORD)
                    _ValidateEnsureEffectAll(settings, "更新");

                sql.AppendLine(";");
                sql.AppendLine("Set @Count = @@RowCount;");
                sql.AppendLine("Set @TotalCount = @TotalCount + @Count;");

                cmdCount++;

                // 其它分表的更新
                if (!isSingleGroup)
                {
                    foreach (GroupInfo gInfo in gInfos.Except(defGInfo))
                    {
                        string groupName = gInfo.GroupName;

                        sql.Append("Update t Set ");
                        _AppendUpdateSetValuesSql(sql, gInfo, pkIndex, rtIndex, row);

                        sql.Append(" From [").Append(GetFullTableName(groupName)).Append("] t Join @Ids d On t.[")
                            .Append(PrimaryKey).Append("] = d.[").Append(PrimaryKey).AppendLine("];");

                        sql.AppendLine("If @@ROWCOUNT != @Count");
                        sql.Append("  With t0 As (Select * From (Values (");
                        int index = 0;
                        List<string> colNames = new List<string>();
                        for (int k = 0; k < gInfo.Columns.Length; k++)
                        {
                            GroupColumnInfo cInfo = gInfo.Columns[k];
                            int cIndex = cInfo.ColumnIndex;
                            if (cIndex != rtIndex && cIndex != pkIndex)
                            {
                                if (index++ > 0)
                                    sql.Append(", ");

                                sql.AppendValue(row.Cells[cIndex], cInfo.ColumnType);
                                colNames.Add(cInfo.ColumnName);
                            }
                        }

                        sql.Append(")) As v (").Append(string.Join(", ", colNames)).AppendLine(")),");
                        sql.Append("  t As (Select * From @Ids Cross Join t0");
                        sql.Append(" Where Not Exists (Select Null From [").Append(GetFullTableName(groupName));
                        sql.Append("] s Where [").Append(PrimaryKey).Append("] = s.[").Append(PrimaryKey).AppendLine("]))");
                        sql.Append("  Insert Into [").Append(GetFullTableName(groupName)).Append("] ");
                        gInfo.AppendColumnsToSql(sql, true);
                        sql.Append(" Select ");
                        gInfo.AppendColumnsToSql(sql, false, "t");
                        sql.AppendLine(" From t;");
                    }

                    cmdCount++;
                }
            }

            if (cmdCount > 1)
            {
                sql.Insert(0, "Begin Transaction;\r\n\r\n");
                sql.AppendLine("\r\nCommit Transaction;");
            }

            sql.Append("\r\nSelect @TotalCount;");
            return sql.ToString();
        }

        private static void _AppendUpdateSetValuesSql(StringBuilder sql, GroupInfo gInfo, int pkIndex, int rtIndex, DataListRow row)
        {
            int j = 0;
            for (int k = 0, len = gInfo.Columns.Length; k < len; k++)
            {
                var gCol = gInfo.Columns[k];
                ColumnInfo ci = gCol.Column;
                int colIndex = gCol.ColumnIndex;
                if (colIndex == pkIndex || colIndex == rtIndex)
                    continue;

                if (j++ > 0)
                    sql.Append(", ");

                sql.Append("[").Append(UtUtility.GetColumnName(ci.Name)).Append("] = ").AppendValue(row.Cells[colIndex], ci.Type);
            }
        }
    }
}
