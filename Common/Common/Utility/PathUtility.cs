using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.Diagnostics;

namespace Common.Utility
{
    /// <summary>
    /// 关于路径的工具
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class PathUtility
    {
        /// <summary>
        /// 将多个路径合并在一起，并忽略其中的“..”与“.”
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            return Revise(Path.Combine(paths));
        }

        /// <summary>
        /// 修正路径，将其中的"."及".."合并
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Revise(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            if (path.IndexOf('.') < 0)
                return path;

            string[] parts = path.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string[] newParts = new string[parts.Length];
            bool isPathRoot = Path.IsPathRooted(parts[0]);

            int newIndex = 0;
            for (int index = 0; index < parts.Length; index++)
            {
                string part = parts[index];
                if (part == ".")
                    continue;

                if (part == "..")
                {
                    if (newIndex == 0 || newIndex == 1 && isPathRoot)
                        throw new FormatException("路径格式不正确");

                    newIndex--;
                }
                else
                {
                    newParts[newIndex++] = part;
                }
            }

            return string.Join("\\", newParts, 0, newIndex);
        }

        private static readonly HashSet<char> _invalidFileNameChars = Path.GetInvalidFileNameChars().ToHashSet();

        public static string ReplaceInvalidFileNameChars(string filename, string replaceTo)
        {
            if (string.IsNullOrEmpty(filename))
                return "";

            StringBuilder sb = new StringBuilder();
            for (int k = 0, length = filename.Length; k < length; k++)
            {
                char ch = filename[k];
                if (_invalidFileNameChars.Contains(ch))
                    sb.Append(replaceTo);
                else
                    sb.Append(ch);
            }

            return sb.ToString();
        }

        private static readonly HashSet<char> _invalidPathChars = Path.GetInvalidPathChars().ToHashSet();

