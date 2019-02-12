using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Common.Data.UnionTable.Metadata;
using System.Diagnostics.Contracts;
using System.Data;
using Common.Collection;
using Common.Utility;
using Common.Package;
using Common.Contracts.Log;

namespace Common.Data.UnionTable.MsSql
{
    /// <summary>
    /// 对表的字段进行比较，以修正表的结构
    /// </summary>
    static class MsSqlUtility
    {
        /// <summary>
        /// 修正表结构
        /// </summary>
        /// <param name="dbQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        public static void Revise(DbQuery dbQuery, string tableName, MtColumn[] columns)
        {
            int rowCount;
            MtColumn[] oldColumns = LoadMtColumns(dbQuery, tableName, out rowCount);
            _Revise(dbQuery, tableName, oldColumns, columns, rowCount);
        }

        /// <summary>
        /// 批量修正表结构
        /// </summary>
        /// <param name="dbQuery"></param>
        /// <param name="tables"></param>
        public static bool Revise(DbQuery dbQuery, UtTableReviseInfo[] tables)
        {
            Contract.Requires(dbQuery != null && tables != null);

            if (tables.Select(t => t.TableName).IsRepeated(IgnoreCaseEqualityComparer.Instance))
                throw new ArgumentNullException("表名重复");

            MtTableItem[] mts = _LoadMtColumns(dbQuery, tables.ToArray(t => t.TableName));
            IDictionary<string, MtTableItem> mtsDict = mts.ToIgnoreCaseDictionary(mt => mt.TableName);

            bool modified = false;
            foreach (UtTableReviseInfo table in tables)
            {
                string tableName = table.TableName;
                MtTableItem item = mtsDict.GetOrDefault(tableName);
                if (_Revise(dbQuery, tableName, (item == null) ? null : item.Columns.ToArray(), table.Columns, (item == null) ? 0 : item.RowCount))
                    modified = true;
            }

            return modified;
        }

        class MtTableItem
        {
            public string TableName;
            public int RowCount;
            public List<MtColumn> Columns;
        }

        private static bool _Revise(DbQuery dbQuery, string tableName, MtColumn[] oldColumns, MtColumn[] newColumns, int rowCount)
        {
            if (oldColumns == null)  // 表不存在
            {
                CreateTable(dbQuery, tableName, newColumns);
                return true;
            }

            MtColumnComparedItem[] comparedItems = _Compare(oldColumns, newColumns);
            if (comparedItems.Length == 0)
                return false;

            if (rowCount == 0)  // 数据库中没有记录的情况下，直接将表删除，并创建新表
            {
                CreateTable(dbQuery, tableName, newColumns, true);
                return true;
            }

            LogItemCollection logItems = new LogItemCollection();
            string sql = _CreateReviseSql(tableName, comparedItems, logItems);
            if (!string.IsNullOrEmpty(sql))
            {
                dbQuery.ExecuteNonQuery(sql);
                logItems.Log();
                return true;
            }

            return false;
        }

        private static string _CreateReviseSql(string tableName, MtColumnComparedItem[] comparedItems, LogItemCollection logItems)
        {
            StringBuilder sql = new StringBuilder();
            foreach (MtColumnComparedItem item in comparedItems)
            {
                MtColumnComparedType comparedType = item.Type;
                MtColumn newCol = item.New, oldCol = item.Old;
                if (comparedType.HasFlag(MtColumnComparedType.NewColumn))
                {
                    sql.Append("Alter Table [").Append(tableName).Append("] Add ");
                    AppendMsSqlColumnSql(sql, newCol);
                    sql.AppendLine(";");

                    logItems.AppendMessageFormat("在表“{0}”中添加列“{1}”", tableName, newCol);
                    _AppendIndexSql(sql, tableName, newCol, logItems);
                }
                else if (comparedType.HasFlag(MtColumnComparedType.MissingColumn))
                {
                    logItems.AppendWarningFormat("表“{0}”中的列“{1}”已经不存在，但数据库中尚未删除", tableName, UtUtility.GetColumnName(oldCol.Name));
                }
                else
                {
                    if (comparedType.HasFlag(MtColumnComparedType.NewIndex))
                    {
                        _AppendIndexSql(sql, tableName, newCol, logItems);
                        sql.AppendLine(";");
                    }

                    if(comparedType.HasFlag(MtColumnComparedType.MissingIndex))
                    {
                        logItems.AppendWarningFormat("表“{0}”中的列“{1}”的索引已经不存在，但数据库中尚未删除", tableName, UtUtility.GetColumnName(oldCol.Name));
                    }

                    if (comparedType.HasFlag(MtColumnComparedType.ColumnSizeChanged))
                    {
                        if (_CanUpgrade(oldCol, newCol))
                        {
                            sql.Append("Alter Table [").Append(tableName).Append("] Alter Column ");
                            AppendMsSqlColumnSql(sql, newCol);
                            sql.AppendLine(";");
                            logItems.AppendMessageFormat("表“{0}”的列“{1}”改变：{2} -> {3}", tableName, _GetColTypeDesc(oldCol), _GetColTypeDesc(newCol));
                        }
                        else
                        {
                            logItems.AppendWarningFormat("表“{0}”的列“{1}”改变：{2} -> {3}，但数据库中未作改变", tableName, _GetColTypeDesc(oldCol), _GetColTypeDesc(newCol));
                        }
                    }
                }
            }

            return sql.ToString();
        }

