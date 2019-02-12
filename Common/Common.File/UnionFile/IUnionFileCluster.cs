using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.File.UnionFile
{
    /// <summary>
    /// 文件的簇
    /// </summary>
    public interface IUnionFileCluster
    {
        /// <summary>
        /// 根目录
        /// </summary>
        IUnionDirectory Root { get; }
    }
}
