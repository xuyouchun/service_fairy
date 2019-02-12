using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Collection;
using Common.Utility;
using Common.Data.SqlExpressions;

namespace Common.Data.UnionTable
{
    partial class UtConnectionBase
    {
        #region Class GroupInfo ...

        /// <summary>
        /// 列信息
        /// </summary>
        protected class GroupInfo
        {
            public GroupInfo(UtConnectionBase owner, string groupName, GroupColumnInfo[] columns)
            {
                _owner = owner;
                GroupName = groupName;
                Columns = columns;
            }

            private readonly UtConnectionBase _owner;

            /// <summary>
            /// 组名
            /// </summary>
            public string GroupName { get; private set; }

            /// <summary>
            /// 列信息
            /// </summary>
            public GroupColumnInfo[] Columns { get; private set; }

            /// <summary>
            /// 是否具有除了主键及路由键以外的字段
            /// </summary>
            /// <returns></returns>
            public bool HasDataColumns()
            {
                if (_hasColumns == null)
                    _hasColumns = Columns.Any(c => !_owner.IsSpecialColumn(c.ColumnName));

                return (bool)_hasColumns;
            }

            private bool? _hasColumns;

            /// <summary>
            /// 获取列值的数值
            /// </summary>
            /// <returns></returns>
            public object[][] GetValueArrays(DataList dataList)
            {
                object[][] arrays = new object[Columns.Length][];
                for (int k = 0; k < arrays.Length; k++)
                {
                    arrays[k] = GetValueArray(dataList.Rows[k]);
                }

                return arrays;
            }

            /// <summary>
            /// 获取列值的数值
            /// </summary>
            /// <param name="r"></param>
            /// <returns></returns>
            private object[] GetValueArray(DataListRow r)
            {
                object[] array = new object[Columns.Length];

                for (int k = 0; k < array.Length; k++)
                {
                    int colIndex = Columns[k].ColumnIndex;
                    array[k] = r.Cells[colIndex];
                }

                return array;
            }

            public object Tag { get; set; }

            /// <summary>
            /// 将列名添加到sql语句中
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="withBricket">是否带有两端的括号</param>
            /// <param name="prefix">列名前缀</param>
            /// <param name="includeGroupName">是否将组名也包含进来</param>
            public void AppendColumnsToSql(StringBuilder sql, bool withBricket = false, string prefix = "", bool includeGroupName = false)
            {
                if (withBricket)
                    sql.Append("(");

                for (int k = 0; k < Columns.Length; k++)
                {
                    if (k > 0)
                        sql.Append(", ");

                    GroupColumnInfo cInfo = Columns[k];
                    if (!string.IsNullOrEmpty(prefix))
                        sql.Append(prefix).Append(".");

                    sql.Append("[");
                    if (includeGroupName && !_owner.IsSpecialColumn(cInfo.ColumnName))
                        sql.Append(GroupName).Append("."); 

                    sql.Append(cInfo.ColumnName).Append("]");
                }

                if (withBricket)
                    sql.Append(")");
            }

            /// <summary>
            /// 将数据添加到sql语句中
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="row"></param>
            /// <param name="withBricket"></param>
            public void AppendRowToSql(StringBuilder sql, DataListRow row, bool withBricket = false)
            {
                if (withBricket)
                    sql.Append("(");

                for (int k = 0; k < Columns.Length; k++)
                {
                    if (k > 0)
                        sql.Append(", ");

                    GroupColumnInfo ci = Columns[k];
                    sql.AppendValue(row.Cells[ci.ColumnIndex], ci.ColumnType);
                }

                if (withBricket)
                    sql.Append(")");
            }

            /// <summary>
            /// 将数据集添加到sql语句
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="data"></param>
            public void AppendRowsToSql(StringBuilder sql, DataList data, int start, int count)
            {
                count = Math.Min(count, data.Rows.Length - start);
                for (int k = 0; k < count; k++)
                {
                    if (k > 0)
                        sql.AppendLine(", ");

                    AppendRowToSql(sql, data.Rows[start + k], true);
                }
            }

            /// <summary>
            /// 将整个数据集添加到sql语句
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="data"></param>
            public void AppendRowsToSql(StringBuilder sql, DataList data)
            {
                AppendRowsToSql(sql, data, 0, data.Rows.Length);
            }

