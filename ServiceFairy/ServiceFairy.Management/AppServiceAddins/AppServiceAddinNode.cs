using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.UIObject;
using ServiceFairy.Management.AppClients;
using Common.Contracts.Service;
using ServiceFairy.Entities.Sys;
using Common.Utility;
using Common.Package.UIObject.Actions;
using Common.Package.UIObject;

namespace ServiceFairy.Management.AppServiceAddins
{
    /// <summary>
    /// 插件
    /// </summary>
    [SoInfo("插件"), UIObjectImage(ResourceNames.AppServiceAddin)]
    class AppServiceAddinNode : ServiceObjectKernelTreeNodeBase
    {
        public AppServiceAddinNode(AppClientContext clientCtx, ServiceDesc serviceDesc, AppServiceAddinInfoItem info)
        {
            _clientCtx = clientCtx;
            _servieDesc = serviceDesc;
            _info = info;
        }

        private readonly AppClientContext _clientCtx;
        private readonly ServiceDesc _servieDesc;
        private readonly AppServiceAddinInfoItem _info;

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc, AddinDesc, ServiceEndPoint>(GetType(), _servieDesc, _info.Info.AddinDesc, _info.Source);
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_info.Info.AddinDesc.ToString());
        }

        protected override IUIObject OnCreateUIObject()
        {
            string imgRes = (_info.AddinType == AppServiceAddinType.In) ? ResourceNames.AppServiceAddin_In : ResourceNames.AppServiceAddin_Out;

            return new ServiceUIObject(OnGetServiceObjectInfo(),
                SmUtility.CreateResourceImageLoader(imgRes)
            );
        }

        [SoInfo("标题"), ServiceObjectProperty]
        public string Title
        {
            get { return _info.Info.Title; }
        }

        [SoInfo("类型"), ServiceObjectProperty]
        public string AddinType
        {
            get { return _info.AddinType.GetDesc(); }
        }

        [SoInfo("来源"), ServiceObjectProperty]
        public string Source
        {
            get { return _info.AddinType == AppServiceAddinType.Out ? "(当前服务)" : _info.Source.ToString(_clientCtx.MgrCtx); }
        }

        [SoInfo("接入位置"), ServiceObjectProperty]
        public string Target
        {
            get { return _info.AddinType == AppServiceAddinType.In ? "(当前服务)" : _info.Target.ToString(); }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return _info.Info.Desc; }
        }
    }
}
