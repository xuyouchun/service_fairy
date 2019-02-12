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
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">列值</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="column">需要选取数据的列</param>
        /// <param name="param">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumnIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            object[] routeKeys, string column, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(inColumn != null);

            if (inValues.IsNullOrEmpty())
                return Array<object>.Empty;

            string where = (string)_CreateInExpression(inColumn, inValues);
            return SelectOneColumn(provider, routeKeys, column, DbSearchParam.FromPrototype(param, where, true), settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">列值</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="column">需要选取数据的列</param>
        /// <param name="param">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumnIn<TRouteKey, TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            TRouteKey[] routeKeys, string column, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return SelectOneColumnIn(provider, inColumn, _ToObjectArray(inValues), _ToObjectArray(routeKeys), column, param, settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">列值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="column">需要选取数据的列</param>
        /// <param name="param">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumnIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            object routeKey, string column, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectOneColumnIn(provider, inColumn, inValues, _ToArray(routeKey), column, param, settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">列值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="column">需要选取数据的列</param>
        /// <param name="param">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumnIn<TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            object routeKey, string column, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return SelectOneColumnIn(provider, inColumn, _ToObjectArray(inValues), routeKey, column, param, settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">列值</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="column">需要选取数据的列</param>
        /// <param name="param">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumnIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            object[] routeKeys, string column, string where, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKeys != null);
            return SelectOneColumnIn(provider, inColumn, inValues, _ToArray(routeKeys), column, DbSearchParam.FromWhere(where), settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">列值</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="column">需要选取数据的列</param>
        /// <param name="param">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumnIn<TRouteKey, TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            TRouteKey[] routeKeys, string column, string where, UtInvokeSettings settings = null)
        {
            return SelectOneColumnIn(provider, inColumn, _ToObjectArray(inValues), _ToObjectArray(routeKeys), column, where, settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">列值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="column">需要选取数据的列</param>
        /// <param name="param">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumnIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            object routeKey, string column, string where, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectOneColumnIn(provider, inColumn, inValues, _ToArray(routeKey), column, where, settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据连接</param>
        /// <param name="inColumn">列名</param>
        /// <param name="inValues">列值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="column">需要选取数据的列</param>
        /// <param name="param">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object[] SelectOneColumnIn<TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            object routeKey, string column, string where, UtInvokeSettings settings = null)
        {
            return SelectOneColumnIn(provider, inColumn, _ToObjectArray(inValues), routeKey, column, where, settings);
        }
    }
}
