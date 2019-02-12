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
        /// 删除数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="entities">实体类</param>
        /// <param name="where">过滤条件</param>
        /// <param name="filterColumns">用作过滤条件的列（默认为主键与路由键）</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int Delete(IUtConnectionProvider provider, IList<TEntity> entities, string where = null, string[] filterColumns = null, UtInvokeSettings settings = null)
        {
            Contract.Requires(provider != null);

            if (filterColumns == null)
                filterColumns = _GetSpecialColumns();

            return _CreateConnection(provider).Delete(entities == null ? null : ToDataList(entities, filterColumns), where, settings);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="entity">实体类</param>
        /// <param name="where">过滤条件</param>
        /// <param name="filterColumns">用作过滤条件的列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public int Delete(IUtConnectionProvider provider, string where = null, string[] filterColumns = null, UtInvokeSettings settings = null)
        {
            return Delete(provider, new[] { (TEntity)this }, where, filterColumns, settings);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="values">实体类</param>
        /// <param name="where">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int Delete(IUtConnectionProvider provider, IDictionary<string, object> values, string where = null, UtInvokeSettings settings = null)
        {
            TEntity entity = new TEntity();
            entity.SetValues(values);

            return entity.Delete(provider, where, values.Keys.ToArray(), settings);
        }
    }
}
