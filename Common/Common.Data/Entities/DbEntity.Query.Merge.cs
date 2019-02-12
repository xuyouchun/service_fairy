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
        /// 合并数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="entities">实体类</param>
        /// <param name="compareColumns">用作比较的字段（默认为主键）</param>
        /// <param name="mergeColumns">用作合并的字段</param>
        /// <param name="where">过滤条件</param>
        /// <param name="option">合并选项</param>
        /// <returns></returns>
        public static int Merge(IUtConnectionProvider provider, object routeKey, IList<TEntity> entities, string[] mergeColumns,
            string[] compareColumns = null, string where = null, UtConnectionMergeOption option = UtConnectionMergeOption.InsertUpdate, UtInvokeSettings settings = null)
        {
            Contract.Requires(routeKey != null);
            return _CreateConnection(provider).Merge(routeKey, ToDataList(entities, mergeColumns), compareColumns, where, option, settings);
        }
    }
}
