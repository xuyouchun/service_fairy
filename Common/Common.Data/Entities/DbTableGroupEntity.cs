using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Data.Entities
{
    /// <summary>
    /// 带有表组信息的表实体
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DbTableGroupEntity<TEntity> : DbEntity<TEntity>
        where TEntity : DbTableGroupEntity<TEntity>, new()
    {
        static DbTableGroupEntity()
        {
            DbTableGroupEntityAttribute attr = typeof(TEntity).GetAttribute<DbTableGroupEntityAttribute>(true);
            if (attr != null)
            {
                TableGroupName = attr.TableGroupName;
            }
        }

        /// <summary>
        /// 表组
        /// </summary>
        public static string TableGroupName { get; private set; }
    }
}
