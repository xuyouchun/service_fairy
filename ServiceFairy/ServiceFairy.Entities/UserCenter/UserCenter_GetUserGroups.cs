using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 获取用户所属的组－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserGroups_Request : RequestEntity
    {
        /// <summary>
        /// 用户
        /// </summary>
        [DataMember]
        public Users Users { get; set; }

        /// <summary>
        /// 是否刷新缓存
        /// </summary>
        [DataMember]
        public bool Refresh { get; set; }
    }

    /// <summary>
    /// 获取用户所属的组－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserGroups_Reply : ReplyEntity
    {
        /// <summary>
        /// 所属组信息
        /// </summary>
        [DataMember]
        public UserGroupItem[] Items { get; set; }
    }

    /// <summary>
    /// 用户所属的组
    /// </summary>
    [Serializable, DataContract]
    public class UserGroupItem
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 组ID
        /// </summary>
        [DataMember]
        public int[] GroupIds { get; set; }
    }
}
