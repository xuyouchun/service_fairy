using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common.WinForm;
using Common.Contracts;

namespace ServiceFairy.Cluster.WinForm.Commands
{
    [Command("CLIENT_STOP_N")]
    class ClientStopNCommand : CommandBase
    {
        public override void Execute(object context)
        {
            CommandContext ctx = (CommandContext)context;

            try
            {
                string countStr = InputDialog.Show(ctx.MainForm, "10", "输入停止个数");
                int count;
                if (!int.TryParse(countStr, out count) || count < 0)
                {
                    WinFormUtility.ShowError(ctx.MainForm, "输入错误");
                    return;
                }

                int stopCount = 0;
                foreach (AppClient client in ctx.Context.ComponentManager.AppClientManager.GetAllClients())
                {
                    if (stopCount >= count)
                        break;

                    if (client.Running)
                    {
                        client.Stop();
                        stopCount++;
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
