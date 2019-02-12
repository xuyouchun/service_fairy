using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts;
using Common.WinForm;

namespace ServiceFairy.Cluster.WinForm.Commands
{
    [Command("CLIENT_STARTALL")]
    class ClientStartAllCommand : CommandBase
    {
        public override void Execute(object context)
        {
            CommandContext ctx = (CommandContext)context;

            try
            {
                foreach (AppClient client in ctx.Context.ComponentManager.AppClientManager.GetAllClients())
                {
                    client.Start();
                }
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ctx.MainForm, ex);
            }
        }
    }
}
