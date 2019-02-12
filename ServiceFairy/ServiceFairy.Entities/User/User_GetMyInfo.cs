using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 获取当前用户信息－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取当前用户信息－应答")]
    public class User_GetMyInfo_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserInfo)]
        public UserInfo Info { get; set; }
    }
}
