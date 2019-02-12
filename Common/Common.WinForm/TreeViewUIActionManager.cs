using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics.Contracts;

namespace Common.WinForm
{
    public class TreeViewUIActionManager : IDisposable
    {
        public TreeViewUIActionManager(TreeView tree, Func<TreeNode, IUIAction> actionCreator = null)
        {
            Contract.Requires(tree != null);

            Tree = tree;
            _actionCreator = actionCreator ?? ((node) => node.Tag as IUIAction);
            tree.NodeMouseClick += new TreeNodeMouseClickEventHandler(tree_NodeMouseClick);
            tree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tree_NodeMouseDoubleClick);
            tree.BeforeSelect += new TreeViewCancelEventHandler(tree_BeforeSelect);
            tree.AfterSelect += new TreeViewEventHandler(tree_AfterSelect);
            tree.BeforeExpand += new TreeViewCancelEventHandler(tree_BeforeExpand);
            tree.AfterExpand += new TreeViewEventHandler(tree_AfterExpand);
        }

        /// <summary>
        /// 树
        /// </summary>
        public TreeView Tree { get; set; }

        private readonly Dictionary<TreeNode, IUIAction> _actions = new Dictionary<TreeNode, IUIAction>();
        private readonly Func<TreeNode, IUIAction> _actionCreator;

        private IUIAction _GetAction(TreeNode node)
        {
            IUIAction action;
            if (!_actions.TryGetValue(node, out action))
                _actions.Add(node, action = _actionCreator(node));

            return action ?? EmptyAction.Instance;
        }

        #region Class EmptyAction ...

        class EmptyAction : IUIAction
        {
            public object Tag { get; set; }

            public void OnAction(UIActionType actionType, object sender, EventArgs e)
            {
                
            }

            public static readonly EmptyAction Instance = new EmptyAction();
        }

        #endregion

        void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _GetAction(e.Node).OnAction(UIActionType.TreeNodeMouseClick, sender, e);
        }

        void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _GetAction(e.Node).OnAction(UIActionType.TreeNodeMouseDoubleClick, sender, e);
        }

        void tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            _GetAction(e.Node).OnAction(UIActionType.TreeNodeBeforeSelect, sender, e);
        }

        void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _GetAction(e.Node).OnAction(UIActionType.TreeNodeAfterSelect, sender, e);
        }

        void tree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            _GetAction(e.Node).OnAction(UIActionType.TreeNodeAfterExpand, sender, e);
        }

        void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            _GetAction(e.Node).OnAction(UIActionType.TreeNodeBeforeExpand, sender, e);
        }

        /// <summary>
        /// 寻找指定类型Action的节点
        /// </summary>
        /// <typeparam name="TUIAction"></typeparam>
        /// <returns></returns>
        public TreeNode[] FindNodes<TUIAction>() where TUIAction : IUIAction
        {
            List<TreeNode> result = new List<TreeNode>();
            _ForEachNodes(Tree.Nodes, (node) => { if (node.Tag is TUIAction) result.Add(node); });

            return result.ToArray();
        }

        private void _ForEachNodes(TreeNodeCollection nodes, Action<TreeNode> action)
        {
            foreach (TreeNode node in nodes)
            {
                if (node == null)
                    continue;

                _ForEachNodes(node.Nodes, action);
                action(node);
            }
        }

        private void _ForEachNodes(TreeNode node, Action<TreeNode> action)
        {
            _ForEachNodes(node.Nodes, action);
            action(node);
        }

        public void Raise(UIActionType actionType, TreeNode node, EventArgs e = null, bool raiseAllSubNodes = false, bool includeCurrent = true)
        {
            Contract.Requires(node != null);
            e = e ?? EventArgs.Empty;

            if (raiseAllSubNodes)
            {
                _ForEachNodes(node.Nodes, (n) => {
                    _GetAction(n).OnAction(actionType, n, e);
                });
            }

            if (includeCurrent)
                _GetAction(node).OnAction(actionType, node, e);
        }

        public void RaiseCurrent(UIActionType actionType, EventArgs e = null, bool raiseAllSubNodes = false, bool includeCurrent = true)
        {
            TreeNode node = Tree.SelectedNode;
            if (node != null)
                Raise(actionType, node, e, raiseAllSubNodes, includeCurrent);
        }

        public void Dispose()
        {
            Tree.NodeMouseClick -= new TreeNodeMouseClickEventHandler(tree_NodeMouseClick);
            Tree.NodeMouseDoubleClick -= new TreeNodeMouseClickEventHandler(tree_NodeMouseDoubleClick);
            Tree.BeforeSelect -= new TreeViewCancelEventHandler(tree_BeforeSelect);
            Tree.AfterSelect -= new TreeViewEventHandler(tree_AfterSelect);
            Tree.BeforeExpand -= new TreeViewCancelEventHandler(tree_BeforeExpand);
            Tree.AfterExpand -= new TreeViewEventHandler(tree_AfterExpand);
        }
    }
}
