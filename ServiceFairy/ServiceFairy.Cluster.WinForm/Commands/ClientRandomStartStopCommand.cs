using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Package;
using ServiceFairy.Cluster.Components;
using Common.Contracts.Service;
using Common.Package.GlobalTimer;
using Common.Utility;

namespace ServiceFairy.Cluster.WinForm.Commands
{
    [Command("CLIENT_RANDOM_START_STOP")]
    class ClientRandomStartStopCommand : CommandBase
    {
        public ClientRandomStartStopCommand()
        {
            _handle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(5), new TaskFuncAdapter(_RandomStartOrStop), false, false);
        }

        private readonly IGlobalTimerTaskHandle _handle;
        private CommandContext _context;

        public override void Execute(object context)
        {
            _context = (CommandContext)context;

            _handle.Enable = !_handle.Enable;
        }

        private void _RandomStartOrStop()
        {
            if (_context == null)
                return;

            AppClient[] clients = _context.Context.ComponentManager.AppClientManager.GetAllClients();
            HashSet<AppClient> selectedClients = new HashSet<AppClient>();

            for (int k = 0; k < 5; k++)
            {
                AppClient client = clients.PackOneRandomOrDefault();
                if (client != null && !selectedClients.Contains(client))
                {
                    selectedClients.Add(client);
                    client.Running = !client.Running;
                }
            }
        }
    }
}
