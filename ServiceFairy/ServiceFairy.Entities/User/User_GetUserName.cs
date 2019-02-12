using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 根据UserId获取UserName－请求
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserName_Request : RequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 根据UserId获取UserName－应答
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserName_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserName)]
        public string UserName { get; set; }
    }
}