        public static string ReplaceInvalidPathChars(string path, string replaceTo)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            StringBuilder sb = new StringBuilder();
            for (int k = 0, length = path.Length; k < length; k++)
            {
                char ch = path[k];
                if (_invalidPathChars.Contains(ch))
                    sb.Append(replaceTo);
                else
                    sb.Append(ch);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 为指定的文件创建目录
        /// </summary>
        /// <param name="file"></param>
        public static void CreateDirectoryForFile(string file)
        {
            Contract.Requires(file != null);

            string dir = Path.GetDirectoryName(file);
            CreateDirectoryIfNotExists(dir);
        }

        /// <summary>
        /// 如果存在指定的文件，则读取其全部内容，否则返回null
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadAllTextIfExists(string file, Encoding encoding)
        {
            Contract.Requires(file != null);

            if (!File.Exists(file))
                return null;

            return File.ReadAllText(file, encoding);
        }

        /// <summary>
        /// 如果存在指定的文件，则读取其全部内容，否则返回null
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadAllTextIfExists(string file)
        {
            Contract.Requires(file != null);

            if (!File.Exists(file))
                return null;

            return File.ReadAllText(file);
        }

        /// <summary>
        /// 如果存在指定的文件，则读取其全部字节，否则返回null
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytesIfExists(string file)
        {
            Contract.Requires(file != null);

            if (!File.Exists(file))
                return null;

            return File.ReadAllBytes(file);
        }

        /// <summary>
        /// 如果不存在该目录，则创建
        /// </summary>
        /// <param name="directory"></param>
        public static void CreateDirectoryIfNotExists(string directory)
        {
            Contract.Requires(directory != null);

            if (!Directory.Exists(directory))
            {
                lock (string.Intern(directory))
                {
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                }
            }
        }

        /// <summary>
        /// 清空指定的路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="autoCreate"></param>
        /// <param name="throwError"></param>
        public static void ClearPath(string path, bool autoCreate = false, bool throwError = false)
        {
            Contract.Requires(path != null);

            try
            {
                if (!Directory.Exists(path))
                {
                    if (autoCreate)
                        CreateDirectoryIfNotExists(path);

                    return;
                }

                Directory.Delete(path, true);
            }
            catch (Exception)
            {
                if (throwError)
                    throw;
            }
        }

        /// <summary>
        /// 将一个路径下的所有文件拷贝到另一个路径
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        /// <param name="overwrite"></param>
        /// <param name="throwError"></param>
        public static void CopyDirectory(string sourceDirectory, string destDirectory, bool overwrite = true, bool throwError = true)
        {
            Contract.Requires(sourceDirectory != null && destDirectory != null);

            CopyDirectory(sourceDirectory, destDirectory, delegate(string src, string dst) {
                return overwrite || !File.Exists(dst);
            }, throwError);
        }

        /// <summary>
        /// 将一个路径下较新的文件拷贝到另一个路径
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        /// <param name="throwError"></param>
        public static void CopyDirectoryIfNewer(string sourceDirectory, string destDirectory, bool throwError = true)
        {
            Contract.Requires(sourceDirectory != null && destDirectory != null);

            CopyDirectory(sourceDirectory, destDirectory, delegate(string src, string dst) {
                return !File.Exists(dst) || File.GetLastWriteTimeUtc(src) > File.GetLastWriteTimeUtc(dst);
            }, throwError);
        }

        /// <summary>
        /// 将一个路径下的符合条件的文件拷贝到另一个路径
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        /// <param name="condition"></param>
        /// <param name="throwError"></param>
        public static void CopyDirectory(string sourceDirectory, string destDirectory, Func<string, string, bool> condition, bool throwError = true)
        {
            Contract.Requires(sourceDirectory != null && destDirectory != null);

            CreateDirectoryIfNotExists(destDirectory);
            foreach (string sourceFile in Directory.GetFiles(sourceDirectory))
            {
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(destDirectory, fileName);

                if (condition == null || condition(sourceFile, destFile))
                {
                    try
                    {
                        File.Copy(sourceFile, destFile, true);
                    }
                    catch (Exception)
                    {
                        if (throwError)
                            throw;
                    }
                }
            }

            foreach (string sourcePath in Directory.GetDirectories(sourceDirectory))
            {
                string dir = Path.GetFileName(sourcePath);
                string destPath = Path.Combine(destDirectory, dir);

                CopyDirectory(sourcePath, destPath, condition);
            }
        }

        /// <summary>
        /// 如果文件存在则拷贝
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFile"></param>
        /// <param name="overwrite"></param>
        public static void CopyFileIfExist(string sourceFile, string destFile, bool overwrite = false)
        {
            Contract.Requires(sourceFile != null && destFile != null);

            if (File.Exists(sourceFile))
                File.Copy(sourceFile, destFile, overwrite);
        }

        /// <summary>
        /// 向指定的文件写入文本，如果目录不存在，则自动创建
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <param name="throwError">是否抛出异常</param>
        public static void WriteAllText(string file, string content, Encoding encoding, bool throwError = false)
        {
            Contract.Requires(file != null);

            try
            {
                CreateDirectoryForFile(file);
                File.WriteAllText(file, content, encoding);
            }
            catch (Exception)
            {
                if (throwError)
                    throw;
            }
        }

        private static string _executePath;
        /// <summary>
        /// 当前可执行程序文件所在的目录
        /// </summary>
        /// <returns></returns>
        public static string GetExecutePath()
        {
            return _executePath ?? (_executePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
        }

        /// <summary>
        /// 如果存在则删除
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteIfExists(string path)
        {
            Contract.Requires(path != null);

            lock (string.Intern(path.ToLower()))
            {
                if (File.Exists(path))
                    File.Delete(path);
                else if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// 获取以当前时间和随机数组成的路径
        /// </summary>
        /// <returns></returns>
        public static string GetUtcTimePath()
        {
            return DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-ffff-" + _timePathRandom.Next(10000).ToString().PadLeft(4, '0'));
        }

        private static readonly Random _timePathRandom = new Random();

        /// <summary>
        /// 给文件名加一个前缀，例如：c:\abc\def.txt，加一个前缀“prefix_”，变为：c:\abc\prefix_def.txt
        /// </summary>
        /// <param name="path"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string AddPrefix(string path, string prefix)
        {
            Contract.Requires(path != null);
            if (string.IsNullOrEmpty(prefix))
                return path;

            int index = path.LastIndexOf('\\');
            return path.Insert(index + 1, prefix);
        }

        /// <summary>
        ///  给文件名加一个后缀，例如：c:\abc\def.txt，加一个后缀“_postfix”，变为：c:\abc\def_postfix.txt
        /// </summary>
        /// <param name="path"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        public static string AddPostfix(string path, string postfix)
        {
            Contract.Requires(path != null);
            if (string.IsNullOrEmpty(postfix))
                return path;

            int index = path.LastIndexOf('.');
            if (index < 0)
                return path + postfix;

            return path.Insert(index, postfix);
        }

        /// <summary>
        /// 是否为文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFile(string path)
        {
            return path != null && File.Exists(path);
        }

        /// <summary>
        /// 是否为目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectory(string path)
        {
            return path != null && Directory.Exists(path);
        }

        /// <summary>
        /// 判断文件是否具有只读属性
        /// </summary>
        /// <param name="path"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static bool IsReadOnly(string path, bool throwError = false)
        {
            if (IsFile(path))
            {
                return File.GetAttributes(path).HasFlag(FileAttributes.ReadOnly);
            }
            else if(IsDirectory(path))
            {
                DirectoryInfo dInfo = new DirectoryInfo(path);
                return dInfo.Attributes.HasFlag(FileAttributes.ReadOnly);
            }

            if (throwError)
                throw new FileNotFoundException("文件或目录：" + path + "不存在");

            return false;
        }
    }
}
