using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.Entities;
using Common.Data;

namespace ServiceFairy.DbEntities.Group
{
    /// <summary>
    /// 群组中的用户
    /// </summary>
    [DbEntity("GroupMember", G_Basic, InitTableCount = 4)]
    public class DbGroupMember : DbGroupBase<DbGroupMember>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DbPrimaryKey(NullValue = Null_Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_Int32, IndexType = DbColumnIndexType.Slave)]
        public int UserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_DateTime)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 基础信息组
        /// </summary>
        public const string G_Basic = "Basic";

        /// <summary>
        /// 主键
        /// </summary>
        public const string F_ID = "ID";

        /// <summary>
        /// 成员ID
        /// </summary>
        public const string F_UserId = G_Basic + ".UserId";

        /// <summary>
        /// 创建时间
        /// </summary>
        public const string F_CreateTime = G_Basic + ".CreateTime";
    }
}
