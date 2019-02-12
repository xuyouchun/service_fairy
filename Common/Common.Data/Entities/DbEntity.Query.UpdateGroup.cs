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
        /// 按组更新数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="entities">实体类</param>
        /// <param name="groups">列组</param>
        /// <param name="option">选项</param>
        /// <param name="where">过滤条件</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int UpdateGroup(IUtConnectionProvider provider, IList<TEntity> entities, string[] groups,
            string where = null, DbEntityGroupOption option = DbEntityGroupOption.WithPrimaryAndRoute, UtInvokeSettings settings = null)
        {
            return Update(provider, entities, GetColumnsOfGroups(groups ?? Array<string>.Empty, option), where, settings);
        }

        /// <summary>
        /// 按组更新数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="groups">列组</param>
        /// <param name="where">过滤条件</param>
        /// <param name="option">选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public int UpdateGroup(IUtConnectionProvider provider, string[] groups, string where = null,
            DbEntityGroupOption option = DbEntityGroupOption.WithPrimaryAndRoute, UtInvokeSettings settings = null)
        {
            return UpdateGroup(provider, new[] { (TEntity)this }, groups, where, option, settings);
        }
    }
}
