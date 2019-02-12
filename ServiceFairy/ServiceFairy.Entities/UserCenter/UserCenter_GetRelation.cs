using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 获取用户关系－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetRelation_Request : RequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }

        /// <summary>
        /// 指定要获取哪些关系
        /// </summary>
        [DataMember]
        public GetRelationMask Mask { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserIds, "UserIds");
        }
    }

    [Flags]
    public enum GetRelationMask
    {
        /// <summary>
        /// 粉丝
        /// </summary>
        Follower = 0x01,

        /// <summary>
        /// 关注
        /// </summary>
        Following = 0x02,

        All = -1,
    }

    /// <summary>
    /// 用户关系
    /// </summary>
    [Serializable, DataContract]
    public class UserRelation
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 粉丝列表
        /// </summary>
        [DataMember]
        public int[] Followers { get; set; }

        /// <summary>
        /// 关注列表
        /// </summary>
        [DataMember]
        public int[] Followings { get; set; }
    }

    /// <summary>
    /// 获取用户关系－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetRelation_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户关系
        /// </summary>
        [DataMember]
        public UserRelation[] Relations { get; set; }
    }
}
