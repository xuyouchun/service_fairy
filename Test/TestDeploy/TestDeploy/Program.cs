using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;
using Common.Package.TaskDispatcher;
using System.Threading;
using Common.Utility;
using ServiceFairy;
using System.Runtime.CompilerServices;
using System.IO;

namespace TestDeploy
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SystemInvoker invoker = new SystemInvoker("117.79.130.229:8090"))
            {
                Console.Beep();
                ServiceDesc[] sds = invoker.Master.GetAllServices();
                ClientDesc[] cds = invoker.Master.GetAllClientDesc();

                ClientDesc cd1 = cds.FirstOrDefault(cd => cd.Title == "应用服务器(1)");

                foreach (ServiceDesc sd in sds.Where(sd0 => sd0.Name.StartsWith("System.") && sd0.Name != "System.Proxy"))
                {
                    ThreadUtility.StartNew(delegate {
                        _Execute(invoker, sd, cd1);
                    });
                }

                Console.ReadLine();
            }
        }

        private static void _Execute(SystemInvoker invoker, ServiceDesc sd, ClientDesc cd)
        {
            for (int k = 0; ; k++)
            {
                _StartOrStop(invoker, sd, cd.ClientID, k % 2 == 0);
            }
        }

        private static void _StartOrStop(SystemInvoker invoker, ServiceDesc sd, Guid clientId, bool isStart)
        {
            try
            {
                if (isStart)
                    invoker.Master.StartService(clientId, sd);
                else
                    invoker.Master.StopService(clientId, sd);

                int times = 0;
                while (invoker.Master.GetClientInfo(clientId).ServiceInfos.Any(si => si.ServiceDesc == sd) != isStart)
                {
                    if (times++ > 30)
                    {
                        _Write(sd + ":Timeout");
                        Console.Beep();
                    }

                    Thread.Sleep(1000);
                }

                _Write(sd + ":" + (isStart ? "Started" : "Stopped"));
            }
            catch (Exception ex)
            {
                _Write(sd + ":" + ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void _Write(string s)
        {
            string s0 = string.Format("[{0}] {1}", DateTime.Now, s);
            File.AppendAllText(@"d:\testdeploy.txt", s0 + "\r\n");

            Console.WriteLine(s0);
        }
    }
}
