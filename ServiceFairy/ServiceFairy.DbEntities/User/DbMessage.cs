using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Data;
using Common.Data.Entities;

namespace ServiceFairy.DbEntities.User
{
    /// <summary>
    /// 消息实体
    /// </summary>
    [DbEntity("Message", G_Basic, InitTableCount = 4)]
    public class DbMessage : DbUserBase<DbMessage>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DbPrimaryKey(NullValue = Null_Int64)]
        public long ID { get; set; }

        /// <summary>
        /// 发送者ID
        /// </summary>
        [DbColumn(G_Basic)]
        public int From { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        [DbColumn(G_Basic, Size = 128)]
        public string Method { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [DbColumn(G_Basic, ColumnType = DbColumnType.AnsiString, Size = 0)]
        public string Data { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DbColumn(G_Basic)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DbColumn(G_Basic)]
        public int Property { get; set; }

        /// <summary>
        /// 基础信息组
        /// </summary>
        public const string G_Basic = "Basic";

        /// <summary>
        /// 主键
        /// </summary>
        public const string F_ID = "ID";

        /// <summary>
        /// 发送者
        /// </summary>
        public const string F_From = G_Basic + ".From";

        /// <summary>
        /// 方法
        /// </summary>
        public const string F_Method = G_Basic + ".Method";

        /// <summary>
        /// 消息内容
        /// </summary>
        public const string F_Data = G_Basic + ".Data";

        /// <summary>
        /// 消息内容格式
        /// </summary>
        public const string F_Format = G_Basic + ".Format";

        /// <summary>
        /// 时间
        /// </summary>
        public const string F_Time = G_Basic + ".Time";

        /// <summary>
        /// 类型
        /// </summary>
        public const string F_Property = G_Basic + ".Property";
    }
}
