using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Framework.TrayPlatform;
using System.Diagnostics;
using Common.Package.TaskDispatcher;
using ServiceFairy.Entities.Navigation;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using System.Threading;

namespace TestLog
{
    class Program
    {
        static Program()
        {
            LogManager.RegisterFileLogWriter(Settings.LogPath);
        }

        private static readonly Stopwatch _sw = new Stopwatch();
        private static int _index = 0;

        static void Main(string[] args)
        {
            // 通过导航获取代理
            //string navigation = Settings.Navigation;
            string navigation = "xuyc-pc:80";
            CommunicationOption[] proxyList = _GetProxyList(navigation);

            if (proxyList.Length == 0)
            {
                Console.WriteLine("无可用代理");
                return;
            }

            string proxy = proxyList[0].Address.ToString();

            _sw.Start();

            for (int k = 0; k < Settings.ThreadCount; k++)
            {
                ThreadUtility.StartNew(_RunningFunc, proxy);
            }

            Console.ReadLine();
        }

        private static void _RunningFunc(string proxy)
        {
            while (true)
            {
                int index = Interlocked.Increment(ref _index);
                string logMsg = "LOG_" + index;

                try
                {
                    string rspJson = Utility.SendRequest(proxy, "{ \"method\": \"Application.Test/WriteLog\", \"body\": { \"Log\": \"" + logMsg + "\" } }");
                    if (index > 0 && index % 100 == 0)
                        Utility.WriteLog(string.Format("Counts:{0}, Milliseconds:{1}, PreSeconds:{2}  OK!", index, _sw.ElapsedMilliseconds, index / _sw.Elapsed.TotalSeconds));
                }
                catch (Exception ex)
                {
                    Utility.WriteLog(ex);
                }
            }
        }

        // 获取代理列表
        private static CommunicationOption[] _GetProxyList(string navAddress)
        {
            Navigation_GetProxyList_Request req = new Navigation_GetProxyList_Request() {
                CommunicationType = CommunicationType.Http, MaxCount = 10
            };

            ReplyWrapper<Navigation_GetProxyList_Reply> reply
                = Utility.SendRequest<Navigation_GetProxyList_Request, Navigation_GetProxyList_Reply>(navAddress, req, "System.Navigation/GetProxyList", "获取代理列表");

            return reply.Body.CommunicationOptions;
        }
    }
}
