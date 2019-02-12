using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Management.AppClients;
using Common.Contracts.Service;
using Common.Utility;
using Common.Contracts.UIObject;
using System.IO;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Management.AppServices
{
    /// <summary>
    /// 文件节点
    /// </summary>
    [SoInfo("文件"), UIObjectImage(ResourceNames.AppServiceFile)]
    partial class AppServiceFileNode : ServiceObjectKernelTreeNodeBase
    {
        public AppServiceFileNode(AppClientContext clientCtx, ServiceDesc serviceDesc, AppFileInfo fileInfo)
        {
            _clientCtx = clientCtx;
            _serviceDesc = serviceDesc;
            _fileInfo = fileInfo;
        }

        private readonly AppClientContext _clientCtx;
        private readonly ServiceDesc _serviceDesc;
        private readonly AppFileInfo _fileInfo;

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_fileInfo.FileName);
        }

        //protected override Common.Contracts.UIObject.IUIObject OnCreateUIObject()
        //{
        //    return new ServiceUIObject(OnGetServiceObjectInfo(),
        //        (IUIObjectImageLoader)UIObjectImageLoader.FromFileSystemIcon(_fileInfo.File)
        //            ?? SmUtility.CreateResourceImageLoader(ResourceNames.AppServiceFile)
        //    );
        //}

        protected override object GetKey()
        {
            return new Tuple<Type, string>(GetType(), _fileInfo.FileName.ToLower());
        }

        [SoInfo("打开"), ServiceObjectAction(ServiceObjectActionType.Default)]
        [UIObjectAction(typeof(OpenFileAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        [SoInfo("下载 ..."), ServiceObjectAction(ServiceObjectActionType.AttachDefault)]
        [UIObjectAction(typeof(DownloadFileAction)), UIObjectImage("download"), ServiceObjectGroup("download")]
        public void Download()
        {

        }

        [SoInfo("大小"), ServiceObjectProperty]
        public string Size
        {
            get { return StringUtility.GetSizeString(_fileInfo.Size); }
        }

        [SoInfo("修改时间"), ServiceObjectProperty]
        public string LastUpdate
        {
            get { return _fileInfo.LastModifyTime.GetLocalTimeString(); }
        }

        [SoInfo("类别"), ServiceObjectProperty]
        public string Category
        {
            get { return SmUtility.GetFileCategory(_fileInfo.FileName); }
        }

        abstract class ActionBase : ActionBase<AppServiceFileNode>
        {
            public AppClientContext ClientCtx { get { return Kernel._clientCtx; } }
            public SfManagementContext MgrCtx { get { return ClientCtx.MgrCtx; } }
            public SystemInvoker Invoker { get { return MgrCtx.Invoker; } }
            public AppFileInfo FileInfo { get { return Kernel._fileInfo; } }
            public ServiceDesc ServiceDesc { get { return Kernel._serviceDesc; } }
        };
    }
}
