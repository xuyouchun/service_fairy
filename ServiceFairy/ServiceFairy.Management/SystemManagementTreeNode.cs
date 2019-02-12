using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Management.AppClients;
using Common.Package.UIObject.Actions;
using ServiceFairy.Management.Balancing;
using ServiceFairy.Management.DeployPackage;
using ServiceFairy.Management.FaultDiagnosis;

namespace ServiceFairy.Management
{
    [SoInfo("系统管理", Name = "SystemManagement"), UIObjectImage(ResourceNames.SystemManagement)]
    class SystemManagementTreeNode : ServiceObjectKernelTreeNodeBase
    {
        public SystemManagementTreeNode(SfManagementContext mgrCtx)
        {
            _mgrCtx = mgrCtx;
        }

        private readonly SfManagementContext _mgrCtx;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return new IServiceObjectTreeNode[] {
                new AppClientListNode(_mgrCtx),     // 终端列表
                //new BalancingNode(_mgrCtx),         // 负载均衡
                new DeployManageNode(_mgrCtx),      // 部署管理
                //new DataBaseManagerNode(_mgrCtx),   // 数据库管理
                //new FaultDiagnosisNode(_mgrCtx),    // 故障诊断
            };
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open")]
        public void Open()
        {

        }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction))]
        public void OpenInNewWindow()
        {

        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return ""; }
        }

        protected override object GetKey()
        {
            return GetType();
        }
    }
}
