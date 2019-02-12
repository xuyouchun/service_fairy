using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using System.Net;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Contracts;
using ServiceFairy;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.Cache;

namespace ServiceFairyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SystemInvoker invoker = new SystemInvoker("127.0.0.1:8090"))
            {
                AppCommandInfo[] infos = invoker.Sys.GetAppCommandInfo(
                    CommunicateCallingSettings.OfTarget(Guid.Parse("10000000-0000-0000-0000-000000000000"), ServiceDesc.Parse("System.Cache"))
                );

                return;
            }
        }
    }
}
