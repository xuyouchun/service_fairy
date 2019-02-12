using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using Common.Utility;
using System.IO;
using System.Diagnostics;

namespace ServiceFairy.Management.DeployPackage
{
    partial class PlatformDeployPackageNode
    {
        /// <summary>
        /// 删除
        /// </summary>
        class DeleteAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);

                if (uiOp.ShowQuery("确认要删除该平台安装包吗？") != true)
                    return;

                Kernel._mgrCtx.Invoker.Master.DeletePlatformDeployPackages(new[] { Kernel._packageInfo.Id });
                Kernel.CurrentStatus = ServiceStatus.Deleting;
                Kernel.StartRefresh(delegate {
                    if (Kernel._mgrCtx.PlatformDeployPackageInfos.Exists(Kernel._packageInfo.Id))
                        return ServiceObjectRefreshResult.ContinueAndRefresh;

                    return ServiceObjectRefreshResult.Dispose;
                });
            }
        }

        /// <summary>
        /// 下载安装包
        /// </summary>
        class DownloadAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                string file = uiOp.SelectFile("下载安装包", PackageInfo.Title + ".deployPackage", "安装包文件(*.deployPackage)|*.deployPackage|所有文件(*.*)|*.*", saveModel: true);
                if (file == null)
                    return;

                var package = uiOp.InvokeWithProgressWindow(() => Invoker.Master.DownloadServiceDeployPackage(PackageInfo.Id),
                    "正在下载安装包 ...", TimeSpan.FromMilliseconds(500), true
                );

                if (package != null && package.Content != null)
                {
                    File.WriteAllBytes(file, package.Content);
                    Process.Start(Path.GetDirectoryName(file));
                }
            }
        }

        /// <summary>
        /// 下载并解压安装包
        /// </summary>
        class ExtratAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                string dir = uiOp.SelectFolder("下载并解压安装包");
                if (dir == null)
                    return;

                var package = uiOp.InvokeWithProgressWindow(() => Invoker.Master.DownloadServiceDeployPackage(PackageInfo.Id),
                    "正在下载安装包 ...", TimeSpan.FromMilliseconds(500), true
                );

                dir = Path.Combine(dir, PackageInfo.Title);
                if (package != null && package.Content != null)
                {
                    StreamUtility.DecompressToDirectory(package.Content, dir);
                    Process.Start(dir);
                }
            }
        }
    }
}
