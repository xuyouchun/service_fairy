using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 获取用户信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserInfo_Request : RequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 获取用户信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserInfo_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserInfo)]
        public UserInfo Info { get; set; }
    }
}
