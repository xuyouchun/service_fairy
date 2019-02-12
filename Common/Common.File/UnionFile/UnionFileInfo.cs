using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.File.UnionFile
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [Serializable, DataContract]
    public class UnionFileInfo
    {
        public UnionFileInfo(string name, string path, long size, DateTime creationTime, DateTime lastModifyTime)
        {
            Name = name;
            Path = path;
            Size = size;
            CreationTime = creationTime;
            LastModifyTime = lastModifyTime;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; private set; }

        /// <summary>
        /// 文件尺寸
        /// </summary>
        [DataMember]
        public long Size { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DataMember]
        public DateTime LastModifyTime { get; private set; }
    }
}
