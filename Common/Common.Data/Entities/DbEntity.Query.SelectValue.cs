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
        /// 选取第一条记录
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="columns">列</param>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序表达式</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static object SelectValue(IUtConnectionProvider provider, string column, object[] routeKeys = null,
            string where = null, string order = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(provider != null && column != null);

            DataList data = _Select(provider, routeKeys, new[] { column }, DbSearchParam.TopOne(where, order), settings);
            if (data.Rows.Length == 0)
                return null;

            return data.GetValue(0, column);
        }

        /// <summary>
        /// 选取第一条记录
        /// </summary>
        /// <typeparam name="TRouteKey">路由键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="column">列</param>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序表达式</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TValue SelectValue<TRouteKey, TValue>(IUtConnectionProvider provider, string column, TRouteKey[] routeKeys = null,
            string where = null, string order = null, TValue defaultValue = default(TValue), UtInvokeSettings settings = null)
        {
            object value = SelectValue(provider, column, _ToObjectArray(routeKeys), where, order, settings);
            return value.ToType<TValue>(defaultValue);
        }

        /// <summary>
        /// 选取第一条记录
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="column">列</param>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序表达式</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static TValue SelectValue<TValue>(IUtConnectionProvider provider, string column, object routeKey,
            string where = null, string order = null, TValue defaultValue = default(TValue), UtInvokeSettings settings = null)
        {
            return SelectValue<object, TValue>(provider, column, _ToArray(routeKey), where, order, defaultValue, settings);
        }
    }
}
