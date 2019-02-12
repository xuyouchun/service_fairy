using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 获取关注列表－请求
    /// </summary>
    [Serializable, DataContract, Summary("获取关注列表－请求")]
    public class User_GetFollowings_Request : RequestEntity
    {
        /// <summary>
        /// 注册时间
        /// </summary>
        [DataMember, SysFieldDoc(SysField.Since), Summary("起始注册时间")]
        public DateTime Since { get; set; }
    }

    /// <summary>
    /// 获取关注列表－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取关注列表－应答")]
    public class User_GetFollowings_Reply : ReplyEntity
    {
        /// <summary>
        /// 所关注的用户的ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds)]
        public int[] UserIds { get; set; }
    }
}
