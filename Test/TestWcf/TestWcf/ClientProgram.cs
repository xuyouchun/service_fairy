#define FLAG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using System.Net.Sockets;
using Common.Utility;
using Common.Package.TaskDispatcher;
using Common.Package;
using System.Threading;
using Common.Contracts;
using System.IO;

namespace TestWcf
{
    class ClientProgram
    {
        private static readonly WcfService _ws = new WcfService();

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();

            for (int k = 0; k < 1; k++)
            {
                WcfConnection con = _ws.Connect(ServiceAddress.Parse("127.0.0.1:99"), CommunicationType.Socket, true);

                con.Open();
                con.Received += new ConnectionDataReceivedEventHandler(con_Received);
                var ret = con.Send("aaaaaaa", new CommunicateData(Encoding.UTF8.GetBytes("ABCDE") , DataFormat.Unknown), settings: CommunicateCallingSettings.RequestReply);

                Console.WriteLine("Send OK!");
            }

            Console.ReadKey();
        }

        static void con_Received(object sender, ConnectionDataReceivedEventArgs e)
        {
            Console.WriteLine("Reply: " + Encoding.UTF8.GetString(e.RequestData.Data));
        }

        static byte[] _CreateBuffer(int size)
        {
            byte[] buffer = new byte[size];
            for (int k = 0; k < size; k++)
            {
                buffer[k] = (byte)k;
            }

            return buffer;
        }
    }
}
