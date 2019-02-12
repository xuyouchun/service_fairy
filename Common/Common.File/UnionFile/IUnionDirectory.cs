using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.File.UnionFile
{
    /// <summary>
    /// 文件
    /// </summary>
    public interface IUnionDirectory : IUnionFileEntity, IEnumerable<IUnionFileEntity>
    {
        /// <summary>
        /// 获取文件数量
        /// </summary>
        /// <param name="pattern">通配符</param>
        /// <param name="includeMarkDeleted">是否包含已标记删除的文件</param>
        /// <returns></returns>
        int GetFileCount(string pattern = null, bool includeMarkDeleted = false);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="count">数量</param>
        /// <param name="pattern">通配符</param>
        /// <param name="includeMarkDeleted">是否包含已标记删除的文件</param>
        /// <returns></returns>
        IUnionFile[] GetFiles(int start = 0, int count = int.MaxValue, string pattern = null, bool includeMarkDeleted = false);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IUnionFile GetFile(string name);

        /// <summary>
        /// 是否包含指定的文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ExistsFile(string name);

        /// <summary>
        /// 获取目录数量
        /// </summary>
        /// <param name="pattern">通配符</param>
        /// <param name="includeMarkDeleted">是否包含已标记删除的目录</param>
        /// <returns></returns>
        int GetDirectoryCount(string pattern = null, bool includeMarkDeleted = false);

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="count">数量</param>
        /// <param name="pattern">通配符</param>
        /// <param name="includeMarkDeleted">是否包含已标记删除的目录</param>
        /// <returns></returns>
        IUnionDirectory[] GetDirectories(int start = 0, int count = int.MaxValue, string pattern = null, bool includeMarkDeleted = false);

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IUnionDirectory GetDirectory(string name);

        /// <summary>
        /// 是否包含指定的目录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ExistsDirectory(string name);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="markDelete">是否标记删除</param>
        /// <returns></returns>
        bool DeleteFile(string name, bool markDelete = true);

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="markDelete">是否标记删除</param>
        /// <returns></returns>
        bool DeleteDirectory(string name, bool markDelete = true);

        /// <summary>
        /// 文件信息
        /// </summary>
        UnionDirectoryInfo GetDirectoryInfo();

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <param name="recursive">是否递归子目录</param>
        /// <param name="includeMarkDeleted">是否包括已标记删除的文件或目录</param>
        /// <returns></returns>
        IEnumerator<IUnionFileEntity> GetEnumerator(bool recursive, bool includeMarkDeleted);
    }
}
