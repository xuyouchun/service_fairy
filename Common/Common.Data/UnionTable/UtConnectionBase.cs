using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Data.UnionTable.Metadata;
using Common.Utility;
using Common.Collection;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 表连接的基类
    /// </summary>
    abstract partial class UtConnectionBase : IUtConnection
    {
        public UtConnectionBase(UtContext utCtx, int partialTableIndex = -1)
        {
            Contract.Requires(utCtx != null);

            UtContext = utCtx;
            PartialTableIndex = partialTableIndex;
            MtTable = utCtx.MtTable;
            TableInfo = utCtx.TableInfo;
            Route = utCtx.Route;
            Schema = utCtx.Schema;

            PrimaryKey = MtTable.PrimaryKey;
            PrimaryKeyColumnType = MtTable.PrimaryKeyColumnType;
            RouteKey = MtTable.Owner.RouteKey;
            RouteKeyColumnType = MtTable.Owner.RouteKeyColumnType;
            TableGroupName = MtTable.Owner.Name;
        }

        /// <summary>
        /// 表的执行上下文
        /// </summary>
        protected UtContext UtContext { get; private set; }

        /// <summary>
        /// 表的元数据
        /// </summary>
        protected MtTable MtTable { get; private set; }

        /// <summary>
        /// 表信息
        /// </summary>
        protected TableInfo TableInfo { get; private set; }

        /// <summary>
        /// 路由器
        /// </summary>
        protected IUtRoute Route { get; private set; }

        /// <summary>
        /// 表架构
        /// </summary>
        protected IUtSchema Schema { get; private set; }

        /// <summary>
        /// 路由键
        /// </summary>
        protected string RouteKey { get; private set; }

        /// <summary>
        /// 路由键类型
        /// </summary>
        protected DbColumnType RouteKeyColumnType { get; private set; }

        /// <summary>
        /// 主键
        /// </summary>
        protected string PrimaryKey { get; private set; }

        /// <summary>
        /// 主键类型
        /// </summary>
        protected DbColumnType PrimaryKeyColumnType { get; private set; }

        /// <summary>
        /// 表组名
        /// </summary>
        protected string TableGroupName { get; private set; }

        /// <summary>
        /// 分表索引号
        /// </summary>
        protected int PartialTableIndex { get; private set; }

        /// <summary>
        /// 表的全名
        /// </summary>
        protected string GetFullTableName(string groupName = null)
        {
            string s = TableGroupName + "." + MtTable.Name;
            if (!string.IsNullOrEmpty(groupName))
                s += "." + groupName;

            if (PartialTableIndex >= 0)
                s += "." + PartialTableIndex.ToString().PadLeft(4, '0');

            return s;
        }

        /// <summary>
        /// 获取路由列索引号
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public int GetRouteKeyIndex(DataList dataList, bool throwError = true)
        {
            Contract.Requires(dataList != null);

            int index = dataList.GetColumnIndex(RouteKey);
            if (index < 0 && throwError)
                throw new DbException("数据集中未包含路由键");

            return index;
        }

        /// <summary>
        /// 获取主键索引号
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public int GetPrimaryKeyIndex(DataList dataList, bool throwError = true)
        {
            Contract.Requires(dataList != null);

            int index = dataList.GetColumnIndex(PrimaryKey);
            if (index < 0 && throwError)
                throw new DbException("数据集中未包含主键列");

            return index;
        }

        protected static bool ValidateMask(IConnectionPoint point, long mask)
        {
            return (mask & (1L << point.PartialTableIndex)) != 0;
        }

        /// <summary>
        /// 将数据按连接点分开
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IDictionary<IConnectionPoint, DataList> SplitByRouteKey(DataList dataList, UtInvokeSettings settings)
        {
            int rtIndex = GetRouteKeyIndex(dataList);
            var dict = new Dictionary<IConnectionPoint, List<DataListRow>>();

            long mask = (settings ?? UtInvokeSettings.Default).PartialTableMask;
            for (int k = 0, len = dataList.Rows.Length; k < len; k++)
            {
                DataListRow row = dataList.Rows[k];
                object routeKey = row.Cells[rtIndex];
                if (routeKey == null)
                    throw new DbException(string.Format("第{0}行未指定路由键", k + 1));

                IConnectionPoint conPoint = Route.GetConnectionPoint(MtTable, routeKey, DbConnectionUsageType.Data);
                if (ValidateMask(conPoint, mask))
                    dict.GetOrSet(conPoint).Add(row);
            }

            return dict.ToDictionary(
                item => item.Key,
                item => new DataList(dataList.Name, dataList.Columns, item.Value.ToArray(), dataList.PrimaryKey)
            );
        }

        /// <summary>
        /// 获取指定路由键的连接
        /// </summary>
        /// <param name="routeKey"></param>
        /// <returns></returns>
        protected IConnectionPoint GetConnectionPoint(object routeKey)
        {
            IConnectionPoint conPoint = Route.GetConnectionPoint(MtTable, routeKey, DbConnectionUsageType.Data);
            if (conPoint == null)
                throw new DbException(string.Format("无法获取路由键为“{0}”的连接", routeKey));

            return conPoint;
        }

        /// <summary>
        /// 将路由键按连接点分组
        /// </summary>
        /// <param name="routeKeys"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IDictionary<IConnectionPoint, object[]> SplitByRouteKey(object[] routeKeys, UtInvokeSettings settings)
        {
            Contract.Requires(routeKeys != null);

            var dict = new Dictionary<IConnectionPoint, HashSet<object>>();

            long mask = (settings ?? UtInvokeSettings.Default).PartialTableMask;
            for (int k = 0; k < routeKeys.Length; k++)
            {
                object routeKey = routeKeys[k];
                IConnectionPoint conPoint = Route.GetConnectionPoint(MtTable, routeKey, DbConnectionUsageType.Data);
                if (ValidateMask(conPoint, mask))
                    dict.GetOrSet(conPoint).Add(routeKey);
            }

            return dict.ToDictionary(
                item => item.Key,
                item => item.Value.ToArray()
            );
        }

        /// <summary>
        /// 按列组拆分
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        protected IDictionary<string, DataList> SplitByColumnGroup(DataList dataList)
        {
            GroupInfo[] gInfos = AnalyzeGroupInfos(dataList);

            foreach (GroupInfo gInfo in gInfos)
            {
                DataListBuilder db = new DataListBuilder(dataList.Name, dataList.PrimaryKey);
                gInfo.Tag = db;
                db.AddColumns(gInfo.Columns.Select(ci => new ColumnInfo(ci.ColumnName, ci.ColumnType, ci.Column.Size)));
                db.AddRows(gInfo.GetValueArrays(dataList));
            }

            return gInfos.ToDictionary(gInfo => gInfo.GroupName, gInfo => ((DataListBuilder)gInfo.Tag).ToDataList());
        }

        /// <summary>
        /// 将主键为空的列填入主键
        /// </summary>
        /// <param name="dataList"></param>
        protected void AssignPrimaryKeys(DataList dataList)
        {
            int pkIndex = GetPrimaryKeyIndex(dataList);
            foreach (DataListRow r in dataList.Rows)
            {
                if (r.Cells[pkIndex] == null)
                    r.Cells[pkIndex] = Schema.GenerateNewKey();
            }
        }

        /// <summary>
        /// 获取列元数据
        /// </summary>
        /// <param name="column"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        protected MtColumn GetMtColumn(string column, bool throwError = true)
        {
            Contract.Requires(column != null);

            MtColumn mtColumn = MtTable.Columns.Get(column);
            if (mtColumn == null && throwError)
                throw new DbException(string.Format("列“{0}”在表“{1}”中不存在", column, MtTable.Name));

            return mtColumn;
        }

        /// <summary>
        /// 获取列元数据
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        protected MtColumn[] GetMtColumns(string[] columns, bool throwError = true)
        {
            return (columns ?? GetAllColumns()).Distinct(IgnoreCaseEqualityComparer.Instance).ToArrayNotNull(c => GetMtColumn(c, throwError));
        }

        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="column"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        protected ColumnInfo GetColumnInfo(string column, bool throwError = true)
        {
            Contract.Requires(column != null);

            MtColumn mtColumn = GetMtColumn(column, throwError);
            return mtColumn == null ? null : mtColumn.ToColumnInfo();
        }

        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        protected ColumnInfo[] GetColumnInfos(string[] columns, bool throwError = true)
        {
            return GetMtColumns(columns, throwError).ToArray(mtColumn => mtColumn.ToColumnInfo());
        }

        /// <summary>
        /// 获取组中的列信息
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        protected ColumnInfo[] GetColumnInfosOfGroup(string group)
        {
            return TableInfo.ColumnInfos.WhereToArray(ci => UtUtility.GetGroupName(ci.Name).IgnoreCaseEqualsTo(group));
        }

        /// <summary>
        /// 创建空的数据集
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        protected DataList CreateEmptyDataList(string[] columns, bool throwError = true)
        {
            return new DataList(GetFullTableName(),
                GetColumnInfos(columns, throwError), Array<DataListRow>.Empty, MtTable.PrimaryKey);
        }

        /// <summary>
        /// 创建DataListBuilder
        /// </summary>
        /// <returns></returns>
        protected DataListBuilder CreateDataListBuilder()
        {
            return new DataListBuilder(GetFullTableName(), PrimaryKey);
        }

        /// <summary>
        /// 获取所有的列名
        /// </summary>
        /// <returns></returns>
        protected string[] GetAllColumns()
        {
            return MtTable.Columns.AllNames.ToArray();
        }

        /// <summary>
        /// 获取所有的组名
        /// </summary>
        /// <returns></returns>
        protected string[] GetAllGroups()
        {
            return TableInfo.AllGroups.ToArray();
        }

        /// <summary>
        /// 判断是否为主键或路由键
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected bool IsSpecialColumn(string column)
        {
            return IsPrimaryKey(column) || IsRouteKey(column);
        }

        /// <summary>
        /// 是否为主键
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool IsPrimaryKey(string column)
        {
            return column.IgnoreCaseEqualsTo(PrimaryKey);
        }

        /// <summary>
        /// 是否为路由键
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool IsRouteKey(string column)
        {
            return column.IgnoreCaseEqualsTo(RouteKey);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="columns">列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public abstract DataList Select(object[] routeKeys, string[] columns, DbSearchParam param, UtInvokeSettings settings);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="autoUpdate">当数据已经存在时，是否将其更新</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public abstract int Insert(DataList data, bool autoUpdate, UtInvokeSettings settings);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public abstract int Update(DataList data, object[] routeKeys, string where, UtInvokeSettings settings);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="where">删除条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public abstract int Delete(DataList data, string where, UtInvokeSettings settings);

        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="routeKey">路由键</param>
        /// <param name="data">需要合并的数据</param>
        /// <param name="compareColumns">用于比较的列</param>
        /// <param name="where">过滤条件</param>
        /// <param name="option">合并选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public abstract int Merge(object routeKey, DataList data, string[] compareColumns, string where,
            UtConnectionMergeOption option, UtInvokeSettings settings);
    }
}
