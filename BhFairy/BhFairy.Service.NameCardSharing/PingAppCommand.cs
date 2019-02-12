using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy;
using BhFairy.Entities;
using Common.Contracts.Service;

namespace BhFairy.Service.NameCardSharing
{
    [AppCommand("Ping")]
    class PingAppCommand : ACS<Service>.Func<NameCardSharing_Ping_Request, NameCardSharing_Ping_Reply>
    {
        protected override NameCardSharing_Ping_Reply OnExecute(AppCommandExecuteContext<Service> context, NameCardSharing_Ping_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            //srv.DefaultSystemInvoker.Cache.Get(
            srv.MyComponent.GetStr();

            return new NameCardSharing_Ping_Reply();
        }
    }
}
