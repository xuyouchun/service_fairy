using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using Common.Contracts;

namespace TestWcfServer2
{
    class Program
    {
        private static readonly WcfService _ws = new WcfService();

        static void Main(string[] args)
        {
            using (WcfListener listener = _ws.CreateListener(ServiceAddress.Parse("127.0.0.1:100"), CommunicationType.Tcp))
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
            e.Connection.Received += new WcfClientDataReceivedEventHandler(Connection_Received);
        }

        static void Connection_Received(object sender, WcfClientDataReceivedEventArgs e)
        {
            e.ReplyData = new CommunicateData(Encoding.UTF8.GetBytes("jkfd;afjds;a"), DataFormat.Binary, ServiceStatusCode.OK);
        }
    }
}
