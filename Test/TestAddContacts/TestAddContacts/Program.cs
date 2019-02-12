using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.Entities.User;
using System.Threading;
using ServiceFairy.Entities.Group;
using Common.Package;

namespace TestAddContacts
{
    class Program
    {
        private const string _navigation = "net.wtcp://117.79.130.229:8090";
        //private const string _navigation = "net.wtcp://127.0.0.1:8090";
        private static readonly UserConnectionManager _userConMgr = UserConnectionManager.FromNavigation(_navigation);

        static void Main(string[] args)
        {
            SystemInvoker invoker = SystemInvoker.FromNavigation(_navigation);
            string[] phoneNumbers = new[] { "+86 13717674044"/*,  "+86 13717674044", "+86 13717674045", "+86 13717674046", "+86 13717674047",
                "+86 13717674048", "+86 13717674049", "+86 13717674050", "+86 13717674051"*/ };

            UserConnection con = null;
            foreach (string phoneNumber in phoneNumbers)
            {
                ServiceResult<int> sr = invoker.User.MobileActiveSr(phoneNumber, "9999");
                if (sr.Succeed)
                {
                    UserConnection userCon = _userConMgr.Create(sr.Sid);
                    userCon.Invoker.Group.GroupChanged += new ServiceClientReceiveEventHandler<Group_GroupChanged_Message>(Group_GroupChanged);
                    userCon.Invoker.Group.Message += new ServiceClientReceiveEventHandler<Group_Message_Message>(Group_Message);
                    userCon.Invoker.User.StatusChanged += new ServiceClientReceiveEventHandler<User_StatusChanged_Message>(User_StatusChanged);
                    userCon.Connect();
                    UserInfo info = userCon.Invoker.User.GetMyInfo();
                    Console.WriteLine("{0} 已连接", phoneNumber);

                    if (con == null)
                        con = userCon;
                    else
                        userCon.Invoker.User.AddContacts(new[] { "+86 13717674043", "0" });
                }
            }

            var g = con.Invoker.Group;
            var u = con.Invoker.User;

            while(true)
            {
                Console.Write("请输入状态：");
                string status = Console.ReadLine();

                try
                {
                    u.SetStatus(status);
                    Console.WriteLine("OK!");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void User_StatusChanged(object sender, ServiceClientReceiveEventArgs<User_StatusChanged_Message> e)
        {
            UserConnection con = (UserConnection)sender;
            Console.WriteLine("{0} 收到消息: {1}", con.UserInfo.UserName, e.Method + " " + e.Entity.UserIds.JoinBy(","));
        }

        static void Group_Message(object sender, ServiceClientReceiveEventArgs<Group_Message_Message> e)
        {
            UserConnection con = (UserConnection)sender;
            Console.WriteLine("{0} 收到消息: {1}", con.UserInfo.UserName, e.Method + " " + e.Entity.Content);
        }

        static void Group_GroupChanged(object sender, ServiceClientReceiveEventArgs<Group_GroupChanged_Message> e)
        {
            UserConnection con = (UserConnection)sender;
            Console.WriteLine("{0} 收到消息: {1}", con.UserInfo.UserName, e.Method + " " + e.Entity.ChangedType + " " + e.Entity.Name);
        }
    }
}
