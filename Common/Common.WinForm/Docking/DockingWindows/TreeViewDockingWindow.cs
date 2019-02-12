using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Contracts;

namespace Common.WinForm.Docking.DockingWindows
{
    public partial class TreeViewDockingWindow : DockContentEx, ITreeViewWindow
    {
        public TreeViewDockingWindow()
        {
            InitializeComponent();
        }

        private void TreeViewDockingWindow_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 以指定的ServiceObject为根显示服务树
        /// </summary>
        /// <param name="executeContext"></param>
        /// <param name="root"></param>
        /// <param name="expandDeep"></param>
        public void Show(IUIObjectExecuteContext executeContext, IServiceObjectTreeNode root, int expandDeep)
        {
            Contract.Requires(root != null);

            try
            {
                _tree.BeginUpdate();
                _Clear();

                _Show(executeContext, _tree.Nodes, root);
                _tree.Expand(expandDeep);
            }
            finally
            {
                _tree.EndUpdate();
            }
        }

        private void _Clear()
        {
            _tree.Nodes.Clear();
            _imageList.Images.Clear();
        }

        #region Class NodeTag ...

        class NodeTag
        {
            public IServiceObjectTreeNode SoTreeNode;
            public bool Loaded;
            public IUIObjectExecuteContext ExecuteContext;
        }

        #endregion

        private void _Show(IUIObjectExecuteContext executeContext, TreeNodeCollection nodes, IServiceObjectTreeNode soTreeNode)
        {
            ServiceObjectInfo soInfo = soTreeNode.ServiceObject.Info;
            string text = StringUtility.GetFirstNotNullOrWhiteSpaceString(soInfo.Title, soInfo.Name);

            TreeNode node = new TreeNode(text);
            IUIObject uiObject = soTreeNode.ServiceObject.GetUIObject();
            if (uiObject != null)
            {
                node.ImageIndex = node.SelectedImageIndex = _AddImage(uiObject.GetImageOrTransparent(_imageList.ImageSize));
                ServiceObjectContextMenuStrip ms = ServiceObjectContextMenuStrip.FromServiceObject(executeContext, soTreeNode.ServiceObject);
                node.ContextMenuStrip = ms;
            }

            node.ToolTipText = soInfo.Desc;
            nodes.Add(node);
            node.Tag = new NodeTag() { Loaded = false, SoTreeNode = soTreeNode, ExecuteContext = executeContext };
            if (soTreeNode.HasChildren != false)
                node.Nodes.Add("正在加载 ...");
        }

        private int _AddImage(Image image)
        {
            int index = _imageList.Images.FindIndex(image);
            if (index >= 0)
                return index;

            _imageList.Images.Add(image);
            return _imageList.Images.Count - 1;
        }

        private void _tree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            NodeTag tag = e.Node.Tag as NodeTag;
            if (tag == null || tag.Loaded)
                return;

            tag.Loaded = true;
            e.Node.Nodes.Clear();

            try
            {
                foreach (IServiceObjectTreeNode soNode in tag.SoTreeNode)
                {
                    _Show(tag.ExecuteContext, e.Node.Nodes, soNode);
                }
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(this, ex);
            }
        }

        public void UpdateChildren(IUIObjectExecuteContext executeContext, IServiceObject serviceObject)
        {
            throw new NotImplementedException();
        }

        private void _tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            NodeTag tag;
            if (e.Node != null && (tag = e.Node.Tag as NodeTag) != null)
            {
                tag.SoTreeNode.ServiceObject.ExecuteActionWithErrorDialog(tag.ExecuteContext,
                    Control.ModifierKeys == Keys.Shift ? ServiceObjectActionType.OpenInNewWindow : ServiceObjectActionType.Open);
            }
        }

        private void _tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeTag tag;
            if (e.Node != null && (tag = e.Node.Tag as NodeTag) != null)
            {
                tag.SoTreeNode.ServiceObject.ExecuteActionWithErrorDialog(tag.ExecuteContext,
                    Control.ModifierKeys == Keys.Shift ? ServiceObjectActionType.AttachDefault : ServiceObjectActionType.Default);
            }
        }

        private void _tree_DoubleClick(object sender, EventArgs e)
        {

        }

        public void SetCurrent(IServiceObject serviceObject)
        {
            
        }
    }
}
