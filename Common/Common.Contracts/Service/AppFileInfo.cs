using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Utility;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 文件的信息
    /// </summary>
    [Serializable, DataContract]
    public class AppFileInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        [DataMember]
        public DateTime LastModifyTime { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [DataMember]
        public long Size { get; set; }

        /// <summary>
        /// 读取指定文件的AppFileInfo
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static AppFileInfo LoadFromFile(string basePath, string file)
        {
            Contract.Requires(basePath != null && file != null);
            FileInfo fInfo = new FileInfo(Path.Combine(basePath, file));
            return new AppFileInfo() {
                FileName = file,
                Size = fInfo.Length,
                LastModifyTime = fInfo.LastWriteTimeUtc,
            };
        }

        /// <summary>
        /// 从指定的目录中加载
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static AppFileInfo[] LoadFromDirectory(string directory)
        {
            Contract.Requires(directory != null);

            return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories)
                .ToArray(f => LoadFromFile(directory, f.Substring(directory.Length).TrimStart('\\', '/')));
        }
    }
}
