using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using Common.Contracts;
using System.Threading;
using Common.Utility;

namespace TestWcfServer
{
    class ServerProgram
    {
        private static readonly WcfService _ws = new WcfService();

        static void Main(string[] args)
        {
            //Console.WriteLine("Server 1: Press any key to start ...");
            //Console.ReadLine();

            using (WcfListener listener = _ws.CreateListener(ServiceAddress.Parse("127.0.0.1:99"), CommunicationType.Socket, true))
            {
                listener.Connected += new EventHandler<WcfListenerConnectedEventArgs>(listener_Connected);
                listener.Disconnected += new EventHandler<WcfListenerDisconnectedEventArgs>(listener_Disconnected);
                listener.Start();

                Console.WriteLine("Server Started ...");
                Console.ReadLine();
            }
        }

        static void listener_Disconnected(object sender, WcfListenerDisconnectedEventArgs e)
        {
            Console.WriteLine("Client Disconnected!");
        }

        static void listener_Connected(object sender, WcfListenerConnectedEventArgs e)
        {
            Console.WriteLine("New Client Connected!");
            e.Connections.ForEach(con => con.Received += new ConnectionDataReceivedEventHandler(Connection_Received));
        }

        static void Connection_Received(object sender, ConnectionDataReceivedEventArgs e)
        {
            byte[] bytes = e.RequestData.Data;
            string sss = Encoding.UTF8.GetString(bytes);
            //Console.WriteLine("Data Received: " + sss);

            e.ReplyData = new CommunicateData(Encoding.UTF8.GetBytes(sss + " OK!"), DataFormat.Binary);

            for (int k = 0; k < 100; k++)
            {
                CommunicateData data = new CommunicateData(Encoding.UTF8.GetBytes(sss + "_" + k + " OK!"), DataFormat.Binary, ServiceStatusCode.OK);
                CommunicateData replyData = e.Connection.Call(null, "MyMethod", data, CommunicateCallingSettings.OneWay);

                //break;
                Thread.Sleep(1000);
            }

            return;
        }
    }
}
