using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;

namespace Common.Package.Storage
{
    /// <summary>
    /// 头部信息
    /// </summary>
    public class StreamTableHeaderInfo
    {
        internal StreamTableHeaderInfo(SVersion version, DateTime creationTime, int tableCount, string name, string desc)
        {
            CreationTime = creationTime;
            Version = version;
            TableCount = tableCount;
            Name = name;
            Desc = desc;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public SVersion Version { get; private set; }

        /// <summary>
        /// 表格数量
        /// </summary>
        public int TableCount { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Desc { get; private set; }

        public override string ToString()
        {
            return string.Format("Version={0}, CreationTime={1}, TableCount={2}, Desc={3}",
                Version, CreationTime, TableCount, Desc);
        }
    }
}
