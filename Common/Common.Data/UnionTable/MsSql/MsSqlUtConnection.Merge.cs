using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Data.SqlExpressions;
using Common.Collection;
using Common.Data.UnionTable.Metadata;

namespace Common.Data.UnionTable.MsSql
{
    partial class MsSqlUtConnection
    {
        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="routeKey">路由键</param>
        /// <param name="data">用于合并的数据</param>
        /// <param name="compareColumns">用于比较的字段</param>
        /// <param name="where">过滤条件</param>
        /// <param name="option">合并选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响的行数</returns>
        public override int Merge(object routeKey, DataList data, string[] compareColumns, string where, UtConnectionMergeOption option, UtInvokeSettings settings)
        {
            if (option == UtConnectionMergeOption.None)
                return 0;

            string sql = _CreateMergeSql(routeKey, data, compareColumns, where, option, settings);
            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                return dbQuery.ExecuteScalar(sql).ToType<int>();
            }
        }

        private string _CreateMergeSql(object routeKey, DataList data, string[] compareColumns, string where, UtConnectionMergeOption option, UtInvokeSettings settings)
        {
            StringBuilder sql = new StringBuilder();

            string[] filterGroups = SearchGroupNames(where);  // 过滤条件的涉及的组
            string[] compareGroups = UtUtility.GetGroupNames(compareColumns);  // 用于比较的列所涉及的组
            GroupInfo[] dataGroupInfos = AnalyzeGroupInfos(data);  // 数据集所涉及的组
            string[] dataGroups = dataGroupInfos.ToArray(gInfo => gInfo.GroupName);  // 数据集所涉及的组
            string defaultGroup = GetDefaultGroup(dataGroups.Concat(filterGroups).ToArray());  // 默认组名（将在该组执行Merge）

            int pkIndex = data.GetColumnIndex(PrimaryKey), rtIndex = data.GetColumnIndex(RouteKey);
            string whereSql = string.IsNullOrEmpty(where) ? null : SqlExpression.Parse(where).ToString(new SqlExpressionContext(this, defaultGroup));

            // 开始事务并定义变量
            sql.AppendLine("Begin Transaction;\r\n");
            sql.AppendLine("Declare @Count As Int;");  // 受影响的行数
            sql.Append("Declare @Ids As Table ([Action] Varchar(10), InsertedKey ").Append(MsSqlUtility.ToColumnTypeName(PrimaryKeyColumnType))
                .Append(", DeletedKey ").Append(MsSqlUtility.ToColumnTypeName(PrimaryKeyColumnType)).AppendLine(");\r\n");

            // 创建表变量保存需要合并的值
            _AppendMergeTableVariable(sql, data);

            // 创建源表的SQL表述
            _AppendMergeSourceTable(sql, routeKey, data, whereSql, filterGroups, compareColumns, defaultGroup);

            // Merge语句
            bool updated, inserted, deleted;
            _AppendMergeSql(sql, compareColumns, defaultGroup, option, out updated, out inserted, out deleted);

            sql.AppendLine("Set @Count = @@RowCount;");

            // 其它表的修改
            if (updated)
                _AppendMergeUpdate(sql, data, dataGroupInfos, defaultGroup);

            if (inserted)
                _AppendMergeInsert(sql, data, dataGroupInfos, defaultGroup);

            if (deleted)
                _AppendMergeDelete(sql, data, dataGroupInfos, defaultGroup);

            // 提交事务
            sql.AppendLine();
            sql.AppendLine("Commit Transaction;");
            sql.AppendLine("Select @Count;");

            return sql.ToString();
        }

        private void _AppendMergeTableVariable(StringBuilder sql, DataList data)
        {
            sql.Append("Declare @TargetTable As Table (");
            MtColumn[] mtCols = new MtColumn[data.Columns.Length];
            for (int k = 0; k < mtCols.Length; k++)
            {
                if (k > 0)
                    sql.Append(", ");

                ColumnInfo ci = data.Columns[k];
                sql.AppendMsSqlColumnSql(mtCols[k] = GetMtColumn(ci.Name));
            }
            sql.AppendLine(");");

            foreach (IEnumerable<DataListRow> rows in data.Rows.Split(200))
            {
                sql.Append("Insert Into @TargetTable (");
                for (int k = 0; k < mtCols.Length; k++)
                {
                    if (k > 0)
                        sql.Append(", ");

                    MtColumn mtCol = mtCols[k];
                    sql.Append("[").Append(mtCol.Name).Append("]");
                }

                sql.Append(") Values ");
                int j = 0;
                foreach (DataListRow row in rows)
                {
                    if (j++ > 0)
                        sql.AppendLine(",");

                    sql.Append("(");
                    for (int i = 0; i < mtCols.Length; i++)
                    {
                        if (i > 0)
                            sql.Append(", ");

                        MtColumn mtCol = mtCols[i];
                        sql.AppendValue(row.Cells[i], mtCol.Type);
                    }

                    sql.Append(")");
                }

                sql.AppendLine(";");
            }

            sql.AppendLine();
        }

