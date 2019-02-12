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
        /// 插入数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="entities">实体类</param>
        /// <param name="autoUpdate">当数据已经存在时，是否自动更新</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static int Insert(IUtConnectionProvider provider, IList<TEntity> entities, bool autoUpdate = false, UtInvokeSettings settings = null)
        {
            Contract.Requires(provider != null);

            DataList data = ToDataList(entities);
            int effectCount = _CreateConnection(provider).Insert(data, autoUpdate, settings);

            int pkIndex = data.GetPrimaryKeyColumnIndex();
            if (pkIndex >= 0)
            {
                for (int k = 0, count = entities.Count; k < count; k++)
                {
                    if (entities[k].GetPrimaryKey() == null)
                        entities[k].SetPrimaryKey(data.Rows[k].Cells[pkIndex]);
                }
            }

            return effectCount;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="provider">数据库连接</param>
        /// <param name="autoUpdate">当数据已经存在时，是否自动更新</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public int Insert(IUtConnectionProvider provider, bool autoUpdate = false, UtInvokeSettings settings = null)
        {
            return Insert(provider, new[] { (TEntity)this }, autoUpdate, settings);
        }
    }
}
