using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 文件系统的工具类
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// 获取指定用户的文件路径
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetPathOfUser(int userId)
        {
            string s = userId.ToString().PadLeft(10, '0');
            return "user/" + s.Substring(0, 5) + "/" + s.Substring(5, 3) + "/" + s.Substring(8);
        }
    }
}
