using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.File.UnionFile
{
    /// <summary>
    /// 目录信息
    /// </summary>
    [Serializable, DataContract]
    public class UnionDirectoryInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="creationTime"></param>
        /// <param name="lastModifytime"></param>
        public UnionDirectoryInfo(string name, string path, DateTime creationTime, DateTime lastModifytime)
        {
            Name = name;
            Path = path;
            CreationTime = creationTime;
            LastModifyTime = lastModifytime;
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
