using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;

namespace StreamTableViewer.Actions
{
    class RootNodeUIAction : NodeActionUIBase
    {
        public RootNodeUIAction(ViewerContext context, TreeViewUIActionManager treeviewUIActionManager, TreeNode node)
            : base(context, treeviewUIActionManager, node)
        {
            node.ContextMenuStrip = _CreateContextMenuStrip();
        }

        private ContextMenuStrip _CreateContextMenuStrip()
        {
            ContextMenuStrip ms = new ContextMenuStrip();
            ToolStripMenuItem tsRefleshAll = new ToolStripMenuItem("全部刷新", null, delegate { _RefreshAll(); });
            ms.Items.Add(tsRefleshAll);
            ms.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem tsCloseAll = new ToolStripMenuItem("全部关闭", null, delegate { _CloseAll(); });
            ms.Items.Add(tsCloseAll);

            return ms;
        }

        private void _CloseAll()
        {
            TreeViewUIActionManager.Raise(UIActionType.Close, Node, null, true, false);
        }

        private void _RefreshAll()
        {
            foreach (TreeNode node in TreeViewUIActionManager.FindNodes<StFileNodeUIAction>())
            {
                StFileNodeUIAction action = node.Tag as StFileNodeUIAction;
                action.Refresh();
            }
        }
    }
}
