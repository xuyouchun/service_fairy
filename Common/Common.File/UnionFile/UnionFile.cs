using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Utility;

namespace Common.File.UnionFile
{
    /// <summary>
    /// 基于真实物理文件的UnionFile
    /// </summary>
    public class UnionFile : IUnionFile
    {
        internal UnionFile(string logicPath, string phisicsPath, IUnionDirectory ownerDirectory)
        {
            Contract.Requires(phisicsPath != null && logicPath != null);
            _logicPath = logicPath;
            _phisicsPath = phisicsPath;
            OwnerDirectory = ownerDirectory;
        }

        private readonly string _phisicsPath, _logicPath;

        private string _GetCurrentVersionFile()
        {
            if (!Directory.Exists(_phisicsPath) || System.IO.File.Exists(_phisicsPath + "\\deleted"))
                return null;

            string[] files = Directory.GetFiles(_phisicsPath, "f_*");
            if (files.IsNullOrEmpty())
                return null;

            return files.Max();
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return _GetCurrentVersionFile() != null;
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <returns></returns>
        public UnionFileStream Open(UnionFileOpenModel openModel)
        {
            if (openModel == UnionFileOpenModel.Write)
            {
                PathUtility.CreateDirectoryIfNotExists(_phisicsPath);
                System.IO.File.Delete(_phisicsPath + "\\deleted");

                string postfix = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                string tempFile = Path.Combine(_phisicsPath, "tf_" + postfix);
                string newFile = Path.Combine(_phisicsPath, "f_" + postfix);

                return new WritableUnionFileStream(
                    new FileStream(tempFile, FileMode.Create, FileAccess.Write),
                    this, tempFile, newFile
                );
            }
            else
            {
                string currentFile = _GetCurrentVersionFile();
                if (currentFile == null)
                    return UnionFileStream.Null;

                return new UnionFileStream(new FileStream(currentFile, FileMode.Open, FileAccess.Read));
            }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name
        {
            get { return Path.GetFileName(_phisicsPath); }
        }

        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreationTime
        {
            get
            {
                string currentFile = _GetCurrentVersionFile();
                if (currentFile == null)
                    return default(DateTime);

                return System.IO.File.GetCreationTimeUtc(currentFile);
            }
        }

        /// <summary>
        /// 最后修改时间（UTC时间）
        /// </summary>
        public DateTime LastModifyTime
        {
            get 
            {
                string currentFile = _GetCurrentVersionFile();
                if (currentFile == null)
                    return default(DateTime);

                return System.IO.File.GetLastWriteTimeUtc(currentFile);
            }
        }

        /// <summary>
        /// 获取文件的信息
        /// </summary>
        /// <returns></returns>
        public UnionFileInfo GetFileInfo()
        {
            string currentFile = _GetCurrentVersionFile();
            if (currentFile == null)
                return null;

            FileInfo fInfo = new FileInfo(currentFile);
            return new UnionFileInfo(Name, _logicPath, fInfo.Length, fInfo.CreationTimeUtc, fInfo.LastWriteTimeUtc);
        }

        /// <summary>
        /// 文件所在目录
        /// </summary>
        public IUnionDirectory OwnerDirectory { get; private set; }

        #region Class WritableUnionFileStream ...

        class WritableUnionFileStream : UnionFileStream
        {
            public WritableUnionFileStream(Stream stream, UnionFile owner, string tempFile, string newFile)
                : base(stream)
            {
                _owner = owner;
                _tempFile = tempFile;
                _newFile = newFile;
            }

            private readonly UnionFile _owner;
            private readonly string _tempFile, _newFile;
            private volatile bool _disposed;

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    if (!_disposed)
                    {
                        if (!Canceled)
                        {
                            if (System.IO.File.Exists(_tempFile))
                                System.IO.File.Move(_tempFile, _newFile);
                        }
                        else
                        {
                            System.IO.File.Delete(_tempFile);
                        }

                        _disposed = true;
                    }
                }
            }
        }

        #endregion
    }
}
