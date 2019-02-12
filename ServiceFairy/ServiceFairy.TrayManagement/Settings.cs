using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common.Utility;
using System.IO;

namespace ServiceFairy.TrayManagement
{
    static class Settings
    {
        static Settings()
        {
            
        }

        public const string ServiceName = "ServiceFairy";

        /// <summary>
        /// 当前程序执行文件的路径
        /// </summary>
        public static string ExecutePath
        {
            get { return PathUtility.GetExecutePath(); }
        }

        /// <summary>
        /// 配置文件的路径
        /// </summary>
        public static string ConfigurationFile
        {
            get { return Path.Combine(ExecutePath, "Configuration.xml"); }
        }
    }
}
