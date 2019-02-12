using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Contracts.UIObject;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Service.UI
{
    [SoInfo("接口列表", "该服务的所有网络通信接口"), UIObjectImage("CommandList")]
    partial class AppCommandListKernelNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppCommandListKernelNode(ServiceDesc serviceDesc)
        {
            _serviceDesc = serviceDesc;
        }

        private readonly ServiceDesc _serviceDesc;

        [SoInfo("显示接口列表"), ServiceObjectAction(ServiceObjectActionType.Default), UIObjectAction(typeof(OpenAction)), ServiceObjectGroup("open")]
        public void ShowCommandList()
        {
            
        }

        [SoInfo("在新窗口中显示接口列表"), ServiceObjectAction(ServiceObjectActionType.AttachDefault), UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void ShowCommandListInNewWindow()
        {

        }

        [SoInfo("刷新"), ServiceObjectAction, UIObjectAction(typeof(RefreshAction))]
        public void Refresh()
        {

        }

        [SoInfo("数量", Weight = 10), ServiceObjectProperty]
        public int Count
        {
            get { return 100; }
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return new IServiceObjectTreeNode[0];
        }

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc>(GetType(), _serviceDesc);
        }
    }
}
