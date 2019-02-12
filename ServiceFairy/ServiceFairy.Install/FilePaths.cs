using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ServiceFairy.Install
{
    /// <summary>
    /// 配置
    /// </summary>
    public class FilePaths
    {
        public FilePaths(string targetPath)
        {
            TargetPath = targetPath;

            InstallPath = Path.Combine(targetPath, "Install");
            ServicePath = Path.Combine(targetPath, "Service");
            ResourcePath = Path.Combine(targetPath, "Resource");
            AssemblyPath = Path.Combine(targetPath, "Assembly");
        }

        /// <summary>
        /// 安装路径
        /// </summary>
        public string TargetPath { get; private set; }

        /// <summary>
        /// 存放与安装相关的文件
        /// </summary>
        public string InstallPath { get; private set; }

        /// <summary>
        /// 服务文件的目录
        /// </summary>
        public string ServicePath { get; private set; }

        /// <summary>
        /// 资源目录
        /// </summary>
        public string ResourcePath { get; private set; }

        /// <summary>
        /// 程序集目录
        /// </summary>
        public string AssemblyPath { get; private set; }
    }
}
