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
        /// 更新数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="entities">实体类</param>
        /// <param name="where">过滤条件</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int Update(IUtConnectionProvider provider, IList<TEntity> entities, string[] columns,
            string where = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(provider != null && entities != null);

            columns = _UnionSpecialKeys(columns ?? Array<string>.Empty, entities);
            return _CreateConnection(provider).Update(ToDataList(entities, columns), null, where, settings);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="entities">实体类</param>
        /// <param name="where">过滤条件</param>
        /// <param name="column">需要更新的列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int Update(IUtConnectionProvider provider, IList<TEntity> entities, string column,
            string where = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(column != null);

            return Update(provider, entities, new[] { column }, where, settings);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="entity">实体类</param>
        /// <param name="where">过滤条件</param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public int Update(IUtConnectionProvider provider, string[] columns, string where = null,
            UtInvokeSettings settings = null)
        {
            return Update(provider, new[] { (TEntity)this }, columns, where, settings);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="column">需要更新的列</param>
        /// <param name="where">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns>受影响的行数</returns>
        public int Update(IUtConnectionProvider provider, string column, string where = null,
            UtInvokeSettings settings = null)
        {
            Contract.Requires(column != null);

            return Update(provider, new[] { column }, where, settings);
        }
    }
}
