using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Communication.Wcf;
using TestCommon;
using Common.Contracts.Service;
using Common.Contracts;
using ServiceFairy.Entities.User;
using Common.Utility;
using Common.Package;
using ServiceFairy.Entities.Group;
using System.Threading;

namespace TestInteractive
{
    class Program
    {
        /*
        const string _httpNavigation = "xuyc-pc:80";
        const string _socketNavigation = "xuyc-pc:9001";*/

        //const string _httpNavigation = "127.0.0.1:80";
        const string _httpNavigation = "117.79.130.229:80";
        const string _socketNavigation = "127.0.0.1:9010";

        /*
        const string _httpNavigation = "117.79.130.229:80";
        const string _socketNavigation = "117.79.130.229:9010";*/

        static void Main(string[] args)
        {
            // 获取代理列表
            CommunicationOption proxy = _GetProxy();

            const int userCount = 5;

            /*
            using (SystemInvoker invoker = SystemInvoker.FromProxy(proxy, DataFormat.Json))
            {
                // 创建多个用户
                for (int k = 1; k <= userCount; k++)
                {
                    invoker.User.Register("username_" + k, "", "9999");
                    Console.WriteLine("Registered:{0}", k);
                }
            }
            */

            UserConnectionManager userConMgr = new UserConnectionManager(proxy, DataFormat.Json, CommunicateType.Socket);

            // 多个用户登录，并将第1个用户加为好友
            for (int k = 1; k <= 4; k++)
            {
                string username = "username_" + k;
                UserConnection con = userConMgr.Connect(username, "111111");
                con.DataReceived += new ConnectionDataReceivedEventHandler(con_DataReceived);

                Console.WriteLine("“{0}”登录", username);

                /*
                con.Invoker.User.AddContact(new ContactItem { UserName = "username_5" });
                Console.WriteLine("AddContact:{0}", k);*/
            }

            while (true)
            {
                string message = Console.ReadLine();

                if (message.Length != 0)
                {
                    // 第一个用户状态改变，其它用户将收到通知
                    UserConnection con1 = userConMgr.GetByUserName("username_1");
                    //con1.Invoker.User.SetStatus(message);

                    con1.Invoker.User.SendMessageToFollowers(message);
                }
            }

            // 第一个用户修改信息，其它用户将收到通知
            //con1.Invoker.User.ModifyUserBasicInfo(new UserBasicInfo { FirstName = "New FirstName", LastName = "New LastName" });

            // 第一个用户断开连接，其它用户将收到通知

            /*
            for (int k = 0; k < 10000; k++)
            {
                con1 = userConMgr.GetByUserName("username_1");

                if (con1 != null)
                {
                    con1.Dispose();
                    Console.WriteLine("连接断开 - {0}", k);
                }
                else
                {
                    userConMgr.Connect("username_1", "111111");
                    Console.WriteLine("连接建立 - {0}", k);
                }

                Thread.Sleep(3000);
            }*/

            /*
            // 第一个用户向我的粉丝发送消息
            //con1.Invoker.User.SendMessage("My Message", "OK!", "fr:me");

            // 创建组
            int groupId = con1.Invoker.Group.CreateGroup("My Group");

            FcGroupInfo[] infos = con1.Invoker.Group.GetGroups();*/

            Console.ReadLine();
        }

        private static void con_DataReceived(object sender, ConnectionDataReceivedEventArgs e)
        {
            UserConnection con = (UserConnection)sender;
            byte[] buffer = e.RequestData.Data;
            string s = Encoding.UTF8.GetString(buffer);

            Console.WriteLine("{0}接收到消息: Method:{1}, Body:{2}", con.UserInfo.UserName, e.Method, s);
        }

        // 获取代理列表（演示如何以HTTP方式和SOCKET方式来获取代理列表）
        private static CommunicationOption _GetProxy()
        {
            // HTTP方式
            CommunicationOption proxy1 = Navigation.GetProxyByHttp(_httpNavigation, CommunicationType.Tcp, true);

            return proxy1;
        }
    }
}
