using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Package;
using Common.WinForm;

namespace ServiceFairy.Cluster.WinForm.Commands
{
    [Command("CLIENT_START_N")]
    class ClientStartNCommand : CommandBase
    {
        public override void Execute(object context)
        {
            CommandContext ctx = (CommandContext)context;

            try
            {
                string countStr = InputDialog.Show(ctx.MainForm, "10", "输入启动个数");
                int count;
                if (!int.TryParse(countStr, out count) || count<0)
                {
                    WinFormUtility.ShowError(ctx.MainForm, "输入错误");
                    return;
                }

                int startCount = 0;
                foreach (AppClient client in ctx.Context.ComponentManager.AppClientManager.GetAllClients())
                {
                    if (startCount >= count)
                        break;

                    if (!client.Running)
                    {
                        startCount++;
                        client.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ctx.MainForm, ex);
            }
        }
    }
}
