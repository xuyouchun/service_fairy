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
    /// <typeparam name="TEntity">实体类型</typeparam>
    partial class DbEntity<TEntity>
    {
        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="keyColumn">键列名</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="valueColumn">值列名</param>
        /// <param name="param">参数</param>
        /// <param name="keys">键</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static KeyValuePair<object, object>[] SelectKeyValues(IUtConnectionProvider provider, string keyColumn, object[] keys,
            object[] routeKeys, string valueColumn, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(keyColumn != null);

            if (keys.IsNullOrEmpty())
                return Array<KeyValuePair<object, object>>.Empty;

            DataList dataList;
            if (IsRouteKey(keyColumn) && (routeKeys == null || object.ReferenceEquals(routeKeys, keys)))
            {
                routeKeys = keys;
                dataList = _CreateConnection(provider).Select(routeKeys, new[] { keyColumn, valueColumn }, param, settings);
            }
            else
            {
                string where = (string)_CreateInExpression(keyColumn, keys);
                dataList = _CreateConnection(provider).Select(routeKeys, new[] { keyColumn, valueColumn }, DbSearchParam.FromPrototype(param, where), settings);
            }

            int keyIndex = dataList.GetColumnIndex(keyColumn, true), valueIndex = dataList.GetColumnIndex(valueColumn, true);
            return dataList.Rows.ToArray(r => new KeyValuePair<object, object>(r.Cells[keyIndex], r.Cells[valueIndex]));
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="keyColumn">键列名</param>
        /// <param name="inValues">键列值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="valueColumn">值列名</param>
        /// <param name="param">参数</param>
        /// <param name="keys">键</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static KeyValuePair<object, object>[] SelectKeyValues(IUtConnectionProvider provider, string keyColumn, object[] keys,
            object routeKey, string valueColumn, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectKeyValues(provider, keyColumn, keys, _ToArray(routeKey), valueColumn, param, settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="keyColumn">键列名</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="valueColumn">值列名</param>
        /// <param name="param">参数</param>
        /// <param name="keys">键</param>
        /// <param name="where">查询条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static KeyValuePair<object, object>[] SelectKeyValues(IUtConnectionProvider provider, string keyColumn, object[] keys,
            object[] routeKeys, string valueColumn, string where, UtInvokeSettings settings = null)
        {
            return SelectKeyValues(provider, keyColumn, keys, routeKeys, valueColumn, DbSearchParam.FromWhere(where), settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="keyColumn">键列名</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="valueColumn">值列名</param>
        /// <param name="param">参数</param>
        /// <param name="keys">键</param>
        /// <param name="where">查询条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static KeyValuePair<object, object>[] SelectKeyValues(IUtConnectionProvider provider, string keyColumn, object[] keys,
            object routeKey, string valueColumn, string where, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectKeyValues(provider, keyColumn, keys, routeKey, valueColumn, where, settings);
        }


        // ---------------------------------------------------------------------

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="keyColumn">键列名</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="valueColumn">值列名</param>
        /// <param name="param">参数</param>
        /// <param name="keys">键</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TValue>[] SelectKeyValues<TKey, TValue>(IUtConnectionProvider provider, string keyColumn, TKey[] keys,
            object[] routeKeys, string valueColumn, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            return SelectKeyValues(provider, keyColumn, keys.CastAsObject(), routeKeys, valueColumn, param, settings)
                .ToArray(v => new KeyValuePair<TKey, TValue>(v.Key.ToTypeWithError<TKey>(), _ConvertTo<TValue>(v.Value)));
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="keyColumn">键列名</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="valueColumn">值列名</param>
        /// <param name="param">参数</param>
        /// <param name="keys">键</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TValue>[] SelectKeyValues<TKey, TValue>(IUtConnectionProvider provider, string keyColumn, TKey[] keys,
            object routeKey, string valueColumn, DbSearchParam param = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectKeyValues<TKey, TValue>(provider, keyColumn, keys, _ToArray(routeKey), valueColumn, param, settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="keyColumn">键列名</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="valueColumn">值列名</param>
        /// <param name="param">参数</param>
        /// <param name="keys">键</param>
        /// <param name="where">查询条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TValue>[] SelectKeyValues<TKey, TValue>(IUtConnectionProvider provider, string keyColumn, TKey[] keys,
            object[] routeKeys, string valueColumn, string where, UtInvokeSettings settings = null)
        {
            return SelectKeyValues<TKey, TValue>(provider, keyColumn, keys, routeKeys, valueColumn, DbSearchParam.FromWhere(where), settings);
        }

        /// <summary>
        /// 选择单列指定列值的数据
        /// </summary>
        /// <param name="provider">数据连接</param>
        /// <param name="keyColumn">键列名</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="valueColumn">值列名</param>
        /// <param name="param">参数</param>
        /// <param name="keys">键</param>
        /// <param name="where">查询条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TValue>[] SelectKeyValues<TKey, TValue>(IUtConnectionProvider provider, string keyColumn, TKey[] keys,
            object routeKey, string valueColumn, string where, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return SelectKeyValues<TKey, TValue>(provider, keyColumn, keys, _ToArray(routeKey), valueColumn, where, settings);
        }
    }
}
