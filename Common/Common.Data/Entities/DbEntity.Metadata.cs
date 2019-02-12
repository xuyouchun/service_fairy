using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Common.Utility;
using Common.Data.UnionTable;
using Common.Collection;
using System.Collections.ObjectModel;
using Common.Data.UnionTable.Metadata;
using System.Diagnostics.Contracts;

namespace Common.Data.Entities
{
    partial class DbEntity<TEntity>
    {
        static DbEntity()
        {
            _w = new Lazy<_MtWrapper>(_LoadWrapper);
        }

        private static readonly Lazy<_MtWrapper> _w;

        private static _MtWrapper _LoadWrapper()
        {
            _MtWrapper w = new _MtWrapper();
            w.TableInfo = _MtUtility.GetTableInfo();

            foreach (_ColumnInfo field in _MtUtility.LoadColumnInfos())
            {
                w.ColumnDict.Add(field.FullName, field);

                _MtUtility.ValidateColumn(w, field);
                _MtUtility.SetPrimaryKey(w, field);
                _MtUtility.SetRouteKey(w, field);
            }

            if (w.RouteKeyColumn == null)
                throw new DbEntityException("未指定路由键");

            if (w.PrimaryKeyColumn == null)
                w.PrimaryKeyColumn = w.RouteKeyColumn;

            w.TableGroupInfo = _MtUtility.GetTableGroupInfo();
            w.ColumnGroupDict = w.ColumnDict.Values.Where(f => f.ColumnKind == DbColumnKind.Normal).GroupBy(f => f.GroupName)
                .ToIgnoreCaseDictionary(g => g.Key, g => g.ToArray());

            w.AllColumnInfos = w.ColumnDict.Values.OrderByDescending(v => v.IndexType).ThenBy(v => v.FullName)
                .Select(v => v.ColumnInfo).AsReadOnly();

            w.GroupNames = w.ColumnGroupDict.Keys.AsReadOnly();
            w.MtColumns = _MtUtility.GetMtColumns(w).AsReadOnly();

            w.DbEntityTableInfo = new DbEntityTableInfo(w.TableInfo.Name, w.TableInfo.DefaultGroup, w.PrimaryKeyColumn.ColumnInfo, w.TableInfo.InitTableCount,
                w.TableGroupInfo.TableGroupName, w.TableGroupInfo.RouteType, w.TableGroupInfo.RouteArgs, w.RouteKeyColumn.ColumnInfo);

            return w;
        }

        #region Class _MtWrapper ...

        class _MtWrapper
        {
            public Dictionary<string, _ColumnInfo> ColumnDict = new IgnoreCaseDictionary<_ColumnInfo>();
            public Dictionary<string, _ColumnInfo[]> ColumnGroupDict;
            public ReadOnlyCollection<string> GroupNames;
            public ReadOnlyCollection<ColumnInfo> AllColumnInfos;
            public _ColumnInfo PrimaryKeyColumn, RouteKeyColumn;
            public _TableInfo TableInfo;
            public _TableGroupInfo TableGroupInfo;
            public ReadOnlyCollection<MtColumn> MtColumns;
            public DbEntityTableInfo DbEntityTableInfo;
        }

        #endregion

        #region Class _MtUtility ...

        static class _MtUtility
        {
            public static void SetPrimaryKey(_MtWrapper w, _ColumnInfo column)
            {
                if (column.ColumnKind.HasFlag(DbColumnKind.PrimaryKey))
                {
                    if (w.PrimaryKeyColumn != null)
                        throw new DbEntityException(string.Format("主键只允许指定一个，表“{0}”", w.TableInfo.Name));

                    w.PrimaryKeyColumn = column;
                }
            }

            public static void SetRouteKey(_MtWrapper w, _ColumnInfo column)
            {
                if (column.ColumnKind.HasFlag(DbColumnKind.RouteKey))
                {
                    if (w.RouteKeyColumn != null)
                        throw new DbEntityException(string.Format("路由键只允许指定一个，表“{0}”", w.TableInfo.Name));

                    w.RouteKeyColumn = column;
                }
            }

