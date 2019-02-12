using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.Entities;

namespace ServiceFairy.DbEntities.User
{
    /// <summary>
    /// 用户关联表
    /// </summary>
    [DbEntity("UserRelation", G_Basic, InitTableCount = 4)]
    public class DbUserRelation : DbUserNameBase<DbUserRelation>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DbPrimaryKey(NullValue = Null_Int64)]
        public long ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DbColumn(G_Basic)]
        public int UserId { get; set; }

        /// <summary>
        /// 粉丝ID
        /// </summary>
        [DbColumn(G_Basic)]
        public int FollowerId { get; set; }

        /// <summary>
        /// 基础信息组
        /// </summary>
        public const string G_Basic = "Basic";

        /// <summary>
        /// 用户ID
        /// </summary>
        public const string F_UserId = G_Basic + ".UserId";

        /// <summary>
        /// 粉丝ID
        /// </summary>
        public const string F_FollowerId = G_Basic + ".FollowerId";
    }
}
