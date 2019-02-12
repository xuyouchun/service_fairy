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
        /// 根据路由键更新
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="entity">实体类</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">过滤条件</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int UpdateByRouteKey(IUtConnectionProvider provider, TEntity entity, object[] routeKeys, string[] columns, string where = null, UtInvokeSettings settings = null)
        {
            return UpdateIn(provider, entity, RouteKeyColumnInfo.Name, routeKeys, columns, where, settings);
        }

        /// <summary>
        /// 根据路由键更新
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="entity">实体类</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">过滤条件</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int UpdateByRouteKey<TRouteKey>(IUtConnectionProvider provider, TEntity entity, TRouteKey[] routeKeys, string[] columns, string where = null, UtInvokeSettings settings = null)
        {
            return UpdateByRouteKey(provider, entity, _ToObjectArray(routeKeys), columns, where, settings);
        }
    }
}
