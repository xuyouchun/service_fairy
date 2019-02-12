using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable;
using Common.Data.Entities;

namespace ServiceFairy.DbEntities.User
{
    /// <summary>
    /// 用户实体
    /// </summary>
    [DbTableGroupEntity("User")]
    public abstract class DbUserBase<TEntity> : DbTableGroupEntity<TEntity> where TEntity : DbUserBase<TEntity>, new()
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DbRouteKey(NullValue = Null_Int32)]
        public int UserId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public const string F_UserId = "UserId";
    }
}
