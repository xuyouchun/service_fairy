using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Utility;
using ServiceFairy.Entities.User;

namespace TestParseUsers
{
    public class Pragma
    {
        public static void Main(string[] args)
        {
            //string navigation = "net.tcp://117.79.130.229:8090";
            string navigation = "net.tcp://127.0.0.1:8090";
            using (SystemInvoker invoker = SystemInvoker.FromNavigation(navigation))
            {
                //_RegisterUsers(invoker);
                //_AddContactList(navigation);

                int[] userIds = invoker.UserCenter.ParseUsers("fr:un:username_1;-un:username_2");

                //string[] usernames = CollectionUtility.GenerateArray(100, k => "username_" + (k + 1));
                //invoker.UserCenter.ParseUserNames(usernames);

                //invoker.UserCenter.ParseUserNames(
            }
        }

        private static void _RegisterUsers(SystemInvoker invoker)
        {
            for (int k = 1; k <= 100; k++)
            {
                invoker.User.Register("username_" + k, "111111", "9999", true);
                Console.WriteLine("{0} OK!", k);
            }
        }

        private static void _AddContactList(string navigation)
        {
            UserConnectionManager userConMgr = UserConnectionManager.FromNavigation(navigation);
            UserConnection userCon = userConMgr.Add("username_1", "111111");
            for (int k = 2; k <= 100; k++)
            {   
                userCon.Invoker.User.AddContact(new ContactItem {
                    FirstName = "firstname_" + k, LastName = "lastname_" + k, UserName = "username_" + k
                });

                Console.WriteLine(k);
            }
        }
    }
}
