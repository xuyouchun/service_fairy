using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using System.Net.Sockets;
using Common.Utility;
using System.IO;

namespace TestLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "{ \"method\":\"Core.Tray/Sys_Hello\" , \"messageId\":\"CFB6B51A-8094-4FD2-8D83-BD70A7A59FB7\"}";
            //string s = "{ \"method\":\"System.User/Login\", \"body\":{  }, \"messageId\":\"CFB6B51A-8094-4FD2-8D83-BD70A7A59FB7\" }";

            TcpClient tc = new TcpClient();
            tc.Connect("117.79.130.229", 9010);
            //tc.Connect("127.0.0.1", 9010);

            NetworkStream ns = tc.GetStream();
            ns.Write(BufferUtility.ToBytes(s.Length));
            ns.Write(Encoding.UTF8.GetBytes(s));

            MemoryStream ms = new MemoryStream();
            int len;
            byte[] buffer = new byte[1024];
            while ((len = tc.Client.Receive(buffer)) > 0)
            {
                ms.Write(buffer, 0, len);

                if (ms.Length > 4)
                {
                    string sss = Encoding.UTF8.GetString(ms.GetBuffer(), 4, (int)(ms.Length - 4));
                }
            }

            Console.ReadLine();
        }
    }
}
