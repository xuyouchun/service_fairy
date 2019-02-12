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
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectAllColumns(IUtConnectionProvider provider, object[] routeKeys = null,
            DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return Select(provider, routeKeys, null, param, settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectAllColumns<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys,
            DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return Select<TRouteKey>(provider, routeKeys, null, param, settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectAllColumns(IUtConnectionProvider provider, object routeKey,
            DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return Select(provider, routeKey, null, param, settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectAllColumns(IUtConnectionProvider provider, object[] routeKeys,
            string where, UtInvokeSettings settings = null)
        {
            return Select(provider, routeKeys, null, where, settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectAllColumns<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys,
            string where, UtInvokeSettings settings = null)
        {
            return Select<TRouteKey>(provider, routeKeys, null, where, settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="where">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectAllColumns(IUtConnectionProvider provider, object routeKey,
            string where, UtInvokeSettings settings = null)
        {
            return Select(provider, routeKey, null, where, settings);
        }
    }
}
