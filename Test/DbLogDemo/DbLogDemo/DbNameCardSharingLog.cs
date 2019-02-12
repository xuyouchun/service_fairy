using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Data.Entities;

namespace DbLogDemo
{
    [DbTableGroupEntity("DbNameCardSharingLog"), DbEntity("DbNameCardSharingLog", G_Basic, InitTableCount = 4)]
    public class DbNameCardSharingLog : DbEntity<DbNameCardSharingLog>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DbRouteKey(NullValue = Null_Int32)]
        public int Id { get; set; }

        /// <summary>
        /// GeoKey
        /// </summary>
        [DbColumn(G_Basic)]
        public string GeoKey { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DbColumn(G_Basic)]
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DbColumn(G_Basic)]
        public string UserName { get; set; }

        /// <summary>
        /// 日志源
        /// </summary>
        [DbColumn(G_Basic)]
        public string LogSource { get; set; }

        /// <summary>
        /// 日志日期
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_DateTime)]
        public DateTime LogDate { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        [DbColumn(G_Basic)]
        public string Device { get; set; }

        /// <summary>
        /// Rom
        /// </summary>
        [DbColumn(G_Basic)]
        public string Rom { get; set; }

        /// <summary>
        /// 主分组
        /// </summary>
        public const string G_Basic = "Basic";

        /// <summary>
        /// 主键
        /// </summary>
        public const string F_Id = G_Basic +  ".Id";

        /// <summary>
        /// GeoKey
        /// </summary>
        public const string F_GeoKey = G_Basic +  ".GeoKey";

        /// <summary>
        /// UserId
        /// </summary>
        public const string F_UserId = G_Basic + ".UserId";

        /// <summary>
        /// UserName
        /// </summary>
        public const string F_UserName = G_Basic + ".UserName";

        /// <summary>
        /// LogSource
        /// </summary>
        public const string F_LogSource = G_Basic + ".LogSource";

        /// <summary>
        /// LogDate
        /// </summary>
        public const string F_LogDate = G_Basic + ".LogDate";

        /// <summary>
        /// Device
        /// </summary>
        public const string F_Device = G_Basic + ".Device";

        /// <summary>
        /// Rom
        /// </summary>
        public const string F_Rom = G_Basic + ".Rom";
    }
}