            /// <summary>
            /// 添加列名与数据到Sql语句中
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="data"></param>
            /// <param name="start"></param>
            /// <param name="count"></param>
            public void AppendColumnAndRowsToSql(StringBuilder sql, DataList data, int start, int count)
            {
                AppendColumnsToSql(sql, true);
                sql.Append(" Values ");
                AppendRowsToSql(sql, data, start, count);
            }

            /// <summary>
            /// 添加列表与数据到Sql语句中
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="data"></param>
            public void AppendColumnAndRowsToSql(StringBuilder sql, DataList data)
            {
                AppendColumnAndRowsToSql(sql, data, 0, data.Rows.Length);
            }

            /// <summary>
            /// 添加列名与数据到Sql语句中
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="row"></param>
            public void AppendColumnAndRowToSql(StringBuilder sql, DataListRow row)
            {
                AppendColumnsToSql(sql, true);
                sql.Append(" Values ");
                AppendRowToSql(sql, row, true);
            }



            /// <summary>
            /// 将Update Set语句添加到Sql语句
            /// </summary>
            /// <param name="sql">Sql</param>
            /// <param name="row">行</param>
            public void AppendUpdateSetToSql(StringBuilder sql, DataListRow row)
            {
                int index = 0;
                GroupColumnInfo pkCi = null;
                for (int k = 0; k < Columns.Length; k++)
                {
                    GroupColumnInfo ci = Columns[k];
                    if (pkCi == null && _owner.IsPrimaryKey(ci.ColumnName))
                    {
                        pkCi = ci;
                        continue;
                    }

                    if (_owner.IsRouteKey(ci.ColumnName))
                        continue;

                    if (index++ > 0)
                        sql.Append(", ");

                    sql.Append("[").Append(ci.ColumnName).Append("] = ");
                    sql.AppendValue(row.Cells[ci.ColumnIndex], ci.ColumnType);
                }

                if (pkCi == null)
                    throw new DbException("执行Update时需要指定主键");

                if (index == 0)  // 没有需要更新的列，为便语法正确，将主键列增加上去
                {
                    sql.Append("[").Append(pkCi.ColumnName).Append("] = ");
                    sql.AppendValue(row.Cells[pkCi.ColumnIndex], pkCi.ColumnType);
                }

                sql.Append(" Where ").AppendEqualsCondition(pkCi.ColumnName, row.Cells[pkCi.ColumnIndex], pkCi.ColumnType);
            }

            /// <summary>
            /// 将Update Set语句添加到Sql
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="prefix"></param>
            /// <param name="includeGroupName"></param>
            public void AppendUpdateSetToSql(StringBuilder sql, string prefix, bool includeGroupName = false)
            {
                GroupColumnInfo pkCi = null;
                int index = 0;

                for (int k = 0; k < Columns.Length; k++)
                {
                    GroupColumnInfo ci = Columns[k];
                    if (pkCi == null && _owner.IsPrimaryKey(ci.ColumnName))
                    {
                        pkCi = ci;
                        continue;
                    }

                    if (_owner.IsRouteKey(ci.ColumnName))
                        continue;

                    if (index++ > 0)
                        sql.Append(", ");

                    sql.Append("[").Append(ci.ColumnName).Append("] = ");
                    if (!string.IsNullOrEmpty(prefix))
                        sql.Append(prefix).Append(".");

                    sql.Append("[");
                    if (includeGroupName)
                        sql.Append(GroupName).Append(".");

                    sql.Append(ci.ColumnName).Append("]");
                }
            }
        }

        /// <summary>
        /// 列信息
        /// </summary>
        protected class GroupColumnInfo
        {
            public GroupColumnInfo(int columnIndex, ColumnInfo column)
            {
                Column = column;
                ColumnIndex = columnIndex;
                ColumnType = column.Type;

                FullName = column.Name;
                int index = column.Name.IndexOf('.');
                ColumnName = (index < 0) ? column.Name : column.Name.Substring(index + 1);
            }

            /// <summary>
            /// 列索引
            /// </summary>
            public int ColumnIndex { get; private set; }

            /// <summary>
            /// 列名
            /// </summary>
            public string ColumnName { get; private set; }

            /// <summary>
            /// 全列名
            /// </summary>
            public string FullName { get; private set; }

            /// <summary>
            /// 列类型
            /// </summary>
            public DbColumnType ColumnType { get; private set; }

            /// <summary>
            /// 列
            /// </summary>
            public ColumnInfo Column { get; private set; }
        }

        #endregion

