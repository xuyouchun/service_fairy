using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.Entities;
using Common.Data;
using Common.Utility;

namespace ServiceFairy.DbEntities.Group
{
    /// <summary>
    /// 群组
    /// </summary>
    [DbEntity("Group", G_Basic, InitTableCount = 4)]
    public class DbGroup : DbGroupBase<DbGroup>
    {
        public DbGroup()
        {

        }

        /// <summary>
        /// 群组名称
        /// </summary>
        [DbColumn(G_Basic, Size = 128, IndexType = DbColumnIndexType.Slave)]
        public string Name { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [DbColumn(G_Basic)]
        public int Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_DateTime)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_Boolean)]
        public bool Enable { get; set; }

        /// <summary>
        /// 信息或成员变化时间
        /// </summary>
        [DbColumn(G_Basic, NullValue = Null_DateTime)]
        public DateTime ChangedTime { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        [DbColumn(G_Detail, ColumnType = DbColumnType.String, Size = 0)]
        public string Detail { get; set; }

        /// <summary>
        /// 基础信息组
        /// </summary>
        public const string G_Basic = "Basic";

        /// <summary>
        /// 详细信息组
        /// </summary>
        public const string G_Detail = "Detail";

        /// <summary>
        /// 群组名称
        /// </summary>
        public const string F_Name = G_Basic + ".Name";

        /// <summary>
        /// 创建者
        /// </summary>
        public const string F_Creator = G_Basic + ".Creator";

        /// <summary>
        /// 创建时间
        /// </summary>
        public const string F_CreateTime = G_Basic + ".CreateTime";

        /// <summary>
        /// 是否启用
        /// </summary>
        public const string F_Enable = G_Basic + ".Enable";

        /// <summary>
        /// 详细信息变化时间
        /// </summary>
        public const string F_ChangedTime = G_Basic + ".ChangedTime";

        /// <summary>
        /// 详细信息
        /// </summary>
        public const string F_Detail = G_Detail + ".Detail";

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
        /// 详细信息转换为字符串
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
