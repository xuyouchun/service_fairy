using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using System.Net;
using System.IO;
using Common.Contracts;
using Common.Contracts.Service;
using WcfServiceEntity;
using Common.Package.Service;

namespace WcfServiceTest.Client
{
    class Program
    {
        static readonly WcfService _wcfService = new WcfService();

        static void Main(string[] args)
        {
            /*
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:80/s");
            req.Method = "POST";
            using (Stream stream = req.GetRequestStream())
            {
                byte[] buffer = Encoding.UTF8.GetBytes("{ req: \"fdafdafda\" }");
                stream.Write(buffer, 0, buffer.Length);
            }

            using (HttpWebResponse rsp = (HttpWebResponse)req.GetResponse())
            {
                string s = new StreamReader(rsp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            }*/

            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();

            //using (WcfConnection wcfCon = _wcfService.Connect(new ServiceAddress("127.0.0.1", 81), CommunicationType.Tcp))
            using (WcfConnection wcfCon = _wcfService.Connect(new ServiceAddress("127.0.0.1", 80), CommunicationType.Http))
            {
                wcfCon.Open();

                wcfCon.Received += new WcfClientDataReceivedEventHandler(wcfCon_Received);

                RequestEntity reqEntity = new RequestEntity(100, "Hello!");
                CommunicateData req = DataBufferParser.Serialize(reqEntity, DataFormat.Json, BufferType.Bytes);
                CommunicateData rsp = wcfCon.Send("TestService/MyMethod", req, true);
                ReplyEntity rspEntity = DataBufferParser.Deserialize<ReplyEntity>(rsp);
                if (rsp != null)
                {
                    Console.WriteLine(Encoding.UTF8.GetString(rsp.ToBytes() ?? new byte[0]));
                }

                Console.ReadLine();
            }

            Console.ReadLine();
        }

        private static int _Index;

        static void wcfCon_Received(object sender, WcfClientDataReceivedEventArgs e)
        {
            WcfConnection con = (WcfConnection)sender;
            Console.WriteLine(Encoding.UTF8.GetString(e.RequestData.ToBytes() ?? new byte[0]));

            byte[] bytes = Encoding.UTF8.GetBytes((++_Index).ToString());
            CommunicateData data = new CommunicateData(bytes, DataFormat.Binary);
            if (e.SupportReply)
                e.ReplyData = data;

            //con.Send("Hello/MyMethod", Encoding.UTF8.GetBytes((++_Index).ToString()), DataFormat.Binary, false);
        }
    }
}
