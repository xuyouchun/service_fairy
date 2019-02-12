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
        /// 按指定的字段值删除数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="where">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int DeleteIn(IUtConnectionProvider provider, string inColumn, object[] inValues, object routeKey, string where = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);

            if (inValues.IsNullOrEmpty())
                return 0;

            DataList data = _ToDataListWithRouteKey(inColumn, inValues, routeKey);
            return _CreateConnection(provider).Delete(data, where, settings);
        }

        /// <summary>
        /// 按指定的字段值删除数据
        /// </summary>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="where">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int DeleteIn<TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues, object routeKey, string where = null, UtInvokeSettings settings = null)
        {
            return DeleteIn(provider, inColumn, _ToObjectArray(inValues), routeKey, where, settings);
        }

        /// <summary>
        /// 按指定的字段值删除数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValue">值</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="where">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int DeleteIn(IUtConnectionProvider provider, string inColumn, object inValue, object routeKey, string where = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(inValue != null);
            return DeleteIn(provider, inColumn, new[] { inValue }, routeKey, where, settings);
        }
    }
}
