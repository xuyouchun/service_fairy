using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FakeMobileClient.Entities
{
    /// <summary>
    /// 用户注册－请求
    /// </summary>
    [Serializable, DataContract]
    public class User_Register_Request : UserRequestEntity
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

        /// <summary>
        /// 验证码
        /// </summary>
        [DataMember]
        public string VerifyCode { get; set; }
    }

    /// <summary>
    /// 用户注册－应答
    /// </summary>
    [Serializable, DataContract]
    public class User_Register_Reply : UserReplyEntity
    {

    }
}
