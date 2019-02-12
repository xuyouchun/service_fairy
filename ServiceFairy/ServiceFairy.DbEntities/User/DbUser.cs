using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.Entities;
using Common.Data;
using Common.Contracts.Service;
using Common.Utility;

namespace ServiceFairy.DbEntities.User
{
    /// <summary>
    /// 用户表
    /// </summary>
    [DbEntity("User", G_Basic, InitTableCount = 4)]
    public class DbUser : DbUserBase<DbUser>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DbColumn(G_Basic, Size = 128, IndexType = DbColumnIndexType.Slave)]
        public string UserName { get; set; }

#warning 需要加上用户帐号类型

        /// <summary>
        /// 姓名
        /// </summary>
        [DbColumn(G_Basic, Size = 128)]
        public string Name { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_DateTime)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 帐号状态（已激活、已停用）
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_Boolean)]
        public bool Enable { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DbColumn(G_Basic, ColumnType = DbColumnType.AnsiString, Size = 128)]
        public string Password { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        [DbColumn(G_Basic, ColumnType = DbColumnType.AnsiString, Size = 128)]
        public string Sid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DbColumn(G_Basic, Size = 512)]
        public string Status { get; set; }

        /// <summary>
        /// 状态连接
        /// </summary>
        [DbColumn(G_Basic, Size = 1024)]
        public string StatusUrl { get; set; }

        /// <summary>
        /// 状态变化时间
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_DateTime)]
        public DateTime StatusChangedTime { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        [DbColumn(G_Detail, ColumnType = DbColumnType.String, Size = 0)]
        public string Detail { get; set; }

        /// <summary>
        /// 详细信息变化时间
        /// </summary>
        [DbColumn(G_Detail, NullValue = Null_DateTime)]
        public DateTime DetailChangedTime { get; set; }

        #region 常量定义 ...

        /// <summary>
        /// 基础信息
        /// </summary>
        public const string G_Basic = "Basic";

        /// <summary>
        /// 详细信息
        /// </summary>
        public const string G_Detail = "Detail";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string F_UserName = G_Basic + ".UserName";

        /// <summary>
        /// 姓名
        /// </summary>
        public const string F_Name = G_Basic + ".Name";

        /// <summary>
        /// 注册时间
        /// </summary>
        public const string F_CreateTime = G_Basic + ".CreateTime";

        /// <summary>
        /// 帐号状态
        /// </summary>
        public const string F_Enable = G_Basic + ".Enable";

        /// <summary>
        /// 密码
        /// </summary>
        public const string F_Password = G_Basic + ".Password";

        /// <summary>
        /// 安全码
        /// </summary>
        public const string F_Sid = G_Basic + ".Sid";

        /// <summary>
        /// 状态
        /// </summary>
        public const string F_Status = G_Basic + ".Status";

        /// <summary>
        /// 状态连接
        /// </summary>
        public const string F_StatusUrl = G_Basic + ".StatusUrl";

        /// <summary>
        /// 状态变化时间
        /// </summary>
        public const string F_StatusChangedTime = G_Basic + ".StatusChangedTime";

        /// <summary>
        /// 详细信息
        /// </summary>
        public const string F_Detail = G_Detail + ".Detail";

        /// <summary>
        /// 详细信息变化时间
        /// </summary>
        public const string F_DetailChangedTime = G_Detail + ".DetailChangedTime";

        #endregion

        /// <summary>
        /// 详细信息转换为字典
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static Dictionary<string, string> DetailToDict(string detail)
        {
            if (string.IsNullOrEmpty(detail))
                return new Dictionary<string, string>();

            return JsonUtility.Deserialize<Dictionary<string, string>>(detail);
        }

        /// <summary>
        /// 将详细信息字典转换为字符串
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string DetailToString(Dictionary<string, string> dict)
        {
            if (dict == null)
                return "";

            return JsonUtility.SerializeToString(dict);
        }
    }
}
