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
        /// 删除所有数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int DeleteAll(IUtConnectionProvider provider, UtInvokeSettings settings = null)
        {
            return Delete(provider, (IList<TEntity>)null, null, null, settings);
        }
    }
}