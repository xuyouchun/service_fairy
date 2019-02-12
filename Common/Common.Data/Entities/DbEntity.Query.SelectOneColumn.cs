using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Data.Entities
{
    /// <summary>
    /// 数据库实体的操作
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    partial class DbEntity<TEntity>
    {
        /// <summary>
        /// 选取单列
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="column">列</param>
        /// <param name="param">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumn(IUtConnectionProvider provider, object[] routeKeys,
            string column, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            DataList dataList = _CreateConnection(provider).Select(routeKeys, new[] { column }, param, settings);
            return dataList.GetValues(column);
        }

        /// <summary>
        /// 选取单列
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="column">列</param>
        /// <param name="param">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumn<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys,
            string column, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return SelectOneColumn(provider, _ToObjectArray(routeKeys), column, param, settings);
        }

        /// <summary>
        /// 选取单列
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="column">列</param>
        /// <param name="param">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumn(IUtConnectionProvider provider, object routeKey,
            string column, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectOneColumn(provider, _ToArray(routeKey), column, param, settings);
        }

        /// <summary>
        /// 选取单列
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="column">列</param>
        /// <param name="where">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumn(IUtConnectionProvider provider, object[] routeKeys,
            string column, string where, UtInvokeSettings settings = null)
        {
            return SelectOneColumn(provider, routeKeys, column, DbSearchParam.FromWhere(where), settings);
        }

        /// <summary>
        /// 选取单列
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="column">列</param>
        /// <param name="where">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumn<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys,
            string column, string where, UtInvokeSettings settings = null)
        {
            return SelectOneColumn(provider, _ToObjectArray(routeKeys), column, where, settings);
        }

        /// <summary>
        /// 选取单列
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="column">列</param>
        /// <param name="where">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumn(IUtConnectionProvider provider, object routeKey,
            string column, string where, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectOneColumn(provider, _ToArray(routeKey), column, where, settings);
        }
    }
}
