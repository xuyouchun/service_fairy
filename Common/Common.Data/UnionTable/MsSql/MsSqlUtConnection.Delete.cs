using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Collection;
using Common.Data.SqlExpressions;

namespace Common.Data.UnionTable.MsSql
{
    partial class MsSqlUtConnection
    {
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="where">删除条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public override int Delete(DataList data, string where, UtInvokeSettings settings)
        {
            string sql = _CreateDeleteSql(data, where, settings);
            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                return dbQuery.ExecuteScalar(sql).ToType<int>(0);
            }
        }

        private string _CreateDeleteSql(DataList data, string where, UtInvokeSettings settings)
        {
            StringBuilder sql = new StringBuilder();
            string[] filterGroups = SearchGroupNames(where);
            string defaultGroup = GetDefaultGroup(filterGroups);
            string[] allGroups = GetAllGroups();
            string whereStr = (where == null) ? null : SqlExpression.Parse(where).ToString(new SqlExpressionContext(this, defaultGroup));
            bool isSingleGroup = (allGroups.Length <= 1);

            if (!isSingleGroup)
            {
                sql.Append("Declare @Ids As Table ([").Append(PrimaryKey).Append("] ")
                    .Append(MsSqlUtility.ToColumnTypeName(PrimaryKeyColumnType)).AppendLine(" Primary Key);");
            }

            sql.AppendLine("Declare @Count As Int;");
            sql.AppendLine("Set @Count = 0;\r\n");

            // 只根据条件删除，未指定数据集
            int cmdCount = 0;
            if (data == null)
            {
                // 有条件删除
                if (!string.IsNullOrEmpty(where))
                {
                    _AppendDeleteSqlHeader(sql, filterGroups, defaultGroup, isSingleGroup);
                    sql.Append(" Where (").Append(whereStr).Append(")");

                    sql.AppendLine(";");
                    sql.AppendLine("Set @Count = @Count + @@RowCount;");
                    _AppendDeleteSqlFooter(sql, defaultGroup, allGroups);
                    cmdCount++;
                }
                else // 全部删除
                {
                    _ValidateEnsureEffectAll(settings, "删除");
                    sql.Append("Select @Count = [Rows] From sys.sysindexes idx Join sys.tables t On idx.id = t.object_id Where t.name ='")
                        .Append(GetFullTableName(defaultGroup)).Append("' and [Indid] < 2").AppendLine(";");

                    foreach (string group in allGroups)
                    {
                        string fullTableName = GetFullTableName(group);
                        sql.Append("Truncate Table [").Append(fullTableName).AppendLine("];");
                    }
                }
            }
            else  // 指定了数据集的删除操作
            {
                int rtIndex = data.GetColumnIndex(RouteKey), pkIndex = data.GetPrimaryKeyColumnIndex();
                GroupInfo[] gInfos = AnalyzeGroupInfos(data);
                filterGroups = filterGroups.Union(gInfos.ToArray(g => g.GroupName), IgnoreCaseEqualityComparer.Instance).ToArray();

                if (data.Columns.Length == 1)  // 只有一列过滤条件的删除操作
                {
                    _AppendDeleteSqlHeader(sql, filterGroups, defaultGroup, isSingleGroup);
                    ColumnInfo cInfo = data.Columns[0];
                    sql.Append(" Where ").AppendEqualsValues(cInfo.Name, cInfo.Type, data.GetValues(0), defaultGroup);
                    if (where != null) sql.Append(" And (").Append(whereStr).Append(")");
                    sql.AppendLine(";").AppendLine("Set @Count = @Count + @@RowCount;");
                    _AppendDeleteSqlFooter(sql, defaultGroup, allGroups);
                    cmdCount++;
                }
                else if (data.Columns.Length == 2 && rtIndex >= 0)  // 有两列，并且有路由键：以路由键分组，使用In语法
                {
                    int anotherIndex = (1 - rtIndex);
                    ColumnInfo rtColInfo = data.Columns[rtIndex], anotherColInfo = data.Columns[anotherIndex];
                    int index = 0;
                    foreach (var g in data.Rows.GroupBy(r => r.Cells[rtIndex]))
                    {
                        if (index++ > 0 && !isSingleGroup)
                            sql.AppendLine("\r\n\r\nDelete From @Ids;");

                        object routeKey = g.Key;
                        object[] anotherDatas = g.ToArray(r => r.Cells[anotherIndex]);

                        _AppendDeleteSqlHeader(sql, filterGroups, defaultGroup, isSingleGroup);
                        sql.Append(" Where ").AppendEqualsCondition(rtColInfo.Name, routeKey, rtColInfo.Type, defaultGroup);
                        sql.Append(" And ").AppendEqualsValues(anotherColInfo.Name, anotherColInfo.Type, anotherDatas, defaultGroup);
                        if (where != null) sql.Append(" And (").Append(whereStr).Append(")");
                        sql.AppendLine(";").AppendLine("Set @Count = @Count + @@RowCount;");
                        _AppendDeleteSqlFooter(sql, defaultGroup, allGroups);
                        cmdCount++;
                    }
                }
                else // 单条删除
                {
                    for (int rowIndex = 0; rowIndex < data.Rows.Length; rowIndex++)
                    {
                        if (rowIndex > 0 && !isSingleGroup)
                        {
                            sql.AppendLine("\r\n\r\nDelete From @Ids;");
                        }

                        DataListRow row = data.Rows[rowIndex];
                        _AppendDeleteSqlHeader(sql, filterGroups, defaultGroup, isSingleGroup);

                        string keyword = " Where ";
                        if (rtIndex >= 0 && row.Cells[rtIndex] != null)
                        {
                            sql.Append(keyword).Append("([").Append(defaultGroup).Append("].[").Append(RouteKey)
                                .Append("] = ").AppendValue(row.Cells[rtIndex]).Append(")");

                            keyword = " And ";
                        }

                        if (pkIndex != rtIndex && pkIndex >= 0 && row.Cells[pkIndex] != null)
                        {
                            sql.Append(keyword).Append("([").Append(defaultGroup).Append("].[").Append(PrimaryKey)
                                .Append("] = ").AppendValue(row.Cells[pkIndex]).Append(")");

                            keyword = " And ";
                        }

                        if (!string.IsNullOrWhiteSpace(whereStr))
                        {
                            sql.Append(keyword).Append("(").Append(whereStr).Append(")");
                            keyword = " And ";
                        }

                        for (int colIndex = 0; colIndex < data.Columns.Length; colIndex++)
                        {
                            if (colIndex == pkIndex || colIndex == rtIndex)
                                continue;

                            sql.Append(keyword);
                            keyword = " And ";

                            ColumnInfo cInfo = data.Columns[colIndex];
                            sql.Append("(").AppendEqualsCondition(cInfo.Name, row.Cells[colIndex], cInfo.Type).Append(")");
                        }

                        sql.AppendLine(";");
                        sql.AppendLine("Set @Count = @Count + @@RowCount;");
                        _AppendDeleteSqlFooter(sql, defaultGroup, allGroups);
                        cmdCount++;
                    }
                }
            }

            if (cmdCount > 1)
            {
                sql.Insert(0, "Begin Transaction;\r\n");
                sql.AppendLine("\r\n\r\nCommit Transaction;");
            }

            sql.AppendLine("Select @Count;");
            return sql.ToString();
        }

