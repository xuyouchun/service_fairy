using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Utility;
using System.Reflection;
using Common.Contracts.UIObject;
using Common.Package.UIObject;

namespace Common.Package.Service
{
    public static class ServiceUtility
    {
        public static ServiceObjectParameter GetServiceObjectParameter(ParameterInfo parameterInfo)
        {
            ServiceObjectInfoAttributeBase attr = parameterInfo.GetAttribute<ServiceObjectInfoAttributeBase>();

            string ns = parameterInfo.GetDeclaringType().FullName + "." + parameterInfo.Member.Name;
            ServiceObjectInfo info = (attr != null) ? attr.GetServiceObjectInfo(parameterInfo) : new ServiceObjectInfo(ns, parameterInfo.Name, parameterInfo.Name, "", "");
            return new ServiceObjectParameter(info, parameterInfo.ParameterType, GetParameterDefaultValue(parameterInfo));
        }

        public static object GetParameterDefaultValue(ParameterInfo parameterInfo)
        {
            object defValue = parameterInfo.DefaultValue;
            if (defValue == DBNull.Value)
                return parameterInfo.ParameterType.GetDefaultValue();

            return defValue;
        }

        public static IListViewWindow GetListViewWindow(IUIObjectExecuteContext context, IServiceObject serviceObject, bool showInNewWindow)
        {
            UIObjectExecuteContextHelper h = new UIObjectExecuteContextHelper(context);
            IListViewWindowManager lvWindowManger = h.GetService<IListViewWindowManager>(true);

            IListViewWindow w;
            if (!showInNewWindow)
            {
                w = lvWindowManger.GetCurrent(false);
                if (w != null)
                    return w;
            }

            w = lvWindowManger.CreateNew(true);
            return w;
        }

        /// <summary>
        /// 显示一个服务对象的子对象列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceObject"></param>
        /// <param name="showInNewWindow"></param>
        /// <param name="isRefresh"></param>
        public static void ShowSubList(IUIObjectExecuteContext context, IServiceObject serviceObject, bool showInNewWindow = false, bool isRefresh = false)
        {
            UIObjectExecuteContextHelper h = new UIObjectExecuteContextHelper(context);
            IServiceObjectProvider sop = h.GetService<IServiceObjectProvider>(true);
            IServiceObjectTreeNode treeNode = serviceObject.GetTreeNode();
            IListViewWindow lv = GetListViewWindow(context, serviceObject, showInNewWindow);

            if (treeNode == null)
            {
                lv.Clear();
                lv.SetWindowInfo(UIWindowInfo.FromServiceObject(serviceObject));
            }
            else
            {
                lv.ShowList(context, new ListViewData(treeNode), isRefresh);
            }

            lv.Activate();
        }

        /// <summary>
        /// 刷新列表数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceObject"></param>
        public static void RefreshCurrentListView(IUIObjectExecuteContext context, IServiceObject serviceObject = null)
        {
            IServiceObject curServiceObject;
            IListViewData d;
            IListViewWindow w = context.ServiceProvider.GetService<IListViewWindowManager>(true).GetCurrent();
            if (w == null || (d = w.GetCurrent()) == null || (curServiceObject = d.Owner) == null)
                return;

            if (serviceObject != null && curServiceObject != serviceObject)
                return;

            IRefreshableServiceObjectTreeNode refreshSupported = serviceObject.GetTreeNode() as IRefreshableServiceObjectTreeNode;
            if (refreshSupported != null)
                refreshSupported.Refresh(true);

            ShowSubList(context, serviceObject, isRefresh: true);
        }
    }
}
