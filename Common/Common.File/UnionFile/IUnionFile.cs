using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.File.UnionFile
{
    /// <summary>
    /// 文件
    /// </summary>
    public interface IUnionFile : IUnionFileEntity
    {
        /// <summary>
        /// 打开文件流
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        UnionFileStream Open(UnionFileOpenModel model = UnionFileOpenModel.Read);

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <returns></returns>
        UnionFileInfo GetFileInfo();
    }

    /// <summary>
    /// 文件打开的方式
    /// </summary>
    public enum UnionFileOpenModel
    {
        /// <summary>
        /// 读取
        /// </summary>
        Read,

        /// <summary>
        /// 写入（将覆盖已有的文件）
        /// </summary>
        Write,
    }
}