        /// <summary>
        /// 分析数据集的列信息
        /// </summary>
        /// <param name="data">数据集</param>
        /// <param name="includeAll">是否包含所有的组</param>
        /// <returns></returns>
        protected GroupInfo[] AnalyzeGroupInfos(DataList data, bool includeAll = false)
        {
            Contract.Requires(data != null);

            IgnoreCaseDictionary<List<int>> dict = new IgnoreCaseDictionary<List<int>>();

            if (includeAll)
                dict.AddRange(GetAllGroups().Select(gName => new KeyValuePair<string, List<int>>(gName, new List<int>())));

            List<int> specialIndexes = new List<int>();

            for (int colIndex = 0; colIndex < data.Columns.Length; colIndex++)
            {
                ColumnInfo column = data.Columns[colIndex];
                string groupName, colName;
                UtUtility.SplitFullColumnName(column.Name, out groupName, out colName);

                if (string.IsNullOrEmpty(groupName))
                    specialIndexes.Add(colIndex);
                else
                    dict.GetOrSet(groupName).Add(colIndex);
            }

            List<GroupInfo> gInfos = new List<GroupInfo>();
            foreach (var item in dict)
            {
                int[] colIndexes = specialIndexes.Concat(item.Value);
                gInfos.Add(new GroupInfo(this, item.Key, colIndexes.ToArray(ci => new GroupColumnInfo(ci, data.Columns[ci]))));
            }

            return gInfos.ToArray();
        }

        /// <summary>
        /// 将列根据组名分组
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        protected IDictionary<string, ColumnInfo[]> SplitColumnsByGroup(string[] columns)
        {
            Contract.Requires(columns != null);

            var dict = new IgnoreCaseDictionary<List<ColumnInfo>>();
            foreach (string column in columns ?? GetAllColumns())
            {
                ColumnInfo columnInfo = GetColumnInfo(column);
                string groupName, columnName;
                UtUtility.SplitFullColumnName(column, out groupName, out columnName);

                dict.GetOrSet(groupName ?? "").Add(columnInfo);
            }

            return dict.ToDictionary(item => item.Key, item => item.Value.ToArray());
        }

        /// <summary>
        /// 从搜索条件中寻找列名
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected string[] SearchColumnNames(DbSearchParam param)
        {
            if (param == null)
                return Array<string>.Empty;

            return SearchColumnNames(param.Where);
        }

        /// <summary>
        /// 从搜索条件中寻找列名
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        protected string[] SearchColumnNames(string where)
        {
            if (string.IsNullOrEmpty(where))
                return Array<string>.Empty;

            SqlExpression exp = SqlExpression.Parse(where);
            return SqlExpression.GetAllFieldNames(exp);
        }

        /// <summary>
        /// 从搜索条件中寻找组名
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected string[] SearchGroupNames(DbSearchParam param)
        {
            string[] fieldNames = SearchColumnNames(param);
            return UtUtility.GetGroupNames(fieldNames);
        }

        /// <summary>
        /// 从搜索条件中寻找组名
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        protected string[] SearchGroupNames(string where)
        {
            string[] fieldNames = SearchColumnNames(where);
            return UtUtility.GetGroupNames(fieldNames);
        }

        /// <summary>
        /// 获取默认组
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        protected string GetDefaultGroup(string[] groups, bool throwError = true)
        {
            string defaultGroup = MtTable.DefaultGroup;
            if (!string.IsNullOrEmpty(defaultGroup) && (groups.IsNullOrEmpty() || groups.Contains(defaultGroup, IgnoreCaseEqualityComparer.Instance)))
                return defaultGroup;

            if (groups.IsNullOrEmpty())
            {
                if (throwError)
                    throw new DbException(string.Format("未找到表“{0}”的默认组", MtTable.Name));

                return null;
            }

            return groups[0];
        }

        /// <summary>
        /// 获取默认组，如果不存在，则修正组，使其至少包含一个组作为默认组
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        protected string GetDefaultGroup(ref string[] groups, bool throwError = true)
        {
            string defaultGroup = GetDefaultGroup(groups, false);
            if (defaultGroup == null)
            {
                if (TableInfo.AllGroups.IsNullOrEmpty())
                    throw new DbException(string.Format("表“{0}”不存在任何组", MtTable.Name));

                defaultGroup = TableInfo.AllGroups[0];
            }

            if (groups.IsNullOrEmpty())
                groups = new[] { defaultGroup };

            return defaultGroup;
        }
    }
}