            public static void ValidateColumn(_MtWrapper w, _ColumnInfo column)
            {
                if (!column.PropertyInfo.CanRead)
                    throw new DbEntityException(string.Format("字段“{0}”不可读取，表“{1}”", column.ColumnName, w.TableInfo.Name));

                if (!column.PropertyInfo.CanWrite)
                    throw new DbEntityException(string.Format("字段“{0}”不可写入，表“{1}”", column.ColumnName, w.TableInfo.Name));

                if (column.ColumnKind.HasFlag(DbColumnKind.PrimaryKey) && !string.IsNullOrEmpty(column.GroupName))
                    throw new DbEntityException(string.Format("主键“{0}”不允许指定组名，表“{1}”", column.ColumnName, w.TableInfo.Name));

                if (column.ColumnKind.HasFlag(DbColumnKind.RouteKey) && !string.IsNullOrEmpty(column.GroupName))
                    throw new DbEntityException(string.Format("路由键“{0}”不允许指定组名，表“{1}”", column.ColumnName, w.TableInfo.Name));

                if (column.ColumnKind == DbColumnKind.Normal && string.IsNullOrEmpty(column.GroupName))
                    throw new DbEntityException(string.Format("普通字段“{0}”必须指定组名，表“{1}”", column.ColumnName, w.TableInfo.Name));

                if (column.IndexType == DbColumnIndexType.Master && !column.ColumnKind.HasFlag(DbColumnKind.RouteKey))
                    throw new DbEntityException(string.Format("只有路由键可以定义主查询索引，表“{0}”", w.TableInfo.Name));
            }

            public static IEnumerable<_ColumnInfo> LoadColumnInfos()
            {
                foreach (PropertyInfo pInfo in typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (pInfo.IsDefined(typeof(DbColumnAttributeBase), true))
                        yield return LoadColumnInfo(pInfo);
                }
            }

            public static _ColumnInfo LoadColumnInfo(PropertyInfo pInfo)
            {
                DbColumnAttributeBase attr = DbColumnAttributeBase.Combine(pInfo.GetAttributes<DbColumnAttributeBase>(true));

                string columnName = attr.FieldName ?? pInfo.Name;
                _ColumnInfo fInfo = new _ColumnInfo {
                    DefaultValue = attr.DefaultValue,
                    NullValue = _ConvertNullValue(attr.NullValue, pInfo, columnName),
                    PropertyInfo = pInfo,
                    ColumnKind = attr.ColumnKind,
                    GroupName = attr.GroupName ?? "",
                    ColumnName = columnName,
                    IndexType = attr.IndexType,
                };

                fInfo.FullName = UtUtility.BuildFullColumnName(fInfo.GroupName, fInfo.ColumnName);
                DbColumnType columnType = (attr.ColumnType == DbColumnType.Unknown) ? fInfo.PropertyInfo.PropertyType.ToDbColumnType() : attr.ColumnType;
                fInfo.ColumnInfo = new ColumnInfo(fInfo.FullName, columnType, _GetSize(columnType, attr.Size));
                return fInfo;
            }

            private static object _ConvertNullValue(object nullValue, PropertyInfo pInfo, string columnName)
            {
                if (nullValue == null)
                    return null;

                try
                {
                    return nullValue.ToTypeWithError(pInfo.PropertyType);
                }
                catch (Exception ex)
                {
                    throw new FormatException(string.Format("数据库表实体字段“{0}.{1}”的默认值错误：{2}", TableName, columnName, ex.Message));
                }
            }

            private static int _GetSize(DbColumnType columnType, int size)
            {
                if (size >= 0)
                    return size;

                switch (columnType)
                {
                    case DbColumnType.AnsiString:
                    case DbColumnType.String:
                        return 16;

                    case DbColumnType.Binary:
                        return 0;
                }

                return 0;
            }

            public static _TableInfo GetTableInfo()
            {
                DbEntityAttribute attr = typeof(TEntity).GetAttribute<DbEntityAttribute>(true);
                if (attr == null)
                    throw new DbEntityException("实体类" + typeof(TEntity) + "未标有DbEntityAttribute属性");

                string tableName = (attr == null || string.IsNullOrEmpty(attr.TableName)) ? typeof(TEntity).Name : attr.TableName;
                int initTableCount = (attr == null) ? 1 : attr.InitTableCount;
                return new _TableInfo { Name = tableName, InitTableCount = initTableCount, DefaultGroup = attr.DefaultGroup };
            }

