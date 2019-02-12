using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using Common.WinForm;
using Common.Utility;
using Common.Framework.Management.DockingWindow;
using System.Threading;
using ServiceFairy.Entities.Sys;
using System.IO;
using System.Diagnostics;
using Common;

namespace ServiceFairy.Management.AppServices
{
    partial class AppServiceFileNode
    {
        private static AppServiceFileData _DownloadFile(IUIOperation op, SystemInvoker invoker,
            string filename, Guid clientId, ServiceDesc serviceDesc, string title)
        {
            return op.InvokeWithProgressWindow(
                () => invoker.Sys.DownloadAppServiceFile(filename, clientId, serviceDesc),
                title, TimeSpan.FromMilliseconds(500), true
            );
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        class OpenFileAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation op = context.ServiceProvider.GetService<IUIOperation>(true);
                AppServiceFileData fd = _DownloadFile(op, Invoker, FileInfo.FileName, ClientCtx.ClientDesc.ClientID, ServiceDesc, "正在下载文件 ...");

                if (fd != null && fd.Content != null)
                {
                    string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "_" + fd.FileName);
                    File.WriteAllBytes(file, fd.Content);
                    Process.Start(file);
                }
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        class DownloadFileAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation op = context.ServiceProvider.GetService<IUIOperation>(true);
                string filename = op.SelectFile("下载文件", fileName: Path.GetFileName(Kernel._fileInfo.FileName), saveModel: true);
                if (filename == null)
                    return;

                AppServiceFileData fd = _DownloadFile(op, Invoker, FileInfo.FileName, ClientCtx.ClientDesc.ClientID, ServiceDesc, "正在下载文件 ...");
                if (fd != null && fd.Content != null)
                {
                    File.WriteAllBytes(filename, fd.Content);
                    Process.Start(Path.GetDirectoryName(filename));
                }
            }
        }
    }
}