        private void _ValidateEnsureEffectAll(UtInvokeSettings settings, string opName)
        {
            if (!(settings ?? UtInvokeSettings.Default).EnsureEffectAll)
                throw new DbException("当前" + opName + "操作不允许影响到所有的行，需要设置EnsureEffectAll=true");
        }

        private void _AppendDeleteSqlFooter(StringBuilder sql, string defaultGroup, string[] allGroups)
        {
            foreach (string group in allGroups.Except(IgnoreCaseEqualityComparer.Instance, defaultGroup))
            {
                sql.Append("Delete From t From [").Append(GetFullTableName(group)).Append("] t Join @Ids d On t.[")
                    .Append(PrimaryKey).Append("] = d.[").Append(PrimaryKey).Append("];");
            }
        }

        private void _AppendDeleteSqlHeader(StringBuilder sql, string[] filterGroups, string defaultGroup, bool isSingleGroup)
        {
            sql.Append("Delete From [").Append(defaultGroup).Append("]");
            if (!isSingleGroup)
            {
                sql.Append(" Output deleted.[").Append(PrimaryKey).Append("] Into @Ids ");
            }

            sql.Append(" From [").Append(GetFullTableName(defaultGroup)).Append("] As [").Append(defaultGroup).Append("]");

            string lastGroup = defaultGroup;
            foreach (string filterGroup in filterGroups.Except(IgnoreCaseEqualityComparer.Instance, defaultGroup))
            {
                sql.Append(" Full Join [").Append(GetFullTableName(filterGroup)).Append("] As [").Append(filterGroup)
                    .Append("] On [").Append(lastGroup).Append("].[").Append(PrimaryKey).Append("] = [")
                    .Append(filterGroup).Append("].[").Append(PrimaryKey).Append("]");

                lastGroup = filterGroup;
            }
        }
    }
}
