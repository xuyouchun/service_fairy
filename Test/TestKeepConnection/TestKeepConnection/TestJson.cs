using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.User;
using Common.Communication.Wcf.Bindings.SocketTransport;
using ServiceFairy.Entities.Sys;
using System.Threading;
using Common.Utility;

namespace TestKeepConnection
{
    static class TestJson
    {
        public static void Test()
        {
            string navigation = "net.http://127.0.0.1:80";
            //string navigation = "net.http://117.79.130.229:80";
            //string navigation = "net.http://xuyc-pc:80";

            // 获取代理地址
            //CommunicationOption proxy = Utility.GetProxyList(navigation, CommunicationType.Socket, true);

            //Console.WriteLine("Press any key to connect ...");

            UserConnectionManager ucMgr = UserConnectionManager.FromNavigation(navigation, proxyCommunicationType: CommunicationType.Socket);
            for (int k = 1; k <= 10; k++)
            {
                string username = "username_" + k;
                UserConnection uc = ucMgr.Connect(username, "111111");
                uc.DataReceived += new ConnectionDataReceivedEventHandler(uc_DataReceived);

                Console.WriteLine("“{0}”登录成功", username);
            }

            EventWaitHandle _event = new ManualResetEvent(false);
            //EventWaitHandle _event = new AutoResetEvent(false);
            Console.WriteLine("Press Enter to send data ...");

            UserConnection uc1 = ucMgr.GetByUserName("username_1");
            ThreadUtility.StartNew(delegate {

                // 改变状态
                for (int k = 1; ; k++)
                {
                    _event.WaitOne();
                    uc1.Invoker.User.SetStatus("status_" + k);
                }
            });

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(false);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    _event.Set();
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    _event.Reset();
                }
            }
        }

        private static void uc_DataReceived(object sender, ConnectionDataReceivedEventArgs e)
        {
            UserConnection uc = (UserConnection)sender;
            string s = Encoding.UTF8.GetString(e.RequestData.Data);
            Console.WriteLine("{0}接收到消息：{1}", uc.UserInfo.UserName, s);
        }

        /*
        class UserCon : IDisposable
        {
            public UserCon(UserSessionState uss, CommunicationOption proxy)
            {
                Uss = uss;
                Proxy = proxy;

                TcpClient = new TcpClient();
            }

            private static int _count;
            public static void ResetCount()
            {
                _count = 0;
            }

            public UserSessionState Uss { get; private set; }

            public CommunicationOption Proxy { get; private set; }

            public TcpClient TcpClient { get; private set; }

            public void Connect()
            {
                TcpClient.Connect(Proxy.Address.Address, Proxy.Address.Port);
                SocketMessageBufferAnalyzeQueue analyzeQueue = new SocketMessageBufferAnalyzeQueue(TcpClient);
                analyzeQueue.Received += new EventHandler<SocketMessageBufferAnalyzeQueueDataReceivedEventArgs>(analyzeQueue_Received);
                analyzeQueue.Closed += new EventHandler(analyzeQueue_Closed);

                if (!TcpClient.Connected)
                    throw new ApplicationException("连接未建成功");

                // 保持连接
                _Hello();
            }

            private void analyzeQueue_Closed(object sender, EventArgs e)
            {
                Console.WriteLine("连接断开：UserName:{0} Time:{1}", Uss.BasicInfo.UserName, DateTime.Now);
            }

            private void analyzeQueue_Received(object sender, SocketMessageBufferAnalyzeQueueDataReceivedEventArgs e)
            {
                string sss = Encoding.UTF8.GetString(e.Data.Array, e.Data.Offset, e.Data.Count);

                int index = Interlocked.Increment(ref _count);
                if (index % 100 == 0)
                    Console.WriteLine("收到第{0}个消息", index);

                Console.WriteLine("{0}: UserName:{1} Data:{2}", index, Uss.BasicInfo.UserName, sss);
            }

            // 保持连接
            private void _Hello()
            {
                Sys_Hello_Request req = new Sys_Hello_Request();

                TcpClient.SendSocketRequest(req, "System.Tray/Hello", Uss.Sid);
            }

            // 添加好友
            public void AddFriendship(UserSessionState uss)
            {
                User_AddFriendship_Request req = new User_AddFriendship_Request() { UserNames = new[] { uss.BasicInfo.UserName } };

                TcpClient.SendSocketRequest(req, "System.User/AddFriendship", Uss.Sid);
            }

            public void AddFriendship(IEnumerable<UserSessionState> usses)
            {
                User_AddFriendship_Request req = new User_AddFriendship_Request() { UserNames = usses.ToArray(us => us.BasicInfo.UserName) };

                TcpClient.SendSocketRequest(req, "System.User/AddFriendship", Uss.Sid);
            }

            // 改变状态
            public void ChangeState(string state)
            {
                User_SetState_Request req = new User_SetState_Request() { State = state };

                TcpClient.SendSocketRequest(req, "System.User/SetState", Uss.Sid);
            }

            public void Dispose()
            {
                Socket socket = TcpClient.Client;

                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                    socket.Close();
                }
            }
        }*/
    }
}
