using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;
using Common.Package;
using Common.Utility;
using ServiceFairy.DbEntities.Test;
using Common.Contracts;

namespace TestContactList
{
    class Program
    {
        static void Main(string[] args)
        {
            string navigation = "net.wtcp://127.0.0.1:8090";
            //string navigation = "net.tcp://xuyc-pc:8090";
            //string navigation = "net.wtcp://117.79.130.229:8090";
            //string navigation = "net.http://117.79.130.229:80";
            //string navigation = "net.wtcp://192.168.1.104:8090";

            using (SystemInvoker invoker = SystemInvoker.FromProxy(navigation))
            {
                // 注册多个用户
                for (int k = 2; k <= 10; k++)
                {
                    invoker.User.Register("username_" + k, "111111", "9999");
                }

                // 都将第一个用户加为关注
                for (int k = 2; k <= 10; k++)
                {
                    string sid = invoker.User.Login("username_" + k, "111111");
                    CommunicateCallingSettings settings = CommunicateCallingSettings.RequestReplyWithSid(sid);

                    invoker.User.AddContact(new[] {
                        new ContactItem() { UserName = "username_1", FirstName = "firstname_1", LastName = "lastname_1" },
                        new ContactItem() { UserName = "username_101", FirstName = "firstname_101", LastName = "lastname_101" }
                    }, settings);

                    Console.WriteLine("OK! " + k);
                }

                /*
                string sid = invoker.User.Login("username_1", "");
                CommunicateCallingSettings settings = CommunicateCallingSettings.RequestReplyWithSid(sid);
                invoker.User.SetStatus("My Status", settings);*/

                /*
                List<ContactItem> ctItems = new List<ContactItem>();
                for (int k = 110; k <= 120; k++)
                {
                    ctItems.Add(new ContactItem() { UserName = "username_" + k, FirstName = "firstname_" + k, LastName = "lastname_" + k });
                }

                invoker.User.AddContact(ctItems.ToArray(), settings);*/

                return;
            }
        }
    }
}
