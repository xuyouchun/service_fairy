using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using System.Diagnostics;
using System.IO;

namespace ServiceFairy.Management.DeployPackage
{
    partial class DeployPackageFileNode
    {
        /// <summary>
        /// 打开
        /// </summary>
        class OpenAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                Process.Start(Path.Combine(BaseDirectory, FileInfo.FileName));
            }
        }

        /// <summary>
        /// 打开所在目录
        /// </summary>
        class OpenFolderAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                Process.Start(BaseDirectory);
            }
        }
    }
}
