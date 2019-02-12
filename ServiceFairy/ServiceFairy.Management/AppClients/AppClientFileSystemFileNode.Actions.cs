using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.Tray;
using Common.Utility;
using IOPath = System.IO.Path;
using System.Diagnostics;
using System.IO;

namespace ServiceFairy.Management.AppClients
{
	partial class AppClientFileSystemFileNode
	{
        private static byte[] _DownloadFile(IUIOperation op, SystemInvoker invoker,
            string path, Guid clientId, string title = null)
        {
            return op.InvokeWithProgressWindow(() => {
                FsFileInfo info;
                return invoker.Tray.FsDownloadFile(path, out info, clientId);
            }, title ?? "正在下载文件 ...", TimeSpan.FromMilliseconds(500), true);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        class OpenAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation op = context.ServiceProvider.GetService<IUIOperation>(true);
                string filePath = System.IO.Path.Combine(DirectoryInfoItem.Directory, Info.Path);
                byte[] content = _DownloadFile(op, Invoker, filePath, ClientCtx.AppClientInfo.ClientId);

                if (content != null)
                {
                    string file = IOPath.Combine(IOPath.GetTempPath(), Guid.NewGuid().ToString() + "_" + IOPath.GetFileName(Info.Path));
                    File.WriteAllBytes(file, content);
                    Process.Start(file);
                }
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        class DownloadAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation op = context.ServiceProvider.GetService<IUIOperation>(true);
                string filePath = System.IO.Path.Combine(DirectoryInfoItem.Directory, Info.Path);
                string filename = op.SelectFile("下载文件", fileName: IOPath.GetFileName(filePath), saveModel: true);
                if (filename == null)
                    return;

                byte[] content = _DownloadFile(op, Invoker, Info.Path, ClientCtx.AppClientInfo.ClientId);
                if (content != null)
                {
                    File.WriteAllBytes(filename, content);
                    Process.Start(IOPath.GetDirectoryName(filename));
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        class DeleteAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation op = context.ServiceProvider.GetService<IUIOperation>(true);
                if (op.ShowQuery("确认要删除该文件吗？") != true)
                    return;

                string filePath = System.IO.Path.Combine(DirectoryInfoItem.Directory, Info.Path);
                Invoker.Tray.FsDeletePath(filePath, ClientCtx.AppClientInfo.ClientId);
                Refresh(context, serviceObject);
            }
        }
	}
}
