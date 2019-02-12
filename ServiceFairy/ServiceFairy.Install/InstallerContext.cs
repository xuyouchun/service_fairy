using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.Install
{
    class InstallerContext
    {
        public InstallerContext(string targetPath)
        {
            FilePaths = new FilePaths(targetPath);
        }

        public FilePaths FilePaths { get; private set; }
    }
}