        private void _AppendMergeSourceTable(StringBuilder sql, object routeKey, DataList data, string where,
            string[] filterGroups, string[] compareColumns, string defaultGroup)
        {
            sql.Append("With s As (Select ");

            string[] groups = filterGroups.Concat(UtUtility.GetGroupNames(compareColumns)).Concat(new[] { defaultGroup }).Distinct(_ig).ToArray();

            sql.AppendColumnName(PrimaryKey, defaultGroup).Append(" As [").Append(PrimaryKey).Append("]");
            if (!RouteKey.IgnoreCaseEqualsTo(PrimaryKey))
                sql.Append(", ").AppendColumnName(RouteKey, defaultGroup).Append(" As [").Append(RouteKey).Append("]");

            foreach (string group in groups)
            {
                ColumnInfo[] colInfos = GetColumnInfosOfGroup(group);

                foreach (ColumnInfo cInfo in colInfos)
                {
                    sql.Append(", ");
                    if (!IsSpecialColumn(cInfo.Name))
                    {
                        sql.AppendColumnName(cInfo.Name).Append(" As [").Append(cInfo.Name).Append("]");
                    }
                }
            }

            sql.Append(" From ");

            for (int k = 0; k < groups.Length; k++)
            {
                if (k > 0)
                    sql.Append(" Full Join ");

                string group = groups[k];
                sql.Append("[").Append(GetFullTableName(group)).Append("] As [").Append(group).Append("]");

                if (k > 0)
                    sql.Append(" On [").Append(groups[k - 1]).Append("].[").Append(PrimaryKey)
                        .Append("] = [").Append(group).Append("].[").Append(PrimaryKey).Append("]");
            }

            sql.Append(" Where ").AppendEqualsCondition(RouteKey, routeKey, RouteKeyColumnType, defaultGroup);
            if (!string.IsNullOrEmpty(where))
                sql.Append(" And ").Append(where);

            sql.AppendLine(")");
        }

        private void _AppendMergeSql(StringBuilder sql, string[] compareColumns, string defaultGroup, UtConnectionMergeOption option,
            out bool updated, out bool inserted, out bool deleted)
        {
            if (compareColumns.IsNullOrEmpty())
                compareColumns = new[] { PrimaryKey };

            sql.Append("Merge Into s Using @TargetTable As t On ");
            for (int k = 0; k < compareColumns.Length; k++)
            {
                if (k > 0)
                    sql.Append(" And ");

                string col = compareColumns[k];
                sql.Append("s.[").Append(col).Append("] = t.[").Append(col).Append("]");
            }

            sql.AppendLine();

            ColumnInfo[] defColInfos = GetColumnInfosOfGroup(defaultGroup);
            updated = _AppendMergeWhenUpdate(sql, compareColumns, option, defColInfos);
            inserted = _AppendMergeWhenInsert(sql, option, defColInfos);
            deleted = _AppendMergeWhenDelete(sql, option, defColInfos);

            sql.Append("  Output $Action, Inserted.[").Append(PrimaryKey)
                .Append("] As InsertedKey, Deleted.[").Append(PrimaryKey).Append("] As DeletedKey Into @Ids");

            sql.AppendLine(";\r\n");
        }

        private bool _AppendMergeWhenDelete(StringBuilder sql, UtConnectionMergeOption option, ColumnInfo[] defColInfos)
        {
            if (!option.HasFlag(UtConnectionMergeOption.Delete))
                return false;

            sql.AppendLine("  When Not Matched By Source Then Delete");
            return true;
        }

        private bool _AppendMergeWhenInsert(StringBuilder sql, UtConnectionMergeOption option, ColumnInfo[] defColInfos)
        {
            if (!option.HasFlag(UtConnectionMergeOption.Insert))
                return false;

            string[] insertCols = defColInfos.Select(ci => ci.Name).Union(new[] { PrimaryKey, RouteKey }, _ig).ToArray();
            if (insertCols.Length == 0)
                return false;

            sql.Append("  When Not Matched By Target Then Insert (");

            for (int k = 0; k < insertCols.Length; k++)
            {
                if (k > 0)
                    sql.Append(", ");

                sql.Append("[").Append(insertCols[k]).Append("]");
            }

            sql.Append(") Values (");

            for (int k = 0; k < insertCols.Length; k++)
            {
                if (k > 0)
                    sql.Append(", ");

                sql.Append("t.[").Append(insertCols[k]).Append("]");
            }

            sql.AppendLine(")");
            return true;
        }

