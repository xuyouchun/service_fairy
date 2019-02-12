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
    /// <summary>
    /// 数据库实体的操作
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    partial class DbEntity<TEntity>
    {
        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">字段值</param>
        /// <param name="entity">实体</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int UpdateIn(IUtConnectionProvider provider, TEntity entity, string inColumn, object[] inValues,
            string[] columns, string where = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(inColumn != null);

            if (inValues.IsNullOrEmpty())
                return 0;

            if (IsRouteKey(inColumn))  // 如果字段为路由键
            {
                DataList data = ToDataList(new[] { entity }, columns);
                return _CreateConnection(provider).Update(data, inValues, where, settings);
            }
            else
            {
                SqlExpression exp = _CreateInExpression(inColumn, inValues);
                if (!string.IsNullOrEmpty(where))
                    exp = SqlExpression.And(exp, SqlExpression.Parse(where));

                return Update(provider, new[] { entity }, columns, exp.ToString(), settings);
            }
        }

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">字段值</param>
        /// <param name="entity">实体</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int UpdateIn<TInValue>(IUtConnectionProvider provider, TEntity entity, string inColumn, TInValue[] inValues,
            string[] columns, string where = null, UtInvokeSettings settings = null)
        {
            return UpdateIn(provider, entity, inColumn, _ToObjectArray(inValues), columns, where, settings);
        }

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">字段值</param>
        /// <param name="entity">实体</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int UpdateIn(IUtConnectionProvider provider, TEntity entity, string inColumn, object[] inValues,
            string column, string where = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(column != null);

            return UpdateIn(provider, entity, inColumn, inValues, new[] { column }, where, settings);
        }

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">字段值</param>
        /// <param name="entity">实体</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int UpdateIn<TInValue>(IUtConnectionProvider provider, TEntity entity, string inColumn, TInValue[] inValues,
            string column, string where = null, UtInvokeSettings settings = null)
        {
            return UpdateIn(provider, entity, inColumn, _ToObjectArray(inValues), column, where, settings);
        }


        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">字段值</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public int UpdateIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            string[] columns, string where = null, UtInvokeSettings settings = null)
        {
            return UpdateIn(provider, (TEntity)this, inColumn, inValues, columns, where, settings);
        }

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">字段值</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public int UpdateIn<TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            string[] columns, string where = null, UtInvokeSettings settings = null)
        {
            return this.UpdateIn(provider, inColumn, _ToObjectArray(inValues), columns, where, settings);
        }

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">字段值</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public int UpdateIn(IUtConnectionProvider provider, string inColumn, object[] inValues,
            string column, string where = null, UtInvokeSettings settings = null)
        {
            return UpdateIn(provider, (TEntity)this, inColumn, inValues, column, where, settings);
        }

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <typeparam name="TInValue">值类型</typeparam>
        /// <param name="provider">数据库连接</param>
        /// <param name="inColumn">字段名称</param>
        /// <param name="inValues">字段值</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="where">更新条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public int UpdateIn<TInValue>(IUtConnectionProvider provider, string inColumn, TInValue[] inValues,
            string column, string where = null, UtInvokeSettings settings = null)
        {
            return this.UpdateIn(provider, inColumn, _ToObjectArray(inValues), column, where, settings);
        }
    }
}
