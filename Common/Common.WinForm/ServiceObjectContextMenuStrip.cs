using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Contracts.UIObject;
using Common.Utility;
using System.Drawing;
using Common.Package;

namespace Common.WinForm
{
    /// <summary>
    /// 服务对象的上下文菜单
    /// </summary>
    public class ServiceObjectContextMenuStrip : ContextMenuStrip
    {
        public ServiceObjectContextMenuStrip(IUIObjectExecuteContext executeContext)
        {
            Contract.Requires(executeContext != null);

            ExecuteContext = executeContext;
        }

        /// <summary>
        /// 执行上下文
        /// </summary>
        public IUIObjectExecuteContext ExecuteContext { get; private set; }

        /// <summary>
        /// 从ServiceObject创建上下文菜单
        /// </summary>
        /// <param name="executeContext"></param>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        public static ServiceObjectContextMenuStrip FromServiceObject(IUIObjectExecuteContext executeContext, IServiceObjectTreeNode treeNode)
        {
            Contract.Requires(executeContext != null && treeNode != null);

            IServiceObject so = treeNode.ServiceObject;
            return FromServiceObject(executeContext, so);
        }

        /// <summary>
        /// 从ServiceObject创建上下文菜单
        /// </summary>
        /// <param name="serviceObject"></param>
        /// <param name="uiFactory"></param>
        /// <returns></returns>
        public static ServiceObjectContextMenuStrip FromServiceObject(IUIObjectExecuteContext executeContext, IServiceObject serviceObject)
        {
            Contract.Requires(executeContext != null && serviceObject != null);

            ServiceObjectContextMenuStrip ms = new ServiceObjectContextMenuStrip(executeContext);
            ServiceObjectAction defaultAction = serviceObject.GetActions(ServiceObjectActionType.Default).FirstOrDefault();

            int index = 0;
            foreach (ServiceObjectGroupItem item in _GetServiceObjectGroupItems(serviceObject))
            {
                if (index++ > 0)
                    ms.Items.Add(new ToolStripSeparator());

                foreach (ServiceObjectAction action in item.Actions)
                {
                    ServiceObjectInfo info = action.Info;
                    IActionUIObject ui = action.GetUIObject() as IActionUIObject ?? new EmptyActionUIObject(info);
                    ServiceObjectActionToolStripMenuItem mi = new ServiceObjectActionToolStripMenuItem(serviceObject, ui, info.Title, ui.GetImage(16));
                    mi.Click += delegate { serviceObject.ExecuteActionWithErrorDialog(ui, executeContext); };

                    if (action == defaultAction)
                        mi.Font = new System.Drawing.Font(mi.Font, FontStyle.Bold);

                    ms.Items.Add(mi);
                }
            }

            return ms;
        }

        private static IEnumerable<ServiceObjectGroupItem> _GetServiceObjectGroupItems(IServiceObject serviceObject)
        {
            ServiceObjectAction[] allActions = serviceObject.GetActions(ServiceObjectActionType.All);
            HashSet<ServiceObjectAction> hasSelectedActions = new HashSet<ServiceObjectAction>();

            // 选择有分组信息的
            foreach (ServiceObjectGroup group in serviceObject.GetGroups().OrderBy(v => v.Info.Weight))
            {
                string groupName = group.Info.Name;
                ServiceObjectAction[] actions = allActions.Where(action => action.Group == groupName).ToArray();
                hasSelectedActions.AddRange(actions);

                if (actions.Length > 0)
                    yield return new ServiceObjectGroupItem() { Group = group, Actions = actions };
            }

            // 选择有分组但没有分组信息的
            foreach (var actionGroup in allActions.Where(
                action => !string.IsNullOrEmpty(action.Group) && !hasSelectedActions.Contains(action)).GroupBy(action => action.Group))
            {
                string groupName = actionGroup.Key;
                ServiceObjectAction[] actions = actionGroup.ToArray();
                hasSelectedActions.AddRange(actions);
                ServiceObjectGroup group = new ServiceObjectGroup(new ServiceObjectInfo("", groupName, groupName));

                yield return new ServiceObjectGroupItem() { Group = group, Actions = actions };
            }

            ServiceObjectAction[] restActions = allActions.Where(action => !hasSelectedActions.Contains(action)).ToArray();
            if (restActions.Length > 0)
                yield return new ServiceObjectGroupItem() { Group = null, Actions = restActions };
        }

        class ServiceObjectGroupItem
        {
            public ServiceObjectGroup Group { get; set; }

            public ServiceObjectAction[] Actions { get; set; }
        }

        /// <summary>
        /// 设置有效状态
        /// </summary>
        public void SetEnableState()
        {
            ToolStripSeparator lastSeparator = new ToolStripSeparator();
            for (int k = 0; k < Items.Count; k++)
            {
                ToolStripItem item = Items[k];
                ServiceObjectActionToolStripMenuItem menuItem;
                ToolStripSeparator separator;

                if ((menuItem = item as ServiceObjectActionToolStripMenuItem) != null)
                {
                    UIObjectExecutorState state = menuItem.UIObject.GetState(ExecuteContext, menuItem.ServiceObject);
                    menuItem.Enabled = state.Enable;
                    menuItem.Checked = state.Checked;
                    menuItem.Visible = state.Visiable;

                    if (state.Visiable)
                        lastSeparator = null;
                }
                else if ((separator = item as ToolStripSeparator) != null)
                {
                    separator.Visible = (lastSeparator == null);
                    lastSeparator = separator;
                }
            }

            if (lastSeparator != null)
                lastSeparator.Visible = false;
        }
    }

    #region Class ServiceObjectActionToolStripMenuItem ...

    public class ServiceObjectActionToolStripMenuItem : ToolStripMenuItem
    {
        public ServiceObjectActionToolStripMenuItem(IServiceObject serviceObject, IActionUIObject uiObject, string title, Image image)
            : base(title, image)
        {
            ServiceObject = serviceObject;
            UIObject = uiObject;
        }

        public IServiceObject ServiceObject { get; private set; }

        public IActionUIObject UIObject { get; private set; }
    }

    #endregion
}
