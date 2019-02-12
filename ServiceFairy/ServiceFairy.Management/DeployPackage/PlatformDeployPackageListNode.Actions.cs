using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using ServiceFairy.WinForm;
using Common.Package.UIObject;
using Common.Utility;
using Common.Contracts;
using Common.Framework.Management.DockingWindow;
using Common.WinForm;
using System.Windows.Forms;
using ServiceFairy.Entities.Master;
using Common.Package.Service;
using ServiceFairy.Entities;

namespace ServiceFairy.Management.DeployPackage
{
	partial class PlatformDeployPackageListNode
	{
        abstract class UploadActionBase : ActionBase
        {
            protected void Upload(IUIObjectExecuteContext context, IServiceObject serviceObject, PlatformDeployPackageInfo info, byte[] content)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);

                PlatformDeployPackageNode node = PlatformDeployPackageNode.StartNew(Kernel._mgrCtx, info);
                Kernel.Add(node);
                ServiceUtility.RefreshCurrentListView(context, serviceObject);

                ThreadUtility.StartNew(delegate {
                    try
                    {
                        Kernel._mgrCtx.Invoker.Master.UploadPlatformDeployPackage(info, content);
                        node.UploadCompletedNotify(null);
                    }
                    catch (Exception ex)
                    {
                        uiOp.ShowError(ex);
                        node.UploadCompletedNotify(ex);
                    }
                });
            }

            protected string BuildDeployPackageTitle()
            {
                return "Platform [" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "]";

                /*
                PlatformDeployPackageInfo[] infos = Kernel._mgrCtx.PlatformDeployPackageInfos.GetAll();
                for (int k = 1; ; k++)
                {
                    string title = "Platform_" + k.ToString().PadLeft(4, '0');
                    if (!infos.Any(info => string.Equals(info.Title, title, StringComparison.OrdinalIgnoreCase)))
                        return title;
                }*/
            }
        }

        /// <summary>
        /// 上传安装包
        /// </summary>
        class UploadAction : UploadActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                UploadPackageDialog dlg = new UploadPackageDialog();
                dlg.SetInitValue(null, BuildDeployPackageTitle(), Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

                if (uiOp.ShowDialog(dlg) != DialogResult.OK)
                    return;

                byte[] content;
                PlatformDeployPackageInfo packageInfo = new PlatformDeployPackageInfo();
                dlg.GetPackage(packageInfo, out content);
                if (string.IsNullOrWhiteSpace(packageInfo.Title))
                    packageInfo.Title = BuildDeployPackageTitle();

                Upload(context, serviceObject, packageInfo, content);
            }
        }

        /// <summary>
        /// 同步平台安装包
        /// </summary>
        class SyncDeployAction : UploadActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                SyncPlatformDeployPackageDialog dlg = new SyncPlatformDeployPackageDialog();
                dlg.AssemblyPath = Settings.PlatformSyncPath;
                dlg.IgnoreFiles = Settings.PlatformSyncPathFilters;

                if (uiOp.ShowDialog(dlg) == DialogResult.OK)
                {
                    byte[] content = dlg.BuildDeployPackage();
                    PlatformDeployPackageInfo info = new PlatformDeployPackageInfo() {
                        Format = DeployPackageFormat.GZipCompress, Id = Guid.NewGuid(), LastUpdate = DateTime.UtcNow,
                        Size = content.Length, Title = BuildDeployPackageTitle(), UploadTime = DateTime.UtcNow,
                    };

                    Upload(context, serviceObject, info, content);
                }
            }
        }
	}
}
