using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.Entities;
using Common.Data;

namespace ServiceFairy.DbEntities.Sms
{
    /// <summary>
    /// 短信发送日志
    /// </summary>
    [DbEntity("SmsLog", G_Basic, InitTableCount = 1), DbTableGroupEntity("Sms")]
    public class DbSmsLog : DbEntity<DbSmsLog>
    {
        public DbSmsLog()
        {

        }

        /// <summary>
        /// ID
        /// </summary>
        [DbPrimaryKey(NullValue = Null_Int32), DbRouteKey]
        public long Id { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [DbColumn(G_Basic, Size = 100)]
        public string Mobiles { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        [DbColumn(G_Basic, Size = 200 )]
        public string Content { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        [DbColumn(G_Basic, Size = 200)]
        public string Result { get; set; }

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
        /// 手机号码
        /// </summary>
        public const string F_Mobiles = G_Basic + ".Mobiles";

        /// <summary>
        /// 发送内容
        /// </summary>
        public const string F_Content = G_Basic + ".Content";

        /// <summary>
        /// 结果
        /// </summary>
        public const string F_Result = G_Basic + ".Result";

        /// <summary>
        /// 创建时间
        /// </summary>
        public const string F_CreateTime = G_Basic + ".CreateTime";
    }
}