        private static string _GetColTypeDesc(MtColumn col)
        {
            if (col == null)
                return "";

            string s = ToColumnTypeName(col.Type);
            DbColumnTypeInfo typeInfo = DbColumnTypeInfo.GetTypeInfo(col.Type);
            if (typeInfo == null || typeInfo.IsVarLen)
                s += "(" + (typeInfo.ElementSize == 0 ? "Max" : typeInfo.ElementSize.ToString()) + ")";

            return s;
        }

        // 判断列的类型是否可以升级
        private static bool _CanUpgrade(MtColumn oldCol, MtColumn newCol)
        {
            DbColumnType oldColType = oldCol.Type, newColType = newCol.Type;
            DbColumnTypeInfo oldColTypeInfo = DbColumnTypeInfo.GetTypeInfo(oldColType);
            DbColumnTypeInfo newColTypeInfo = DbColumnTypeInfo.GetTypeInfo(newColType);

            if (oldColType == newColType)  // 相同类型
            {
                return !oldColTypeInfo.IsVarLen || newCol.Size >= oldCol.Size;
            }
            else // 不同类型
            {
                if (oldColTypeInfo.TypeCode == newColTypeInfo.TypeCode)
                    return newCol.Size >= oldCol.Size;
            }

            return false;
        }

        public static void AppendMsSqlColumnSql(this StringBuilder sql, MtColumn column)
        {
            sql.Append("[").Append(column.Name).Append("] ").Append(ToColumnTypeName(column.Type));

            if (IsVarLenType(column.Type))
            {
                sql.Append("(").Append(column.Size == 0 ? "Max" : column.Size.ToString()).Append(")");
            }
        }

        private static void _AppendIndexSql(StringBuilder sql, string tableName, MtColumn column, LogItemCollection logItems)
        {
            if (column.IndexType == DbColumnIndexType.None)
                return;

            sql.Append("Create ");
            sql.Append((column.IndexType == DbColumnIndexType.Master) ? "Clustered" : "NonClustered");
            sql.Append(" Index [Index_").Append(tableName).Append("_").Append(column.Name);
            sql.Append("] On [").Append(tableName).Append("] ([").Append(column.Name).Append("])");
            sql.AppendLine(";");

            logItems.AppendMessageFormat("在表“{0}”的列“{1}”添加{2}", tableName, UtUtility.GetColumnName(column.Name), column.IndexType.GetDesc());
        }

        #region Class MtColumnComparedItem ...

        class MtColumnComparedItem
        {
            public MtColumnComparedType Type;

            public MtColumn Old, New;

            public override string ToString()
            {
                return (Old ?? New).Name + " " + Type.ToString();
            }
        }

        [Flags]
        enum MtColumnComparedType
        {
            None = 0,

            /// <summary>
            /// 列缺失
            /// </summary>
            MissingColumn = 0x01,

            /// <summary>
            /// 新列
            /// </summary>
            NewColumn = 0x02,

            /// <summary>
            /// 列类型变化
            /// </summary>
            ColumnTypeChanged = 0x04,

            /// <summary>
            /// 列长度变化
            /// </summary>
            ColumnSizeChanged = 0x08,

            /// <summary>
            /// 索引缺失
            /// </summary>
            MissingIndex = 0x10,

