using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Security.Components
{
    class SystemUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 安全级别
        /// </summary>
        public SecurityLevel SecurityLevel { get; set; }
    }
}
