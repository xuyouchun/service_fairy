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
        private static DataList _Select(IUtConnectionProvider provider, object[] routeKeys = null,
            string[] columns = null, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return _CreateConnection(provider).Select(routeKeys, columns, param, settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="columns">要选取的列，如果为空引用，则选取全部的列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] Select(IUtConnectionProvider provider, object[] routeKeys = null,
            string[] columns = null, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(provider != null);

            DataList dataList = _Select(provider, routeKeys, columns, param, settings);
            return FromDataList(dataList);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="columns">要选取的列，如果为空引用，则选取全部的列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] Select<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys,
            string[] columns = null, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return Select(provider, _ToObjectArray(routeKeys), columns, param, settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="columns">要选取的列，如果为空引用，则选取全部的列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] Select(IUtConnectionProvider provider, object routeKey,
            string[] columns = null, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return Select(provider, _ToArray(routeKey), columns, param, settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="columns">要选取的列，如果为空引用，则选取全部的列</param>
        /// <param name="where">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] Select(IUtConnectionProvider provider, object[] routeKeys, string[] columns,
            string where, UtInvokeSettings settings = null)
        {
            return Select(provider, routeKeys, columns, DbSearchParam.FromWhere(where), settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="columns">要选取的列，如果为空引用，则选取全部的列</param>
        /// <param name="where">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] Select<TRouteKey>(IUtConnectionProvider provider, TRouteKey[] routeKeys, string[] columns,
            string where, UtInvokeSettings settings = null)
        {
            return Select(provider, _ToObjectArray(routeKeys), columns, where, settings);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="columns">要选取的列，如果为空引用，则选取全部的列</param>
        /// <param name="where">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] Select(IUtConnectionProvider provider, object routeKey, string[] columns,
            string where, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return Select(provider, _ToArray(routeKey), columns, where, settings);
        }
    }
}