            /// <summary>
            /// 新索引
            /// </summary>
            NewIndex = 0x20,

            /// <summary>
            /// 索引类型变化
            /// </summary>
            IndexTypeChanged = 0x40
        }

        #endregion

        private static MtColumnComparedItem[] _Compare(MtColumn[] oldColumns, MtColumn[] newColumns)
        {
            List<MtColumnComparedItem> items = new List<MtColumnComparedItem>();

            oldColumns.ToIgnoreCaseDictionary(col => col.Name).CompareTo(newColumns.ToIgnoreCaseDictionary(col => col.Name), (colName, oldCol, newCol, changedType) => {

                if (changedType == CollectionChangedType.Add)
                {
                    items.Add(new MtColumnComparedItem { New = newCol, Type = MtColumnComparedType.NewColumn });
                }
                else if (changedType == CollectionChangedType.Remove)
                {
                    items.Add(new MtColumnComparedItem { Old = oldCol, Type = MtColumnComparedType.MissingColumn });
                }
                else if (changedType == CollectionChangedType.Update)
                {
                    MtColumnComparedType comparedType = MtColumnComparedType.None;
                    if (oldCol.IndexType != newCol.IndexType)
                    {
                        if (oldCol.IndexType == DbColumnIndexType.None)
                            comparedType |= MtColumnComparedType.NewIndex;
                        else if (newCol.IndexType == DbColumnIndexType.None)
                            comparedType |= MtColumnComparedType.MissingIndex;
                        else
                            comparedType |= MtColumnComparedType.IndexTypeChanged;
                    }

                    if (oldCol.Type != newCol.Type)
                        comparedType |= MtColumnComparedType.ColumnTypeChanged;
                    else if (!_CompareColumnSize(oldCol.Type, oldCol.Size, newCol.Size))
                        comparedType |= MtColumnComparedType.ColumnSizeChanged;

                    if (comparedType != MtColumnComparedType.None)
                        items.Add(new MtColumnComparedItem { Old = oldCol, New = newCol, Type = comparedType });
                }
            });

            return items.ToArray();
        }

