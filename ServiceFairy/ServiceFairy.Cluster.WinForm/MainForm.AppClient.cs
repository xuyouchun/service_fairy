using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WinForm;
using System.Windows.Forms;
using ServiceFairy.Cluster.WinForm.UIActions;

namespace ServiceFairy.Cluster.WinForm
{
    partial class MainForm
    {
        private IUIAction _CreateClientUIAction(TreeNode node)
        {
            AppClient client = (AppClient)node.Tag;

            return new AppClientTreeNodeUIAction(_context, _panelViewer, client);
        }
    }
}
