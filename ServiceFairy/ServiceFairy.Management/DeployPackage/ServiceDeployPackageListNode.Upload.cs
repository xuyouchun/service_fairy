using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using ServiceFairy.WinForm;
using Common.Utility;
using ServiceFairy.Entities.Master;
using System.Windows.Forms;
using Common.Package.Service;
using Common.Contracts;
using System.IO;
using ServiceFairy.Entities;

namespace ServiceFairy.Management.DeployPackage
{
	partial class ServiceDeployPackageListNode
	{
        /// <summary>
        /// 上传服务安装包
        /// </summary>
        class UploadAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                UploadPackageDialog dlg = new UploadPackageDialog();
                dlg.SetInitValue(null, "", Settings.ServicePath);
                dlg.SetValidator(new _Validator());

                if (uiOp.ShowDialog(dlg) != DialogResult.OK)
                    return;

                byte[] content;
                ServiceDeployPackageInfo packageInfo = new ServiceDeployPackageInfo();
                dlg.GetPackage(packageInfo, out content);
                packageInfo.ServiceDesc = WinFormUtility.InvokeWithProgressWindow(uiOp,
                    () => _GetServiceDesc(content), "正在分析安装包 ...");

                if (string.IsNullOrWhiteSpace(packageInfo.Title))
                    packageInfo.Title = packageInfo.ServiceDesc.ToString() + " (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")";

                ServiceDeployPackageNode node = ServiceDeployPackageNode.StartNew(Kernel._mgrCtx, packageInfo);
                Kernel.Add(node);
                ServiceUtility.RefreshCurrentListView(context, serviceObject);

                ThreadUtility.StartNew(delegate {
                    try
                    {
                        Kernel._mgrCtx.Invoker.Master.UploadServiceDeployPackage(packageInfo, content);
                        node.UploadCompletedNotify(null);
                    }
                    catch (Exception ex)
                    {
                        uiOp.ShowError(ex);
                        node.UploadCompletedNotify(ex);
                    }
                });
            }

            private ServiceDesc _GetServiceDesc(byte[] content)
            {
                string tempPath = Path.Combine(Path.GetTempPath(), PathUtility.GetUtcTimePath());
                try
                {
                    StreamUtility.DecompressToDirectory(content, tempPath);
                    ServiceUIInfo uiInfo = SFUtility.LoadServiceUIInfo(tempPath);
                    if (uiInfo == null)
                        throw new Exception("无法从安装包中寻找服务信息");

                    return uiInfo.ServiceDesc;
                }
                catch (Exception ex)
                {
                    throw new Exception("从安装包中寻找服务信息时出错：" + ex.Message, ex);
                }
                finally
                {
                    Directory.Delete(tempPath, true);
                }
            }

            private string _BuildDeployPackageTitle()
            {
                ServiceDeployPackageInfo[] infos = Kernel._mgrCtx.ServiceDeployPackageInfos.GetAll();
                for (int k = 1; ; k++)
                {
                    string title = "Service_" + k.ToString().PadLeft(4, '0');
                    if (!infos.Any(info => string.Equals(info.Title, title, StringComparison.OrdinalIgnoreCase)))
                        return title;
                }
            }

            class _Validator : IValidator
            {
                public ValidateResult Validate(object value)
                {
                    object[] items = (object[])value;
                    if (!items.OfType<UploadPackageDialog.FileItem>().Any(
                        item => string.Compare(Path.GetFileName(item.FileName), "Main.dll", true) == 0))
                    {
                        return new ValidateResult(false, "服务的安装包必须具有Main.dll文件");
                    }

                    return ValidateResult.SucceedResult;
                }
            }
        }
	}
}
