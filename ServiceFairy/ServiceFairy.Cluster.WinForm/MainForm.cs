using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using System.IO;
using System.Configuration;
using ServiceFairy.Cluster.Components;
using Common.Contracts.Service;
using ServiceFairy.WinForm;
using ServiceFairy.Cluster.WinForm.Commands;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Cluster.WinForm
{
    public partial class MainForm : XForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private ClusterContext _context;
        private MenuStripCommandManager _menuStripCommandManager;
        private TreeViewUIActionManager _treeViewUIActionManager;
        private ClusterOperations _clusterOperations;

        private void MainForm_Load(object sender, EventArgs e)
        {
            CommunicationDialog dlg = new CommunicationDialog(true, true);
            dlg.CommunicationOption = Settings.DefaultCommunication;
            dlg.Text = "中心服务器地址";
            if (dlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
            {
                Close();
                return;
            }

            try
            {
                _context = new ClusterContext(
                    dlg.CommunicationOption,
                    Settings.ServicePath,
                    Settings.RunningPath,
                    Settings.DeployPackagePath
                );

                _context.ComponentManager = new ClusterComponentManager(_context);
                _menuStripCommandManager = new MenuStripCommandManager(_msMenu, typeof(MainForm).Assembly,
                    new CommandContext(this, _context, _clusterOperations = new ClusterOperations(this)));
                _treeViewUIActionManager = new TreeViewUIActionManager(_xtvClientList, _CreateClientUIAction);
                _context.ComponentManager.Start();
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(this, ex);
            }
        }

        private TreeNode _clientsNode;

        private void MainForm_Shown(object sender, EventArgs e)
        {
            _clientsNode = _xtvClientList.Nodes.Add("ClientList", "服务终端");
            foreach (AppClient client in _context.ComponentManager.AppClientManager.GetAllClients())
            {
                _clusterOperations.AddClient(client);
            }

            _clientsNode.ExpandAll();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_context != null)
                _context.Dispose();

            if (_menuStripCommandManager != null)
                _menuStripCommandManager.Dispose();
        }

        /// <summary>
        /// 服务终端列表树
        /// </summary>
        public XTreeView ClientListTree
        {
            get { return _xtvClientList; }
        }

        /// <summary>
        /// 服务终端列表节点
        /// </summary>
        public TreeNode ClientsTreeNode
        {
            get { return _clientsNode; }
        }
    }
}
