using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace Common.File.UnionFile
{
    public static class UnionFileUtility
    {
        /// <summary>
        /// 获取文件所在的物理路径
        /// </summary>
        /// <param name="logicPath"></param>
        /// <returns></returns>
        internal static string GetFilePhysicsPath(string logicPath)
        {
            return Path.Combine(logicPath, "f");
        }

        /// <summary>
        /// 获取文件删除标识的物理路径
        /// </summary>
        /// <param name="logicPath"></param>
        /// <returns></returns>
        internal static string GetDeletedFilePhysicsPath(string logicPath)
        {
            return GetFilePhysicsPath(logicPath) + "\\deleted";
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="logicPath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        internal static bool IsFileExists(string logicPath, string filename)
        {
            string path = Path.Combine(GetFilePhysicsPath(logicPath), filename);
            return !Directory.Exists(path) || System.IO.File.Exists(path + "\\deleted");
        }

        /// <summary>
        /// 获取目录所在的物理路径
        /// </summary>
        /// <param name="logicPath"></param>
        /// <returns></returns>
        internal static string GetDirectoryPhysicsPath(string logicPath)
        {
            return Path.Combine(logicPath, "d");
        }

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="logicPath"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        internal static bool IsDirectoryExists(string logicPath, string directory)
        {
            string path = Path.Combine(GetDirectoryPhysicsPath(logicPath), directory);
            return !Directory.Exists(path) || System.IO.File.Exists(path + "\\deleted");
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="logicPath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        internal static bool DeleteFile(string logicPath, string filename, bool markDelete)
        {
            string physicsPath = GetFilePhysicsPath(logicPath);
            string path = Path.Combine(physicsPath, filename);
            string deleteFlagPath;
            if (!Directory.Exists(path) || System.IO.File.Exists(deleteFlagPath = (path + "\\deleted")))
                return false;

            if (markDelete)
                System.IO.File.WriteAllBytes(deleteFlagPath, Array<byte>.Empty);
            else
                System.IO.Directory.Delete(physicsPath, true);

            return true;
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="logicPath"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        internal static bool DeleteDirectory(string logicPath, string directory, bool markDelete)
        {
            string physicsPath = GetDirectoryPhysicsPath(logicPath);
            string path = Path.Combine(physicsPath, directory);
            string deleteFlagPath;
            if (!Directory.Exists(path) || System.IO.File.Exists(deleteFlagPath = (path + "\\deleted")))
                return false;

            if (markDelete)
                System.IO.File.WriteAllBytes(deleteFlagPath, Array<byte>.Empty);
            else
                System.IO.Directory.Delete(physicsPath, true);

            return true;
        }

        internal static string CreateFileKey(string path)
        {
            return string.Intern("pathlock_" + path);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool Delete(this IUnionFile file)
        {
            Contract.Requires(file != null);

            IUnionDirectory dir = file.OwnerDirectory;
            if (dir == null)
                return false;

            return dir.DeleteFile(file.Name);
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static bool Delete(this IUnionDirectory directory)
        {
            Contract.Requires(directory != null);

            IUnionDirectory dir = directory.OwnerDirectory;
            if (dir == null)
                return false;

            return dir.DeleteDirectory(directory.Name);
        }

        /// <summary>
        /// 寻找指定路径的UnionFileDirectory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IUnionDirectory SearchDirectory(this IUnionDirectory directory, string path)
        {
            Contract.Requires(directory != null);

            if (string.IsNullOrEmpty(path))
                return directory;

            foreach (string name in path.Split('/'))
            {
                directory = directory.GetDirectory(name);
            }

            return directory;
        }

        /// <summary>
        /// 寻找指定路径下的UnionFile
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IUnionFile SearchFile(this IUnionDirectory directory, string path)
        {
            Contract.Requires(directory != null && path != null);

            string[] parts = path.Split('/');
            for (int k = 0; k < parts.Length - 1; k++)
            {
                directory = directory.GetDirectory(parts[k]);
            }

            return directory.GetFile(parts[parts.Length - 1]);
        }

        /// <summary>
        /// 寻找指定目录中的全部文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IUnionFile[] GetFiles(this IUnionDirectory directory, string pattern)
        {
            Contract.Requires(directory != null);
            return directory.GetFiles(pattern: pattern);
        }

        /// <summary>
        /// 寻找指定目录中的全部子目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IUnionDirectory[] GetDirectories(this IUnionDirectory directory, string pattern)
        {
            Contract.Requires(directory != null);
            return directory.GetDirectories(pattern: pattern);
        }
    }
}
