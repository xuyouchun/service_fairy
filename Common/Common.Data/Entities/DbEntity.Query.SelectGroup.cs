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
        /// 按组选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="groups">列组</param>
        /// <param name="option">查询选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectGroup(IUtConnectionProvider provider, object[] routeKeys, string[] groups,
            DbSearchParam param = null, DbEntityGroupOption option = DbEntityGroupOption.WithPrimaryAndRoute, UtInvokeSettings settings = null)
        {
            return Select(provider, routeKeys, GetColumnsOfGroups(groups, option), param, settings);
        }

        /// <summary>
        /// 按组选取数据
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="groups">列组</param>
        /// <param name="option">查询选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectGroup<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys, string[] groups,
            DbSearchParam param = null, DbEntityGroupOption option = DbEntityGroupOption.WithPrimaryAndRoute, UtInvokeSettings settings = null)
        {
            return SelectGroup(provider, _ToObjectArray(routeKeys), groups, param, option, settings);
        }

        /// <summary>
        /// 按组选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="groups">列组</param>
        /// <param name="option">查询选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectGroup(IUtConnectionProvider provider, object routeKey, string[] groups, DbSearchParam param = null,
            DbEntityGroupOption option = DbEntityGroupOption.WithPrimaryAndRoute, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectGroup(provider, _ToArray(routeKey), groups, param, option, settings);
        }

        /// <summary>
        /// 按组选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">查询参数</param>
        /// <param name="groups">列组</param>
        /// <param name="option">查询选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectGroup(IUtConnectionProvider provider, object[] routeKeys, string[] groups,
            string where, DbEntityGroupOption option = DbEntityGroupOption.WithPrimaryAndRoute, UtInvokeSettings settings = null)
        {
            return SelectGroup(provider, routeKeys, groups, DbSearchParam.FromWhere(where), option, settings);
        }

        /// <summary>
        /// 按组选取数据
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">查询参数</param>
        /// <param name="groups">列组</param>
        /// <param name="option">查询选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectGroup<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys, string[] groups,
            string where, DbEntityGroupOption option = DbEntityGroupOption.WithPrimaryAndRoute, UtInvokeSettings settings = null)
        {
            return SelectGroup(provider, _ToObjectArray(routeKeys), groups, where, option, settings);
        }

        /// <summary>
        /// 按组选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="where">查询参数</param>
        /// <param name="groups">列组</param>
        /// <param name="option">查询选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectGroup(IUtConnectionProvider provider, object routeKey, string[] groups,
            string where, DbEntityGroupOption option = DbEntityGroupOption.WithPrimaryAndRoute, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectGroup(provider, _ToArray(routeKey), groups, where, option, settings);
        }
    }
}