            public static _TableGroupInfo GetTableGroupInfo()
            {
                DbTableGroupEntityAttribute attr = typeof(TEntity).GetAttribute<DbTableGroupEntityAttribute>(true);
                if (attr == null)
                    return new _TableGroupInfo { TableGroupName = typeof(TEntity).Name, RouteType = DbRouteType.Mod };
                else
                    return new _TableGroupInfo { TableGroupName = attr.TableGroupName, RouteType = (attr.RouteType == DbRouteType.Unknown) ? DbRouteType.Mod : attr.RouteType, RouteArgs = attr.RouteArgs };
            }

            public static _ColumnInfo GetFieldInfo(string name, bool throwError)
            {
                _ColumnInfo fInfo;
                if (!_w.Value.ColumnDict.TryGetValue(name, out fInfo) && throwError)
                    throw new DbEntityException(string.Format("不存在字段“{0}”", name));

                return fInfo;
            }

            public static _ColumnInfo[] GetFieldInfosOfGroup(string group)
            {
                return _w.Value.ColumnGroupDict.GetOrDefault(group);
            }

            // 创建表的元数据
            public static IList<MtColumn> GetMtColumns(_MtWrapper w)
            {
                List<MtColumn> columns = new List<MtColumn>();
                foreach (_ColumnInfo ci in w.ColumnDict.Values)
                {
                    MtColumn cln = new MtColumn(ci.FullName, ci.ColumnInfo.Type, ci.ColumnInfo.Size, ci.IndexType);
                    columns.Add(cln);
                }

                return columns;
            }
        }

        #endregion

        #region Class _ColumnInfo ...

        class _ColumnInfo
        {
            public object DefaultValue, NullValue;
            public PropertyInfo PropertyInfo;
            public DbColumnKind ColumnKind;
            public string GroupName, ColumnName, FullName;
            public DbColumnIndexType IndexType;
            public ColumnInfo ColumnInfo;

            public void SetValue(object entity, object value)
            {
                if (value == null || value == DBNull.Value)
                    value = NullValue;

                PropertyInfo.SetValue(entity, value.ToTypeWithError(PropertyInfo.PropertyType), null);
            }

            public object GetValue(object entity)
            {
                object value = PropertyInfo.GetValue(entity, null);
                return object.Equals(value, NullValue) ? null : value;
            }
        }

        #endregion

        #region Class _TableInfo ...

        class _TableInfo
        {
            public string Name;

            public string Desc;

            public string DefaultGroup;

            public int InitTableCount;
        }

        #endregion

        #region Class _TableGroupInfo ...

        class _TableGroupInfo
        {
            public string TableGroupName;

            public DbRouteType RouteType;

            public string RouteArgs;
        }

        #endregion

        private static string _tableName, _fullTableName;

        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName { get { return _tableName ?? (_tableName = _w.Value.TableInfo.Name); } }

        /// <summary>
        /// 全表名
        /// </summary>
        public static string FullTableName { get { return _fullTableName ?? (_fullTableName = _w.Value.TableGroupInfo.TableGroupName + "." + TableName); } }

        /// <summary>
        /// 主键字段名称
        /// </summary>
        public static ColumnInfo PrimaryKeyColumnInfo { get { return _w.Value.PrimaryKeyColumn.ColumnInfo; } }

        /// <summary>
        /// 路由键字段名称
        /// </summary>
        public static ColumnInfo RouteKeyColumnInfo { get { return _w.Value.RouteKeyColumn.ColumnInfo; } }

        /// <summary>
        /// 列组
        /// </summary>
        public static ReadOnlyCollection<string> ColumnGroups { get { return _w.Value.GroupNames; } }

        /// <summary>
        /// 列信息
        /// </summary>
        public static ReadOnlyCollection<ColumnInfo> AllColumnInfos { get { return _w.Value.AllColumnInfos; } }