        private bool _AppendMergeWhenUpdate(StringBuilder sql, string[] compareColumns, UtConnectionMergeOption option, ColumnInfo[] defColInfos)
        {
            if (!option.HasFlag(UtConnectionMergeOption.Update))
                return false;

            string[] updateCols = defColInfos.Select(c => c.Name)
                .Except(_ig, new[] { PrimaryKey, RouteKey }).Except(_ig, compareColumns).ToArray();

            if (updateCols.Length == 0)
                return false;

            sql.Append("  When Matched Then Update Set ");
            for (int k = 0; k < updateCols.Length; k++)
            {
                if (k > 0)
                    sql.Append(", ");

                string colName = updateCols[k];
                sql.Append("[").Append(colName).Append("] = t.[").Append(colName).Append("]");
            }

            sql.AppendLine();
            return true;
        }

        private void _AppendMergeUpdate(StringBuilder sql, DataList data, GroupInfo[] dataGroupInfos, string defaultGroup)
        {
            Func<GroupColumnInfo, bool> filter = (ci) => {
                string colName = ci.ColumnName;
                return !IsSpecialColumn(colName) && data.ContainsColumn(colName);
            };

            GroupInfo[] gis = dataGroupInfos.Where(gi => gi.GroupName.IgnoreCaseEqualsTo(defaultGroup) && gi.Columns.Length == 0).ToArray();
            if (gis.Length == 0)
                return;

            sql.AppendLine("If Exists (Select Null From @Ids Where [Action] = 'UPDATE') Begin");
            foreach (GroupInfo gi in gis)
            {
                GroupColumnInfo[] cis = gi.Columns.WhereToArray(filter);
                sql.Append("  Update s Set ");
                for (int k = 0; k < cis.Length; k++)
                {
                    if (k > 0)
                        sql.Append(", ");

                    GroupColumnInfo ci = cis[k];
                    sql.Append("[").Append(ci.ColumnName).Append("] = t.[").Append(ci.ColumnName).Append("]");
                }

                sql.Append(" From [").Append(GetFullTableName(gi.GroupName)).Append("] s Join @TargetTable t On s.[")
                    .Append(PrimaryKey).Append("] = t.[").Append(PrimaryKey).Append("] Join @Ids i ")
                    .Append("i.InsertedKey = s.[").Append(PrimaryKey).Append("] Where i.[Action] = 'INSERT'");
                sql.AppendLine();
            }
            sql.AppendLine("End;");
        }

        private void _AppendMergeInsert(StringBuilder sql, DataList data, GroupInfo[] dataGroupInfos, string defaultGroup)
        {
            GroupInfo[] gis = dataGroupInfos.Where(gi => !gi.GroupName.IgnoreCaseEqualsTo(defaultGroup)).ToArray();
            if (gis.Length == 0)
                return;

            sql.AppendLine("If Exists (Select Null From @Ids Where [Action] = 'INSERT') Begin");

            foreach (GroupInfo gi in gis)
            {
                string[] columnNames = gi.Columns.Select(ci => ci.FullName).Union(new[] { PrimaryKey, RouteKey }, _ig).ToArray();
                sql.Append("  Insert Into [").Append(GetFullTableName(gi.GroupName)).Append("] (");
                for (int k = 0; k < columnNames.Length; k++)
                {
                    if (k > 0)
                        sql.Append(", ");

                    sql.Append("[").Append(UtUtility.GetColumnName(columnNames[k])).Append("]");
                }
                sql.Append(") Select ");
                for (int k = 0; k < columnNames.Length; k++)
                {
                    if (k > 0)
                        sql.Append(", ");

                    sql.Append("[").Append(columnNames[k]).Append("]");
                }

                sql.Append(" From @TargetTable t Join @Ids i On i.InsertedKey = t.[")
                    .Append(PrimaryKey).Append("] Where i.[Action] = 'INSERT'");

                sql.AppendLine(";");
            }

            sql.AppendLine("End;");
        }

        private void _AppendMergeDelete(StringBuilder sql, DataList data, GroupInfo[] dataGroupInfos, string defaultGroup)
        {
            string[] groups = GetAllGroups().Except(_ig, defaultGroup).ToArray();
            if (groups.Length == 0)
                return;

            sql.AppendLine("If Exists (Select Null From @Ids Where [Action] = 'DELETE') Begin");

            foreach (string group in groups)
            {
                sql.Append("  Delete From s From [").Append(GetFullTableName(group)).Append("] s Join @Ids i On ")
                    .Append("i.DeletedKey = s.[").Append(PrimaryKey).Append("] Where i.[Action] = 'DELETE'");

                sql.AppendLine(";");
            }

            sql.AppendLine("End;");
        }
    }
}