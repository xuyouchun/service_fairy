using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Data.Entities
{
    partial class DbEntity<TEntity>
    {
        /// <summary>
        /// 选取指定数量的记录
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="columns">列</param>
        /// <param name="count">数量</param>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序表达式</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectTop(IUtConnectionProvider provider, object[] routeKeys = null,
            string[] columns = null, int count = 1, string where = null, string order = null, UtInvokeSettings settings = null)
        {
            return Select(provider, routeKeys, columns, DbSearchParam.Top(count, where, order), settings);
        }

        /// <summary>
        /// 选取指定数量的记录
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="columns">列</param>
        /// <param name="count">数量</param>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序表达式</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectTop<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys = null,
            string[] columns = null, int count = 1, string where = null, string order = null, UtInvokeSettings settings = null)
        {
            return SelectTop(provider, _ToObjectArray(routeKeys), columns, count, where, order, settings);
        }

        /// <summary>
        /// 选取指定数量的记录
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="columns">列</param>
        /// <param name="count">数量</param>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序表达式</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectTop(IUtConnectionProvider provider, object routeKey = null,
            string[] columns = null, int count = 1, string where = null, string order = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return Select(provider, _ToArray(routeKey), columns, DbSearchParam.Top(count, where, order), settings);
        }
    }
}
