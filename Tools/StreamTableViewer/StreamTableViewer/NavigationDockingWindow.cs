using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm.Docking;
using System.Diagnostics.Contracts;
using Common.WinForm;
using StreamTableViewer.Actions;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace StreamTableViewer
{
    public partial class NavigationDockingWindow : DockContentEx
    {
        public NavigationDockingWindow(ViewerContext viewerContext)
        {
            InitializeComponent();
            _viewerContext = viewerContext;

            _uiActionManager = new TreeViewUIActionManager(_tree);
            _rootNode = _tree.Nodes["StreamTable"];
            _rootNode.Tag = new RootNodeUIAction(_viewerContext, _uiActionManager, _rootNode);

            _viewerContext.MainWindow.DockPanel.ActiveDocumentChanged += new EventHandler(_dockPanel_ActiveDocumentChanged);
        }

        private readonly TreeNode _rootNode;
        private readonly ViewerContext _viewerContext;
        private readonly TreeViewUIActionManager _uiActionManager;

        public void ShowFiles(string[] files)
        {
            Contract.Requires(files != null);

            TreeNode[] nodes = _uiActionManager.FindNodes<StFileNodeUIAction>();

            foreach (TreeNode node in nodes)
            {
                if (files.Any(file => string.Compare((node.Tag as StFileNodeUIAction).File, file, true) == 0))
                {
                    _uiActionManager.Raise(UIActionType.Close, node, raiseAllSubNodes: true);
                }
            }

            for (int k = 0; k < files.Length; k++)
            {
                string file = files[k];
                string name = Path.GetFileName(file);
                TreeNode node = new TreeNode(name);
                _rootNode.Nodes.Add(node);
                node.Tag = new StFileNodeUIAction(_viewerContext, _uiActionManager, node, file);
                node.StateImageIndex = 1;

                if (k == 0)
                    node.Expand();
            }

            _rootNode.Expand();
        }

        private void _tree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        public void CloseCurrent()
        {
            _uiActionManager.RaiseCurrent(UIActionType.Close);
        }

        private void _dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            DockPanel dp = (DockPanel)sender;
            Form form = dp.ActiveDocument as Form;
            NodeActionUIBase ui;
            if (form != null && (ui = form.Tag as NodeActionUIBase) != null)
            {
                ui.Node.Select();
            }
        }
    }
}