        private static bool _CompareColumnSize(DbColumnType columnType, int size1, int size2)
        {
            DbColumnTypeInfo typeInfo = DbColumnTypeInfo.GetTypeInfo(columnType);
            if (typeInfo == null || typeInfo.IsVarLen)
                return size1 == size2;

            return true;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="dbQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="dropIfExists">如果表已经存在，则先将其删除</param>
        public static void CreateTable(DbQuery dbQuery, string tableName, MtColumn[] columns, bool dropIfExists = false)
        {
            Contract.Requires(dbQuery != null && tableName != null && columns != null);

            _ValidateColumns(columns);
            _CreateTable(dbQuery, tableName, columns, dropIfExists);
        }

        // 创建表
        private static void _CreateTable(DbQuery dbQuery, string tableName, MtColumn[] columns, bool dropIfExists)
        {
            StringBuilder sql = new StringBuilder();
            LogItemCollection logItems = new LogItemCollection();

            if (dropIfExists)
            {
                sql.Append("If Object_Id('[").Append(tableName).Append("]') Is Not Null And Not Exists (Select Null From [")
                    .Append(tableName).Append("]) ").AppendLine();
                sql.Append("  Drop Table [").Append(tableName).AppendLine("];");

                logItems.AppendMessageFormat("删除表“{0}”", tableName);
            }

            sql.Append("Create Table [").Append(tableName).AppendLine("] (");

            columns = columns.OrderByDescending(col => col.IndexType).ThenBy(col => col.Name).ToArray();
            for (int i = 0; i < columns.Length; i++)
            {
                if (i > 0)
                    sql.AppendLine(", ");

                MtColumn column = columns[i];
                _ValidateColumn(column);
                AppendMsSqlColumnSql(sql, column);
            }

            sql.AppendLine().AppendLine(");");

            foreach (MtColumn column in columns)
            {
                if (column.IndexType == DbColumnIndexType.None)
                    continue;

                _AppendIndexSql(sql, tableName, column, logItems);
            }

            logItems.AppendMessageFormat("创建表“{0}”", tableName);
            dbQuery.ExecuteNonQuery(sql.ToString());

            logItems.Log();
        }

        private static void _ValidateColumns(MtColumn[] columns)
        {
            columns.FindRepeated(col => col.Name, (col) => {
                throw new DbException("列名“" + col.Name + "”重复");
            }, IgnoreCaseEqualityComparer.Instance);

            if (columns.Count(col => col.IndexType == DbColumnIndexType.Master) >= 2)
                throw new DbException("存在多个主查询索引");

            columns.ForEach(_ValidateColumn);
        }

        private static void _ValidateColumn(MtColumn column)
        {
            if (string.IsNullOrEmpty(column.Name))
                throw new DbException("列名“" + column.Name + "”为空");
        }

        /// <summary>
        /// 加载指定表的列信息
        /// </summary>
        /// <param name="dbQuery"></param>
        /// <param name="tableNames"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static MtColumn[] LoadMtColumns(DbQuery dbQuery, string tableName, out int rowCount)
        {
            Contract.Requires(dbQuery != null && tableName != null);

            MtTableItem[] items = _LoadMtColumns(dbQuery, new[] { tableName });
            if (items.IsNullOrEmpty())
            {
                rowCount = 0;
                return null;
            }

            MtTableItem mt = items[0];
            rowCount = mt.RowCount;
            return mt.Columns.ToArray();
        }

        private static MtTableItem[] _LoadMtColumns(DbQuery dbQuery, string[] tableNames)
        {
            Contract.Requires(tableNames != null);

            if (tableNames.Length == 0)
                return Array<MtTableItem>.Empty;

            string sqlFormat =

                // 表名
                "Select Name As TableName From sys.tables Where name In ( {0} );\r\n"

                // 行数
                + "Select t.Name As TableName, [Rows] From sys.sysindexes idx Join sys.tables t On idx.id = t.object_id Where t.name In ( {0} ) and [Indid] < 2; \r\n"

                // 索引
                + "Select t.Name As TableName, ix.type As IndexType, ix.type_desc As IndexTypeDesc, c.Name As ColumnName From "
                + "sys.indexes ix Join sys.index_columns ixc On ix.object_id = ixc.object_id And ix.index_id = ixc.index_id "
                + "Join sys.all_columns c On c.object_id = ixc.object_id And c.column_id = ixc.column_id "
                + "Join sys.tables t on ixc.object_id = t.object_id Where t.Name In ( {0} ); "

                // 列
                + "Select t.Name As TableName, c.Name As ColumnName, st.Name As ColumnTypeName, c.max_length As ColumnSize From "
                + "sys.tables t Join sys.all_columns c On t.object_id = c.object_id Join sys.systypes st On c.system_type_id = st.xusertype "
                + "Where t.Name In ( {0} );\r\n";


            string sql = string.Format(sqlFormat,
                tableNames.Distinct(IgnoreCaseEqualityComparer.Instance).Select(n => "'" + ReviseString(n) + "'").JoinBy(", ")
            );

            using (IDataReader r = dbQuery.ExecuteReader(sql))
            {
                IgnoreCaseDictionary<MtTableItem> dict = new IgnoreCaseDictionary<MtTableItem>();

                // 表
                while (r.Read())
                {
                    string tableName = r["TableName"].ToType<string>();
                    dict.Add(tableName, new MtTableItem { TableName = tableName, Columns = new List<MtColumn>() });
                }

                // 行数
                r.NextResult();
                while (r.Read())
                {
                    MtTableItem mtTableItem;
                    string tableName = r["TableName"].ToType<string>();
                    if (dict.TryGetValue(tableName, out mtTableItem))
                        mtTableItem.RowCount = r["Rows"].ToType<int>();
                }

                // 索引
                r.NextResult();
                Dictionary<Tuple<string, string>, DbColumnIndexType> indexDict = new Dictionary<Tuple<string, string>, DbColumnIndexType>();
                while (r.Read())
                {
                    int indexTypeNum = r["IndexType"].ToType<int>();
                    DbColumnIndexType indexType = (indexTypeNum == 1) ? DbColumnIndexType.Master
                        : (indexTypeNum == 2) ? DbColumnIndexType.Slave : DbColumnIndexType.None;

                    string tableName = r["TableName"].ToType<string>();
                    MtTableItem mtTableItem;
                    if (dict.TryGetValue(tableName, out mtTableItem))
                    {
                        string columnName = r["ColumnName"].ToType<string>();
                        indexDict.Add(new Tuple<string, string>(tableName.ToLower(), columnName.ToLower()), indexType);
                    }
                }

                // 列
                r.NextResult();
                while (r.Read())
                {
                    string tableName = r["TableName"].ToType<string>();
                    string columnTypeName = r["ColumnTypeName"].ToType<string>();
                    string columnName = r["ColumnName"].ToType<string>();
                    DbColumnIndexType indexType;
                    indexDict.TryGetValue(new Tuple<string, string>(tableName.ToLower(), columnName.ToLower()), out indexType);

                    MtColumn col = new MtColumn(columnName, "", ToDbColumnType(columnTypeName),
                        _ReviseSize(columnTypeName, r["ColumnSize"].ToType<int>()), indexType
                    );

                    MtTableItem mtTableItem;
                    if (dict.TryGetValue(tableName, out mtTableItem))
                        mtTableItem.Columns.Add(col);
                }

                return dict.Values.ToArray();
            }
        }

        private static int _ReviseSize(string sqlType, int size)
        {
            DbColumnType colType = ToDbColumnType(sqlType);
            DbColumnTypeInfo colTypeInfo = DbColumnTypeInfo.GetTypeInfo(colType);

            if (colTypeInfo == null)
                return size;

            if (!colTypeInfo.IsVarLen)
                return colTypeInfo.ElementSize;

            if (size < 0)
                return 0;

            return size / colTypeInfo.ElementSize;
        }

        /// <summary>
        /// 将SqlServer列类型转换为标准列类型
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static DbColumnType ToDbColumnType(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "varchar":
                case "char":
                case "text":
                    return DbColumnType.AnsiString;

                case "nvarchar":
                case "nchar":
                case "ntext":
                    return DbColumnType.String;

                case "tinyint":
                    return DbColumnType.Int8;

                case "smallint":
                    return DbColumnType.Int16;

                case "int":
                    return DbColumnType.Int32;

                case "bigint":
                    return DbColumnType.Int64;

                case "datetime":
                case "smalldatetime":
                    return DbColumnType.DateTime;

                case "decimal":
                case "numeric":
                    return DbColumnType.Decimal;

                case "real":
                    return DbColumnType.Single;

                case "float":
                    return DbColumnType.Double;

                case "bit":
                    return DbColumnType.Boolean;

                case "uniqueidentifier":
                    return DbColumnType.Guid;

                case "binary":
                case "varbinary":
                case "image":
                    return DbColumnType.Binary;

                default:
                    return DbColumnType.Unknown;
            };
        }

