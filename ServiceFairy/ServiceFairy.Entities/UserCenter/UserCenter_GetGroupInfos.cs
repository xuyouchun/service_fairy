using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using ServiceFairy.Entities.Group;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 获取群组信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetGroupInfos_Request : RequestEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember]
        public int[] GroupIds { get; set; }

        /// <summary>
        /// 获取群组信息的遮罩
        /// </summary>
        [DataMember]
        public GroupInfoMask Mask { get; set; }

        /// <summary>
        /// 是否刷新缓存
        /// </summary>
        [DataMember]
        public bool Refresh { get; set; }
    }

    /// <summary>
    /// 获取群组信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetGroupInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 信息
        /// </summary>
        [DataMember]
        public GroupInfos[] Infos { get; set; }
    }

    /// <summary>
    /// 群组信息项
    /// </summary>
    [Serializable, DataContract]
    public class GroupInfos
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember]
        public int GroupId { get; set; }

        /// <summary>
        /// 群组基础信息
        /// </summary>
        [DataMember]
        public GroupBasicInfo BasicInfo { get; set; }

        /// <summary>
        /// 群组详细信息
        /// </summary>
        [DataMember]
        public GroupDetailInfo DetailInfo { get; set; }

        /// <summary>
        /// 群组成员信息
        /// </summary>
        [DataMember]
        public GroupMemberInfo[] MemberInfos { get; set; }
    }

    /// <summary>
    /// 获取或缓存群组详细信息的遮罩
    /// </summary>
    public enum GroupInfoMask
    {
        /// <summary>
        /// 群组基础信息
        /// </summary>
        Basic = 0x01,

        /// <summary>
        /// 群组详细信息
        /// </summary>
        Detail = 0x02,

        /// <summary>
        /// 群组成员信息
        /// </summary>
        Member = 0x04,
    }
}
