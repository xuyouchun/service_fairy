using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Package.UIObject.Actions;
using ServiceFairy.Management.AppClients;
using Common.Utility;
using Common.Contracts;

namespace ServiceFairy.Management.AppCommands
{
    /// <summary>
    /// 接口
    /// </summary>
    [SoInfo("接口"), UIObjectImage(ResourceNames.AppCommand)]
    partial class AppCommandNode : ServiceObjectKernelTreeNodeBase
    {
        public AppCommandNode(AppClientContext clientCtx, ServiceDesc serviceDesc, AppCommandInfo cmdInfo)
        {
            _clientCtx = clientCtx;
            _serviceDesc = serviceDesc;
            _cmdInfo = cmdInfo;
        }

        private AppClientContext _clientCtx;
        private ServiceDesc _serviceDesc;
        private AppCommandInfo _cmdInfo;

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc, CommandDesc>(GetType(), _serviceDesc, _cmdInfo.CommandDesc);
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            string title = _cmdInfo.CommandDesc.Name;
            return ServiceObjectInfo.OfTitle(title);
        }

        protected override bool? HasChildren()
        {
            return false;
        }

        [SoInfo("文档与测试 ...")]
        [ServiceObjectAction(ServiceObjectActionType.Default)]
        [UIObjectAction(typeof(TestAction)), UIObjectImage("test"), ServiceObjectGroup("test")]
        public void Open() { }

        [SoInfo("版本号"), ServiceObjectProperty]
        public string Version
        {
            get { return _cmdInfo.CommandDesc.Version.ToString(); }
        }

        [SoInfo("标题"), ServiceObjectProperty]
        public string Title
        {
            get
            {
                string title = _cmdInfo.Title;
                if (_cmdInfo.Usable != Common.UsableType.Normal)
                {
                    string desc = !string.IsNullOrEmpty(_cmdInfo.UsableDesc) ? _cmdInfo.UsableDesc : _cmdInfo.Usable.GetDesc();
                    title += "(" + desc + ")";
                }

                return title;
            }
        }

        [SoInfo("类型"), ServiceObjectProperty]
        public string Type
        {
            get { return _cmdInfo.Category.GetDesc(); }
        }

        [SoInfo("输入/输出"), ServiceObjectProperty]
        public string InputOutput
        {
            get
            {
                return _GetParameterDesc(_cmdInfo.InputParameter) + "/" + _GetParameterDesc(_cmdInfo.OutputParameter);
            }
        }

        private string _GetParameterDesc(AppParameter p)
        {
            return (p == null || p.IsEmpty) ? "无" : "有";
        }

        [SoInfo("权限"), ServiceObjectProperty]
        public string SecurityLevel
        {
            get
            {
                return _cmdInfo.SecurityLevel.GetSecurityLevelDesc();
            }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return _cmdInfo.Desc; }
        }

        abstract class ActionBase : ActionBase<AppCommandNode> { }
    }
}
