using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Collection;

namespace Common.Data.Entities
{
    /// <summary>
    /// 数据库实体的操作
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    partial class DbEntity<TEntity>
    {
        private static IUtConnection _CreateConnection(IUtConnectionProvider provider)
        {
            Contract.Requires(provider != null);

            IUtConnection utCon = provider.CreateUtConnection(FullTableName);
            if (utCon == null)
                throw new DbEntityException(string.Format("无法创建表“{0}”的连接", FullTableName));

            return utCon;
        }

        // 是否包含不为空的主键
        private static bool _AnyPrimaryKey(IEnumerable<TEntity> entities)
        {
            return entities.Any(entity => entity.GetPrimaryKey() != null);
        }

        // 是否全部的主键都不为空
        private static bool _AllPrimaryKey(IEnumerable<TEntity> entities)
        {
            return entities.All(entity => entity.GetPrimaryKey() != null);
        }

        // 是否包含不为空的路由键
        private static bool _AnyRouteKey(IEnumerable<TEntity> entities)
        {
            return entities.Any(entity => entity.GetRouteKey() != null);
        }

        // 是否全部的路由键都不为空
        private static bool _AllRouteKey(IEnumerable<TEntity> entities)
        {
            return entities.All(entity => entity.GetRouteKey() != null);
        }

        // 如果路由键或主键不为空，则将其附加进去
        private static string[] _UnionSpecialKeys(IEnumerable<string> columns, IEnumerable<TEntity> entities = null)
        {
            IgnoreCaseHashSet hs = new IgnoreCaseHashSet(columns);
            if (!hs.Contains(PrimaryKeyColumnInfo.Name) && (entities == null || _AnyPrimaryKey(entities)))
                hs.Add(PrimaryKeyColumnInfo.Name);

            if (!hs.Contains(RouteKeyColumnInfo.Name) && (entities == null || _AnyRouteKey(entities)))
                hs.Add(RouteKeyColumnInfo.Name);

            return hs.ToArray();
        }
    }

    /// <summary>
    /// 组查询选项
    /// </summary>
    public enum DbEntityGroupOption
    {
        None,

        /// <summary>
        /// 带有主键
        /// </summary>
        WithPrimaryKey = 0x01,

        /// <summary>
        /// 带有路由键
        /// </summary>
        WithRouteKey = 0x02,

        /// <summary>
        /// 带有主键与路由键
        /// </summary>
        WithPrimaryAndRoute = WithPrimaryKey | WithRouteKey,
    }
}
