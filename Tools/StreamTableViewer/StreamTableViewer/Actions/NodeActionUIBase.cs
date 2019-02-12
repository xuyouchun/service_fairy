using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WinForm;
using System.Windows.Forms;
using Common.Contracts;
using Common.Utility;

namespace StreamTableViewer.Actions
{
    public class NodeActionUIBase : UIActionBase
    {
        public NodeActionUIBase(ViewerContext context, TreeViewUIActionManager treeViewUIActionManager, TreeNode node)
        {
            TreeViewUIActionManager = treeViewUIActionManager;
            Context = context;
            Node = node;
        }

        public TreeNode Node { get; private set; }

        public ViewerContext Context { get; private set; }

        public TreeViewUIActionManager TreeViewUIActionManager { get; private set; }

        public override void OnAction(UIActionType actionType, object sender, EventArgs e)
        {
            switch (actionType)
            {
                case UIActionType.TreeNodeAfterSelect:
                    _ShowProperty();
                    break;
            }
        }

        private void _ShowProperty()
        {
            Context.ServiceProvider.GetService<IPropertyViewer>(true).ShowProperty(OnGetPropertyObject());
        }

        protected virtual object OnGetPropertyObject()
        {
            return null;
        }
    }
}
