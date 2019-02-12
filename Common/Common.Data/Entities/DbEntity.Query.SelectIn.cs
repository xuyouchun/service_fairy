using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Data.SqlExpressions;

namespace Common.Data.Entities
{
    partial class DbEntity<TEntity>
    {
        /// <summary>
        /// 选取指定列值的数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="columns">要查询的列</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">值</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            object[] routeKeys = null, string[] columns = null, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(inColumn != null);

            if (inValues.IsNullOrEmpty())
                return Array<TEntity>.Empty;

            if (IsRouteKey(inColumn) && (routeKeys == null || object.ReferenceEquals(routeKeys, inValues)))
            {
                routeKeys = inValues;
                return Select(provider, routeKeys, columns, param, settings);
            }
            else
            {
                string where = (string)_CreateInExpression(inColumn, inValues);
                return Select(provider, routeKeys, columns, DbSearchParam.FromPrototype(param, where), settings);
            }
        }

        /// <summary>
        /// 选取指定列值的数据
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="columns">要查询的列</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">值</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectIn<TRouteKey, TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            TRouteKey[] routeKeys = null, string[] columns = null, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return SelectIn(provider, inColumn, _ToObjectArray(inValues), _ToObjectArray(routeKeys), columns, param, settings);
        }

        private static SqlExpression _CreateInExpression(string inColumn, object[] inValues)
        {
            ColumnInfo cInfo = GetColumnInfo(inColumn);
            return (inValues.Length == 1 ? SqlExpression.Equals(inColumn, inValues[0], cInfo.Type)
                : SqlExpression.In(inColumn, inValues, cInfo.Type));
        }

        /// <summary>
        /// 选取指定列值的数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="columns">要查询的列</param>
        /// <param name="param">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            object routeKey = null, string[] columns = null, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectIn(provider, inColumn, inValues, _ToArray(routeKey), columns, param, settings);
        }

        /// <summary>
        /// 选取指定列值的数据
        /// </summary>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="columns">要查询的列</param>
        /// <param name="param">查询参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectIn<TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            object routeKey = null, string[] columns = null, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return SelectIn(provider, inColumn, _ToObjectArray(inValues), routeKey, columns, param, settings);
        }

        /// <summary>
        /// 选取指定列值的数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">值</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="columns">要查询的列</param>
        /// <param name="param">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            object[] routeKeys, string[] columns, string where, UtInvokeSettings settings = null)
        {
            return SelectIn(provider, inColumn, inValues, routeKeys, columns, DbSearchParam.FromWhere(where), settings);
        }

        /// <summary>
        /// 选取指定列值的数据
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">值</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="columns">要查询的列</param>
        /// <param name="param">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectIn<TRouteKey, TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            TRouteKey[] routeKeys, string[] columns, string where, UtInvokeSettings settings = null)
        {
            return SelectIn(provider, inColumn, _ToObjectArray(inValues), _ToObjectArray(routeKeys), columns, where, settings);
        }

        /// <summary>
        /// 选取指定列值的数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="columns">要查询的列</param>
        /// <param name="param">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            object routeKey, string[] columns, string where, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectIn(provider, inColumn, inValues, _ToArray(routeKey), columns, where, settings);
        }

        /// <summary>
        /// 选取指定列值的数据
        /// </summary>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="columns">要查询的列</param>
        /// <param name="param">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TEntity[] SelectIn<TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            object routeKey, string[] columns, string where, UtInvokeSettings settings = null)
        {
            return SelectIn(provider, inColumn, _ToObjectArray(inValues), routeKey, columns, where, settings);
        }
    }
}
