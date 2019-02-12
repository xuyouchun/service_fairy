using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.File.UnionFile
{
    /// <summary>
    /// UnionFile的实体
    /// </summary>
    public interface IUnionFileEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 父目录或文件所在目录
        /// </summary>
        IUnionDirectory OwnerDirectory { get; }

        /// <summary>
        /// 判断文件或目录是否存在
        /// </summary>
        /// <returns></returns>
        bool Exists();
    }
}
