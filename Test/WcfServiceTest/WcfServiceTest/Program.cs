using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using System.Net;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using WcfServiceEntity;

namespace WcfServiceTest
{
    class Program
    {
        static readonly WcfService _wcfService = new WcfService();

        static void Main(string[] args)
        {
            WcfListener httpListener = _wcfService.CreateListener(new ServiceAddress("127.0.0.1", 80), CommunicationType.Http);
            httpListener.Connected += new EventHandler<WcfListenerConnectedEventArgs>(httpListener_Connected);
            httpListener.Start();

            /*
            WcfListener tcpListener = _wcfService.CreateListener(new ServiceAddress("127.0.0.1", 81), CommunicationType.Tcp, true);
            tcpListener.Connected += new EventHandler<WcfListenerConnectedEventArgs>(tcpListener_Connected);
            tcpListener.Start();*/

            Console.WriteLine("Running ...");
            Console.ReadLine();
        }

        static void httpListener_Connected(object sender, WcfListenerConnectedEventArgs e)
        {
            e.Connection.Received += new WcfClientDataReceivedEventHandler(Connection_Received);
        }

        static void tcpListener_Connected(object sender, WcfListenerConnectedEventArgs e)
        {
            e.Connection.Received += new WcfClientDataReceivedEventHandler(Connection_Received);
        }

        static void Connection_Received(object sender, WcfClientDataReceivedEventArgs e)
        {
            RequestEntity req = DataBufferParser.Deserialize<RequestEntity>(e.RequestData);
            ReplyEntity rsp = new ReplyEntity(123, "My GOD! hello !");
            CommunicateData data = DataBufferParser.Serialize(rsp, e.RequestData.DataFormat, e.RequestData.BufferType, ServiceStatusCode.DataError, 100, "NONONO!");
            
            if (e.SupportReply)
                e.ReplyData = data;
            else
                ((WcfConnection)sender).Send(e.Method, data, true);
        }
    }
}
