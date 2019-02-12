using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 获取用户信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserInfos_Request : RequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public Users Users { get; set; }

        /// <summary>
        /// 需要获取哪些用户信息
        /// </summary>
        [DataMember]
        public UserInfoMask Mask { get; set; }

        /// <summary>
        /// 是否刷新缓存
        /// </summary>
        [DataMember]
        public bool Refresh { get; set; }
    }

    /// <summary>
    /// 获取或缓存用户详细信息的遮罩
    /// </summary>
    [Flags]
    public enum UserInfoMask
    {
        /// <summary>
        /// 基础信息
        /// </summary>
        Basic = 0x01,

        /// <summary>
        /// 详细信息
        /// </summary>
        Detail = 0x02,

        /// <summary>
        /// 状态消息
        /// </summary>
        Status = 0x04,
    }

    /// <summary>
    /// 获取应用信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        [DataMember]
        public UserInfos[] Infos { get; set; }
    }

    /// <summary>
    /// 用户信息的组合
    /// </summary>
    [Serializable, DataContract]
    public class UserInfos
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 基础信息
        /// </summary>
        [DataMember]
        public UserBasicInfo BasicInfo { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        [DataMember]
        public UserDetailInfo DetailInfo { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        [DataMember]
        public UserStatusInfo StatusInfo { get; set; }
    }
}
