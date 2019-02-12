using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Data.UnionTable.Metadata;

namespace Common.Data.UnionTable.MsSql
{
    partial class MsSqlUtConnection
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="autoUpdate">当数据已经存在时，是否将其更新</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响的行数</returns>
        public override int Insert(DataList data, bool autoUpdate, UtInvokeSettings settings)
        {
            Contract.Requires(data != null);

            if (data.Rows.Length == 0)
                return 0;

            string sql = _CreateInsertSql(data, autoUpdate, settings);
            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                dbQuery.ExecuteNonQuery(sql);
                return data.Rows.Length;
            }
        }

        private string _CreateInsertSql(DataList data, bool autoUpdate, UtInvokeSettings settings)
        {
            GroupInfo[] gInfos = AnalyzeGroupInfos(data, true);
            GroupInfo defGInfo = gInfos.First(gInfo => string.Equals(gInfo.GroupName, MtTable.DefaultGroup, StringComparison.OrdinalIgnoreCase));
            StringBuilder sql = new StringBuilder();
            const int partialCount = 100;
            int cmdCount = data.Rows.Length / partialCount;

            if (!autoUpdate)  // 不更新的插入
            {
                int index = 0;
                while (index < data.Rows.Length)
                {
                    // 有数据的组
                    foreach (GroupInfo gInfo in gInfos)
                    {
                        sql.Append("Insert Into [").Append(GetFullTableName(gInfo.GroupName)).Append("] ");
                        gInfo.AppendColumnsToSql(sql, true);
                        sql.Append(" Values ");
                        gInfo.AppendRowsToSql(sql, data, index, partialCount);
                        sql.AppendLine(";");
                    }

                    index += partialCount;
                }

                if (cmdCount > 1)
                {
                    sql.Insert(0, "Begin Transaction;\r\n\r\n");
                    sql.Append("\r\n\r\nCommit Transaction;");
                }
            }
            else // 自动更新的插入
            {
                sql.AppendLine("Begin Transaction;\r\n");
                if (data.Rows.Length == 1)  // 行数较少
                {
                    DataListRow row = data.Rows[0];
                    for (int k = 0; k < gInfos.Length; k++)
                    {
                        if (k > 0)
                            sql.AppendLine();

                        GroupInfo gInfo = gInfos[k];
                        sql.Append("Update [").Append(GetFullTableName(gInfo.GroupName)).Append("] Set ");
                        gInfo.AppendUpdateSetToSql(sql, row);
                        sql.AppendLine(";");

                        sql.AppendLine("If @@RowCount = 0");
                        sql.Append("  Insert Into [").Append(GetFullTableName(gInfo.GroupName)).Append("] ");
                        gInfo.AppendColumnAndRowToSql(sql, row);
                        sql.AppendLine(";");
                    }
                }
                else  // 行数较多，需要创建表变量
                {
                    _AppendInsertTableVariable(sql, data);

                    for (int k = 0; k < gInfos.Length; k++)
                    {
                        if (k > 0)
                            sql.AppendLine();

                        GroupInfo gInfo = gInfos[k];

                        // 插入不存在的记录
                        sql.Append("Insert Into [").Append(GetFullTableName(gInfo.GroupName)).Append("] ");
                        gInfo.AppendColumnsToSql(sql, true);
                        sql.Append(" Select ");
                        gInfo.AppendColumnsToSql(sql, prefix: "t", includeGroupName: true);
                        sql.Append(" From @TargetTable t Where Not Exists (Select Null From [").Append(GetFullTableName(gInfo.GroupName));
                        sql.Append("] s Where s.[").Append(MtTable.PrimaryKey).Append("] = t.[").Append(MtTable.PrimaryKey).AppendLine("]);");

                        // 更新已存在的记录
                        if (gInfo.HasDataColumns())
                        {
                            sql.Append("If @@RowCount <> ").Append(data.Rows.Length).AppendLine();
                            sql.Append("  Update s Set ");
                            gInfo.AppendUpdateSetToSql(sql, "t", true);
                            sql.Append(" From [").Append(GetFullTableName(gInfo.GroupName)).Append("] s Join @TargetTable t On t.[");
                            sql.Append(MtTable.PrimaryKey).Append("] = s.[").Append(MtTable.PrimaryKey).AppendLine("];");
                        }
                    }
                }

                sql.Append("\r\n\r\nCommit Transaction;");
            }

            return sql.ToString();
        }

        private void _AppendInsertTableVariable(StringBuilder sql, DataList data)
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
    }
}
