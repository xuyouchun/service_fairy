using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.Install
{
    /// <summary>
    /// 安装目录整理
    /// </summary>
    class InstallPathInitializer
    {
        public InstallPathInitializer(InstallerContext ctx)
        {
            _ctx = ctx;
            _filePaths = ctx.FilePaths;
        }

        private readonly InstallerContext _ctx;

        private readonly FilePaths _filePaths;

        /// <summary>
        /// 执行目录的整理
        /// </summary>
        public void Execute()
        {
            
        }
    }
}
