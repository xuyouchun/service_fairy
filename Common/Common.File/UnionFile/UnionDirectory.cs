using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Utility;
using U = Common.File.UnionFile.UnionFileUtility;

namespace Common.File.UnionFile
{
    /// <summary>
    /// 基于真实物理目录的UnionFileDirectory
    /// </summary>
    public class UnionDirectory : IUnionDirectory
    {
        public UnionDirectory(string logicPath, string physicsPath, IUnionDirectory ownerDirectory)
        {
            Contract.Requires(logicPath != null && physicsPath != null);
            _logicPath = logicPath;
            _physicsPath = physicsPath;
            OwnerDirectory = ownerDirectory;
        }

        private readonly string _logicPath, _physicsPath;

        public int GetFileCount(string pattern = "*", bool includeMarkDeleted = false)
        {
            string path = U.GetFilePhysicsPath(_physicsPath);
            if (!Directory.Exists(path))
                return 0;

            return Directory.EnumerateDirectories(path, pattern ?? "*")
                .Count(f => includeMarkDeleted || !System.IO.File.Exists(f + "\\deleted")
            );
        }

        public IUnionFile[] GetFiles(int start, int count, string pattern = "*", bool includeMarkDeleted = false)
        {
            string path = U.GetFilePhysicsPath(_physicsPath);
            if (!Directory.Exists(path))
                return Array<IUnionFile>.Empty;

            return Directory.EnumerateDirectories(path, pattern ?? "*")
                .Where(f => includeMarkDeleted || !System.IO.File.Exists(f + "\\deleted"))
                .Range(start, count).ToArray(f => new UnionFile(_CombineToLogicPath(Path.GetFileName(f)), f, this));
        }

        public IUnionFile GetFile(string name)
        {
            _ValidatePath(name);
            return new UnionFile(_CombineToLogicPath(name), Path.Combine(U.GetFilePhysicsPath(_physicsPath), name), this);
        }

        public bool ExistsFile(string name)
        {
            _ValidatePath(name);
            return U.IsFileExists(_physicsPath, name);
        }

        public int GetDirectoryCount(string pattern = "*", bool includeMarkDeleted = false)
        {
            string path = U.GetDirectoryPhysicsPath(_physicsPath);
            if(!Directory.Exists(path))
                return 0;

            return Directory.GetDirectories(path, pattern ?? "*")
                .Count(d => includeMarkDeleted || !System.IO.File.Exists(d + "\\deleted")
            );
        }

        public IUnionDirectory[] GetDirectories(int start, int count, string pattern = "*", bool includeMarkDeleted = false)
        {
            string path = U.GetDirectoryPhysicsPath(_physicsPath);
            if (!Directory.Exists(path))
                return Array<IUnionDirectory>.Empty;
            
            return Directory.EnumerateDirectories(path, pattern ?? "*", SearchOption.TopDirectoryOnly)
                .Where(d => includeMarkDeleted || !System.IO.File.Exists(d + "\\deleted"))
                .Range(start, count).ToArray(d => new UnionDirectory(_CombineToLogicPath(Path.GetFileName(d)), d, this));
        }

        public IUnionDirectory GetDirectory(string name)
        {
            _ValidatePath(name);

            return new UnionDirectory(
                _CombineToLogicPath(name),
                Path.Combine(_physicsPath, name), this
            );
        }

        private string _CombineToLogicPath(string name)
        {
            if (string.IsNullOrEmpty(_logicPath))
                return name;

            if (_logicPath.EndsWith('/'))
                return _logicPath + name.TrimStart('/');

            return _logicPath + "/" + name.TrimStart('/');
        }

        private void _ValidatePath(string name)
        {
            Contract.Requires(name != null);

            if (name.IndexOfAny(_errorPathChars) >= 0)
                throw new ArgumentException("错误的目录或文件名，不允许包含“/”与“\\”");
        }

        private static readonly char[] _errorPathChars = new char[] { '\\', '/' };

        public bool ExistsDirectory(string name)
        {
            _ValidatePath(name);
            return U.IsDirectoryExists(_physicsPath, name);
        }

        public bool DeleteFile(string name, bool markDelete = true)
        {
            _ValidatePath(name);
            return U.DeleteFile(_physicsPath, name, markDelete);
        }

        public bool DeleteDirectory(string name, bool markDelete = true)
        {
            _ValidatePath(name);
            return U.DeleteDirectory(_physicsPath, name, markDelete);
        }

        /// <summary>
        /// 父目录
        /// </summary>
        public IUnionDirectory OwnerDirectory { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return Path.GetFileName(_physicsPath); }
        }

        /// <summary>
        /// 获取文件的信息
        /// </summary>
        /// <returns></returns>
        public UnionDirectoryInfo GetDirectoryInfo()
        {
            if (!Directory.Exists(_physicsPath))
                return null;

            DirectoryInfo dInfo = new DirectoryInfo(_physicsPath);
            return new UnionDirectoryInfo(Name, _physicsPath, dInfo.CreationTimeUtc, dInfo.LastWriteTimeUtc);
        }

        /// <summary>
        /// 该目录是否真实存在
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return Directory.Exists(_physicsPath);
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <param name="recursive"></param>
        /// <param name="includeMarkDeleted"></param>
        /// <returns></returns>
        public IEnumerator<IUnionFileEntity> GetEnumerator(bool recursive, bool includeMarkDeleted)
        {
            foreach (IUnionFile file in GetFiles(0, int.MaxValue))
            {
                yield return file;
            }

            foreach (IUnionDirectory directory in GetDirectories(0, int.MaxValue))
            {
                yield return directory;

                if (recursive)
                {
                    using (IEnumerator<IUnionFileEntity> subEnumerator = directory.GetEnumerator(true, includeMarkDeleted))
                    {
                        while (subEnumerator.MoveNext())
                        {
                            yield return subEnumerator.Current;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IUnionFileEntity> GetEnumerator()
        {
            return GetEnumerator(false, false);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
