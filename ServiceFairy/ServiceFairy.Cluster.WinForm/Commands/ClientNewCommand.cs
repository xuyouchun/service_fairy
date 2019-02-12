using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts;
using Common.WinForm;
using System.Windows.Forms;

namespace ServiceFairy.Cluster.WinForm.Commands
{
    /// <summary>
    /// 新建服务终端
    /// </summary>
    [Command("CLIENT_NEW")]
    class ClientNewCommand : CommandBase
    {
        public override void Execute(object context)
        {
            CommandContext ctx = (CommandContext)context;

            try
            {
                AppClient client = ctx.Context.ComponentManager.AppClientManager.CreateNew();
                TreeNode newNode = ctx.Operations.AddClient(client);
                ctx.MainForm.ClientsTreeNode.Expand();
                ctx.MainForm.ClientListTree.SelectedNode = newNode;
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ctx.MainForm, ex);
            }
        }
    }
}
