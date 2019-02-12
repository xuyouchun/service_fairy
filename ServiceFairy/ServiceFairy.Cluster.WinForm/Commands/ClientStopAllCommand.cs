using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Package;
using Common.WinForm;

namespace ServiceFairy.Cluster.WinForm.Commands
{
    /// <summary>
    /// 新建服务终端
    /// </summary>
    [Command("CLIENT_STOPALL")]
    class ClientStopAllCommand : CommandBase
    {
        public override void Execute(object context)
        {
            CommandContext ctx = (CommandContext)context;

            try
            {
                foreach (AppClient client in ctx.Context.ComponentManager.AppClientManager.GetAllClients())
                {
                    client.Stop();
                }
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ctx.MainForm, ex);
            }
        }
    }
}