        /// <summary>
        /// 转换为Sql-Server列类型
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static string ToColumnTypeName(DbColumnType columnType)
        {
            switch (columnType)
            {
                case DbColumnType.Int8:
                    return "TinyInt";

                case DbColumnType.Int16:
                    return "SmallInt";

                case DbColumnType.Int32:
                    return "Int";

                case DbColumnType.Int64:
                    return "BigInt";

                case DbColumnType.Single:
                    return "Real";

                case DbColumnType.Double:
                    return "Float";

                case DbColumnType.Decimal:
                    return "Decimal";

                case DbColumnType.AnsiString:
                    return "Varchar";

                case DbColumnType.String:
                    return "NVarchar";

                case DbColumnType.Boolean:
                    return "Bit";

                case DbColumnType.DateTime:
                    return "DateTime";

                case DbColumnType.Guid:
                    return "UniqueIdentifier";

                case DbColumnType.Binary:
                    return "VarBinary";

                default:
                    throw new NotSupportedException("不支持数据类型“" + columnType + "”");
            }
        }

        /// <summary>
        /// 是否为变长类型
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static bool IsVarLenType(DbColumnType columnType)
        {
            DbColumnTypeInfo typeInfo = DbColumnTypeInfo.GetTypeInfo(columnType);
            return typeInfo != null && typeInfo.IsVarLen;
        }

        /// <summary>
        /// 修正字符串，将单引号替换为两个单引号
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReviseString(string s)
        {
            return s == null ? null : s.Replace("'", "''");
        }

        internal static void ValidateConnectionType(MtConnectionPoint conPoint)
        {
            Contract.Requires(conPoint != null);

            if (conPoint.Connection.ConType != DbConnectionTypes.MsSql)
                throw new ArgumentException(string.Format("连接类型必须为“{0}”", DbConnectionTypes.MsSql));
        }
    }
}
