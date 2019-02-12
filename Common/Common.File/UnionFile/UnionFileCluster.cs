using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.File.UnionFile
{
    /// <summary>
    /// 基于真实物理文件目录的簇
    /// </summary>
    public class UnionFileCluster : IUnionFileCluster
    {
        public UnionFileCluster(string baseDirectory)
        {
            Contract.Requires(baseDirectory != null);

            _directory = new UnionDirectory("", baseDirectory, null);
        }

        private readonly UnionDirectory _directory;

        /// <summary>
        /// 根目录
        /// </summary>
        public IUnionDirectory Root
        {
            get
            {
                return _directory;
            }
        }
    }
}
