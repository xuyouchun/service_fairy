using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts;
using System.Windows.Forms;
using Common.WinForm;

namespace ServiceFairy.Cluster.WinForm.Commands
{
    /// <summary>
    /// 删除客户端
    /// </summary>
    [Command("CLIENT_DELETE")]
    class ClientDeleteCommand : CommandBase
    {
        public override void Execute(object context)
        {
            if (!IsAvaliable(context))
                return;

            CommandContext ctx = (CommandContext)context;
            TreeNode curNode = ctx.MainForm.ClientListTree.SelectedNode;
            AppClient client = curNode.Tag as AppClient;

            if (!WinFormUtility.ShowQuestion(ctx.MainForm, "确认要删除该服务终端吗？", false))
                return;

            try
            {
                ctx.Context.ComponentManager.AppClientManager.Remove(client);
                curNode.Remove();
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ctx.MainForm, ex);
            }
        }

        public override bool IsAvaliable(object context)
        {
            CommandContext ctx = (CommandContext)context;
            return ctx.Operations.IsClientNode(ctx.MainForm.ClientListTree.SelectedNode);
        }
    }
}
