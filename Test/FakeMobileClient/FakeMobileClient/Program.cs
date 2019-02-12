using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using FakeMobileClient.Entities;

namespace FakeMobileClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CommunicationOption[] proxyList = _GetProxyList("xuyc-pc:8099");

            if (proxyList.Length == 0)
            {
                Console.WriteLine("无可用代理");
                return;
            }

            string address = proxyList[0].Address.ToString();
            string sessionId = Guid.NewGuid().ToString();

            ReplyWrapper<User_Register_Reply> reply = Utility.SendRequest<User_Register_Request, User_Register_Reply>(address,
                new User_Register_Request() { UserName = "myname", Password = "mypassword", SeccionID = sessionId, VerifyCode = "1234" }, "System.User/Register");

            return;
        }

        private static CommunicationOption[] _GetProxyList(string navAddress)
        {
            Navigation_GetProxyList_Request req = new Navigation_GetProxyList_Request() {
                CommunicationType = CommunicationType.Http, MaxCount= 10
            };

            ReplyWrapper<Navigation_GetProxyList_Reply> reply
                = Utility.SendRequest<Navigation_GetProxyList_Request, Navigation_GetProxyList_Reply>(navAddress, req, "System.Navigation/GetProxyList");

            return reply.Body.CommunicationOptions;
        }


    }
}
