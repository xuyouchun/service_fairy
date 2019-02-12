using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.Entities;
using ServiceFairy.DbEntities.User;

namespace ServiceFairy.DbEntities.Group
{
    /// <summary>
    /// 群组实体基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [DbTableGroupEntity("Group")]
    public abstract class DbGroupBase<TEntity> : DbTableGroupEntity<TEntity> where TEntity : DbGroupBase<TEntity>, new()
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DbRouteKey(NullValue = Null_Int32)]
        public int GroupId { get; set; }

        /// <summary>
        /// 群组ID
        /// </summary>
        public const string F_GroupId = "GroupId";
    }
}
