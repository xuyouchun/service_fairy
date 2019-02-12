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
        /// 按组合并数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="entities">实体类</param>
        /// <param name="groups">组</param>
        /// <param name="compareColumns">用作比较的列</param>
        /// <param name="where">过滤条件</param>
        /// <param name="option">合并选项</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int MergeGroup(IUtConnectionProvider provider, object routeKey, IList<TEntity> entities, string[] groups,
            string[] compareColumns = null, string where = null, UtConnectionMergeOption option = UtConnectionMergeOption.InsertUpdate,
            UtInvokeSettings settings = null)
        {
            return Merge(provider, routeKey, entities, GetColumnsOfGroups(groups, DbEntityGroupOption.None), compareColumns, where, option, settings);
        }
    }
}
