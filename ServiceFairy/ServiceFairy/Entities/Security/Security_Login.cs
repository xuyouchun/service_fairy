using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Security
{
    /// <summary>
    /// 登录－请求
    /// </summary>
    [Serializable, DataContract]
    public class Security_Login_Request : RequestEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string Password { get; set; }
    }

    /// <summary>
    /// 登录－应答
    /// </summary>
    [Serializable, DataContract]
    public class Security_Login_Reply : ReplyEntity
    {
        /// <summary>
        /// 安全码信息
        /// </summary>
        [DataMember]
        public SidInfo SidInfo { get; set; }
    }
}
