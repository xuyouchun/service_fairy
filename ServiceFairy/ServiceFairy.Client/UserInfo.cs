using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Client
{
    public class UserInfo
    {
        public UserInfo(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public Contact ToContact()
        {
            return new Contact() { UserName = UserName };
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}