        /// <summary>
        /// 全部的列
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllColumns()
        {
            return AllColumnInfos.ToArray(ci => ci.Name);
        }

        /// <summary>
        /// 表的元数据
        /// </summary>
        public static ReadOnlyCollection<MtColumn> MtColumns { get { return _w.Value.MtColumns; } }

        /// <summary>
        /// 获取指定组的列
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string[] GetColumnsOfGroup(string group)
        {
            Contract.Requires(group != null);

            return GetColumnInfosOfGroup(group).ToArray(c => c.Name);
        }

        /// <summary>
        /// 获取指定组的列
        /// </summary>
        /// <param name="group">组名</param>
        /// <returns>该组的所有列信息</returns>
        public static ColumnInfo[] GetColumnInfosOfGroup(string group)
        {
            Contract.Requires(group != null);

            _ColumnInfo[] infos;
            if (!_w.Value.ColumnGroupDict.TryGetValue(group, out infos))
                return Array<ColumnInfo>.Empty;

            return infos.ToArray(info => info.ColumnInfo);
        }

        /// <summary>
        /// 获取指定名称的列信息
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="throwError">是否在不存在该列时抛出异常</param>
        /// <returns>列信息</returns>
        public static ColumnInfo GetColumnInfo(string column, bool throwError = true)
        {
            Contract.Requires(column != null);

            _ColumnInfo colInfo;
            if (_w.Value.ColumnDict.TryGetValue(column, out colInfo))
                return colInfo.ColumnInfo;

            if (throwError)
                throw new DbEntityException(string.Format("列“{0}”不存在", column));

            return null;
        }

        /// <summary>
        /// 获取指定名称的列信息
        /// </summary>
        /// <param name="columns">列名</param>
        /// <param name="throwError">是否不存在该列名时抛出异常</param>
        /// <returns>列信息</returns>
        public static ColumnInfo[] GetColumnInfos(string[] columns, bool throwError = true)
        {
            if (columns == null)
                return AllColumnInfos.ToArray();

            return columns.ToArray(col => GetColumnInfo(col, throwError));
        }

        /// <summary>
        /// 获取指定组的列
        /// </summary>
        /// <param name="groups">组名</param>
        /// <param name="option">选项</param>
        /// <returns>该组名中的所有列名</returns>
        public static string[] GetColumnsOfGroups(string[] groups, DbEntityGroupOption option)
        {
            if (groups == null)
                return AllColumnInfos.ToArray(ci => ci.Name);

            HashSet<string> hs = new HashSet<string>(IgnoreCaseEqualityComparer.Instance);
            for (int k = 0; k < groups.Length; k++)
            {
                hs.AddRange(GetColumnsOfGroup(groups[k]));
            }

            if (option.HasFlag(DbEntityGroupOption.WithPrimaryKey))
                hs.Add(PrimaryKeyColumnInfo.Name);

            if (option.HasFlag(DbEntityGroupOption.WithRouteKey))
                hs.Add(RouteKeyColumnInfo.Name);

            return hs.ToArray();
        }

        /// <summary>
        /// 获取主键与路由键的列名数组
        /// </summary>
        /// <returns></returns>
        private static string[] _GetSpecialColumns()
        {
            string pkName = PrimaryKeyColumnInfo.Name, rtName = RouteKeyColumnInfo.Name;
            if (pkName.IgnoreCaseEqualsTo(rtName))
                return new[] { pkName };

            return new[] { pkName, rtName };
        }

        /// <summary>
        /// 是否为路由键
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsRouteKey(string column)
        {
            return RouteKeyColumnInfo.Name.IgnoreCaseEqualsTo(column);
        }

        /// <summary>
        /// 是否为主键
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsPrimaryKey(string column)
        {
            return PrimaryKeyColumnInfo.Name.IgnoreCaseEqualsTo(column);
        }

        /// <summary>
        /// 获取全部的列
        /// </summary>
        /// <returns></returns>
        public override ReadOnlyCollection<MtColumn> GetColumns()
        {
            return MtColumns;
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <returns></returns>
        public override DbEntityTableInfo GetTableInfo()
        {
            return _w.Value.DbEntityTableInfo;
        }
    }
}
