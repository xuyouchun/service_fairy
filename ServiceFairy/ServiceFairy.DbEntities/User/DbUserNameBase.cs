using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.Entities;

namespace ServiceFairy.DbEntities.User
{
    [DbTableGroupEntity("UserName")]
    public abstract class DbUserNameBase<TEntity> : DbTableGroupEntity<TEntity> where TEntity : DbUserNameBase<TEntity>, new()
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DbRouteKey(Size = 128)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public const string F_UserName = "UserName";
    }
}
