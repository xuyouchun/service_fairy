using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServiceFairy.Cluster.WinForm
{
    /// <summary>
    /// 集群的一些操作
    /// </summary>
    class ClusterOperations
    {
        public ClusterOperations(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        private readonly MainForm _mainForm;

        /// <summary>
        /// 添加一个终端
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public TreeNode AddClient(AppClient client)
        {
            TreeNode clientTreeNode = _mainForm.ClientsTreeNode.Nodes.Add(client.Title);
            clientTreeNode.Tag = client;

            return clientTreeNode;
        }

        /// <summary>
        /// 是否为服务终端的节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsClientNode(TreeNode node)
        {
            return node != null && node.Tag is AppClient;
        }
    }
}
