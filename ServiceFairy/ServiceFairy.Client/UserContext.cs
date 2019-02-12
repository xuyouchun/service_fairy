using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Client
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public class UserContext
    {
        public UserContext(string username, string password, UserConnectionManager userConMgr)
        {
            UserName = username;
            Password = password;

            _userConMgr = userConMgr;
            _userCon = new Lazy<UserConnection>(_LoadUserCon);
        }

        private readonly UserConnectionManager _userConMgr;
        private readonly Lazy<UserConnection> _userCon;

        private UserConnection _LoadUserCon()
        {
            return _userConMgr.Connect(UserName, Password);
        }

        /// <summary>
        /// 用户连接
        /// </summary>
        public UserConnection UserCon
        {
            get { return _userCon.Value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
    }
}
