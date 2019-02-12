using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;
using System.Diagnostics;
using Common.Package.TaskDispatcher;
using Common.Package;
using System.Threading;
using ServiceFairy.Entities.Navigation;
using Common.Communication.Wcf;
using Common.Contracts;

namespace TestCacheService
{
    class Program
    {
        static void Main(string[] args)
        {
            string navigation = "net.tcp://127.0.0.1:8090";
            //string navigation = "net.tcp://xuyc-pc:8090";
            //string navigation = "net.tcp://117.79.130.229:8090";

            SystemInvoker invoker0 = SystemInvoker.FromProxy(navigation, DataFormat.Json);
            string s = invoker0.Navigation.GetProxy(CommunicationType.Http, false);

            _sw.Start();
            for (int k = 0; k < 1; k++)
            {
                SocketConnection sc = new SocketConnection(
                    SystemInvoker.GetProxyList(CommunicationOption.Parse(navigation), CommunicationType.Socket, CommunicationDirection.Bidirectional).First()
                );

                SystemInvoker invoker = SystemInvoker.FromCommunicate(sc);
                //SystemInvoker invoker = SystemInvoker.FromNavigation(navigation);
                int start = k * 1000000;
                ThreadPool.QueueUserWorkItem(delegate { _Run(invoker, start); });
            }

            _sw.AutoTrace();
        }

        private static CountStopwatch _sw = new CountStopwatch();

        private static void _Run(SystemInvoker invoker, int start)
        {
            for (int k = 0; ; k++)
            {
                try
                {
                    invoker.Cache.Set("key_" + k, "value_" + k, TimeSpan.FromSeconds(1), settings: CommunicateCallingSettings.OneWay);
                    invoker.Cache.Get<string>("key_" + k);

                    _sw.Increment(2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
