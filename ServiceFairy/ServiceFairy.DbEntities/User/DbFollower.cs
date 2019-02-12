using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.Entities;

namespace ServiceFairy.DbEntities.User
{
    /// <summary>
    /// 粉丝表
    /// </summary>
    [DbEntity("Follower", G_Basic, InitTableCount = 4)]
    public class DbFollower : DbUserBase<DbFollower>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DbPrimaryKey(NullValue = Null_Int64)]
        public long ID { get; set; }

        /// <summary>
        /// 粉丝UID
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_Int32)]
        public int FollowerId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_DateTime)]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 基础信息组
        /// </summary>
        public const string G_Basic = "Basic";

        /// <summary>
        /// 主键
        /// </summary>
        public const string F_ID = "ID";

        /// <summary>
        /// 粉丝ID
        /// </summary>
        public const string F_FollowerId = G_Basic + ".FollowerId";

        /// <summary>
        /// 创建时间
        /// </summary>
        public const string F_CreationTime = G_Basic + ".CreationTime";
    }
}
