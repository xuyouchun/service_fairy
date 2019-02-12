using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 表的连接
    /// </summary>
    class UtConnection : UtConnectionBase
    {
        public UtConnection(UtContext utCtx, IUtProvider utProvider)
            : base(utCtx, -1)
        {
            _utProvider = utProvider;
        }

        private readonly IUtProvider _utProvider;

        /// <summary>
        /// 获取所有的连接
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private IConnectionPoint[] _GetAllConnectionPoints(UtInvokeSettings settings)
        {
            IConnectionPoint[] points = _utProvider.ConnectionPointManager.GetAllTableConnectionPoints(MtTable, DbConnectionUsageType.Data);

            long mask;
            if (settings == null || (mask = settings.PartialTableMask) == -1)
                return points;

            List<IConnectionPoint> arr = new List<IConnectionPoint>();
            for (int k = 0; k < points.Length; k++)
            {
                IConnectionPoint point = points[k];
                if (ValidateMask(point, mask))
                    arr.Add(point);
            }

            return arr.ToArray();
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询条件</param>
        /// <param name="columns">列</param>
        /// <param name="settings">调用设置</param>
        /// <returns>查询结果</returns>
        public override DataList Select(object[] routeKeys, string[] columns, DbSearchParam param, UtInvokeSettings settings)
        {
            if (routeKeys == null)  // 在所有分表中选取
                return _SelectFromAllPartialTables(param, columns, settings);

            if (routeKeys.Length == 0)
                return CreateEmptyDataList(columns);

            // 从指定路由的分表中选取
            return _SelectByRouteKeys(routeKeys, param, columns, settings);
        }

        // 从所有分表中选取
        private DataList _SelectFromAllPartialTables(DbSearchParam param, string[] columns, UtInvokeSettings settings)
        {
            DataListBuilder builder = CreateDataListBuilder();
            foreach (QueryTask task in _GetAllConnectionPointsWithParam(new QueryTaskContext { Builder = builder, Param = param }, settings))
            {
                DataList dataList = task.ConnectionPoint.CreateTableConnection(UtContext).Select(null, columns, task.Param, settings);
                builder.AddDataList(dataList);
            }

            return _ToDataList(builder, columns);
        }

        private IEnumerable<QueryTask> _GetAllConnectionPointsWithParam(QueryTaskContext ctx, UtInvokeSettings settings)
        {
            DbSearchParam param = ctx.Param;
            int totalCount = (param == null ? -1 : param.Count);
            if (totalCount == 0)
                yield break;

            foreach (IConnectionPoint conPoint in _GetAllConnectionPoints(settings))
            {
                yield return new QueryTask {
                    ConnectionPoint = conPoint,
                    Param = totalCount < 0 ? param : DbSearchParam.FromPrototype(param, totalCount - ctx.Builder.RowCount)
                };

                if (totalCount >= 0 && ctx.Builder.RowCount >= totalCount)
                    yield break;
            }
        }

        class QueryTask
        {
            public IConnectionPoint ConnectionPoint;
            public DbSearchParam Param;
        }

        class QueryTaskContext
        {
            public DbSearchParam Param;
            public DataListBuilder Builder;
        }

        private DataList _ToDataList(DataListBuilder builder, string[] columns)
        {
            if (builder.IsEmpty)
                CreateEmptyDataList(columns);

            return builder.ToDataList();
        }

        // 从指定路由的分表中选取
        private DataList _SelectByRouteKeys(object[] routeKeys, DbSearchParam param, string[] columns, UtInvokeSettings settings)
        {
            DataListBuilder builder = CreateDataListBuilder();
            IDictionary<IConnectionPoint, object[]> dict = SplitByRouteKey(routeKeys, settings);

            foreach (var item in dict)
            {
                IConnectionPoint conPoint = item.Key;
                object[] data = item.Value;

                DataList dataList = conPoint.CreateTableConnection(UtContext).Select(data, columns, param, settings);
                builder.AddDataList(dataList);
            }

            return _ToDataList(builder, columns);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="autoUpdate">当数据已经存在时，是否将其更新</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public override int Insert(DataList data, bool autoUpdate, UtInvokeSettings settings)
        {
            Contract.Requires(data != null);

            AssignPrimaryKeys(data);
            IDictionary<IConnectionPoint, DataList> dict = SplitByRouteKey(data, settings);

            int count = 0;
            foreach (var item in dict)
            {
                IConnectionPoint conPoint = item.Key;
                DataList d = item.Value;

                count += conPoint.CreateTableConnection(UtContext).Insert(d, autoUpdate, settings);
            }

            return count;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">需要更新数据</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">查询条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public override int Update(DataList data, object[] routeKeys, string where, UtInvokeSettings settings)
        {
            Contract.Requires(data != null);

            if (data.Rows.Length == 0)
                return 0;

            int rtIndex = data.GetColumnIndex(RouteKey);
            if (routeKeys == null)
            {
                // 只有一行，并且未指定路由键
                if (data.Rows.Length == 1 && (rtIndex < 0 || data.GetValue(0, rtIndex) == null))
                {
                    return _UpdateWithoutRouteKeys(data, where, settings);
                }

                // 有一行或多行，并且都指定了路由键
                if (rtIndex < 0 || data.Rows.Any(r => r.Cells[rtIndex] == null))
                    throw new DbException("多行更新时，必须全部指定路由键");

                return _UpdateWithRouteKeys(data, where, settings);
            }
            else
            {
                if (data.Rows.Length != 1)
                    throw new DbException("通过routeKeys参数指定路由键的更新，数据集中只允许包含一行");

                if (rtIndex >= 0)
                    throw new DbException("通过routeKeys参数指定路由键后，数据集中不允许再包含路由键");

                return _UpdateWithoutRouteKeys(data, routeKeys, where, settings);
            }
        }

        // 按条件在所有分表中更新
        private int _UpdateWithoutRouteKeys(DataList data, string where, UtInvokeSettings settings)
        {
            int count = 0;
            foreach (IConnectionPoint conPoint in _GetAllConnectionPoints(settings))
            {
                count += conPoint.CreateTableConnection(UtContext).Update(data, null, where, settings);
            }

            return count;
        }

        // 按数据集中的路由键更新
        private int _UpdateWithRouteKeys(DataList data, string where, UtInvokeSettings settings)
        {
            int count = 0;
            foreach (KeyValuePair<IConnectionPoint, DataList> item in SplitByRouteKey(data, settings))
            {
                IConnectionPoint conPoint = item.Key;
                DataList d = item.Value;

                count += conPoint.CreateTableConnection(UtContext).Update(d, null, where, settings);
            }

            return count;
        }

        // 按明确指定的路由键更新
        private int _UpdateWithoutRouteKeys(DataList data, object[] routeKeys, string where, UtInvokeSettings settings)
        {
            IDictionary<IConnectionPoint, object[]> dict = SplitByRouteKey(routeKeys, settings);

            int count = 0;
            foreach (var item in dict)
            {
                IConnectionPoint conPoint = item.Key;
                object[] d = item.Value;

                count += conPoint.CreateTableConnection(UtContext).Update(data, d, where, settings);
            }

            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data">需要删除的数据</param>
        /// <param name="where">删除条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public override int Delete(DataList data, string where, UtInvokeSettings settings)
        {
            int rtIndex;
            if (data == null || ((rtIndex = data.GetColumnIndex(RouteKey)) < 0 && data.Rows.Length == 1))
                return _DeleteWithoutRouteKeys(data, where, settings);

            if (data.Rows.Any(r => r.Cells[rtIndex] == null))
                throw new DbException("按路由键删除数据时，必须在数据集中的所有行都指定路由键");

            if (data.Rows.Length == 0)
                return 0;

            return _DeleteWithRouteKeys(data, where, settings);
        }

        // 在所有分表中执行删除
        private int _DeleteWithoutRouteKeys(DataList data, string where, UtInvokeSettings settings)
        {
            int count = 0;
            foreach (IConnectionPoint conPoint in _GetAllConnectionPoints(settings))
            {
                count += conPoint.CreateTableConnection(UtContext).Delete(data, where, settings);
            }

            return count;
        }

        // 按路由键删除
        private int _DeleteWithRouteKeys(DataList data, string where, UtInvokeSettings settings)
        {
            int count = 0;
            foreach (KeyValuePair<IConnectionPoint, DataList> item in SplitByRouteKey(data, settings))
            {
                IConnectionPoint conPoint = item.Key;
                DataList d = item.Value;

                count += conPoint.CreateTableConnection(UtContext).Delete(d, where, settings);
            }

            return count;
        }

        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="routeKey">路由键</param>
        /// <param name="data">需要合并的数据</param>
        /// <param name="compareColumns">用于比较的字段</param>
        /// <param name="where">过滤条件</param>
        /// <param name="option">合并选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响行数</returns>
        public override int Merge(object routeKey, DataList data, string[] compareColumns, string where, UtConnectionMergeOption option, UtInvokeSettings settings)
        {
            if (option == UtConnectionMergeOption.None)
                return 0;

            if (routeKey == null)
                throw new DbException("合并数据时，路由键不可以为空");

            int rtIndex = data.GetColumnIndex(RouteKey), pkIndex = data.GetPrimaryKeyColumnIndex();
            if (rtIndex >= 0 && data.Rows.Any(r => r.Cells[rtIndex] != null && !object.Equals(r.Cells[rtIndex], routeKey)))
                throw new DbException("合并数据时，数据集中的路由键与参数中指定的路由键不一致");

            if (option.HasFlag(UtConnectionMergeOption.Insert))
            {
                if (pkIndex < 0)
                    throw new DbException("合并数据时，若指定插入选项，则数据集中必须包含主键列");

                AssignPrimaryKeys(data);
            }

            if (compareColumns.IsNullOrEmpty())
            {
                if (pkIndex < 0)
                    throw new DbException("未指定用于比较的字段，并且未指定主键");

                compareColumns = new[] { PrimaryKey };
            }

            // 用于比较的字段，皆不能为空值
            foreach (string colName in compareColumns)
            {
                int index = data.GetColumnIndex(colName);
                if (index < 0)
                    throw new DbException(string.Format("用作比较的字段“{0}”在数据集中不存在", colName));

                if (data.Rows.Any(r => r.Cells[index] == null))
                    throw new DbException(string.Format("用作比较的字段“{0}”不允许含有空值", colName));
            }

            IConnectionPoint conPoint = GetConnectionPoint(routeKey);
            return conPoint.CreateTableConnection(UtContext).Merge(routeKey, data, compareColumns, where, option, settings);
        }
    }
}
