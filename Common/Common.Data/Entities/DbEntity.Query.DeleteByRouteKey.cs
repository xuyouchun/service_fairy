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
        /// 根据路由键删除
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int DeleteByRouteKey(IUtConnectionProvider provider, object[] routeKeys, string where = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKeys != null);

            if (routeKeys.Length == 0)
                return 0;

            DataList data = _ToDataList(RouteKeyColumnInfo.Name, routeKeys);
            return _CreateConnection(provider).Delete(data, where, settings);
        }

        /// <summary>
        /// 根据路由键删除
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="where">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int DeleteByRouteKey(IUtConnectionProvider provider, object routeKey, string where = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);

            return DeleteByRouteKey(provider, _ToArray(routeKey), where, settings);
        }

        /// <summary>
        /// 根据路由键删除
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int DeleteByRouteKey<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys, string where = null, UtInvokeSettings settings = null)
        {
            return DeleteByRouteKey(provider, _ToObjectArray(routeKeys), where, settings);
        }
    }
}
