using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 获取群组成员－请求
    /// </summary>
    [Serializable, DataContract, Summary("获取群组成员－请求")]
    public class Group_GetMembersEx_Request : RequestEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupIds)]
        public int[] GroupIds { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(GroupIds, "GroupIds");
        }
    }

    /// <summary>
    /// 群组成员
    /// </summary>
    [Serializable, DataContract, SysFieldDoc(SysField.GroupMemberItem)]
    public class GroupMemberItem
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 成员
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupMembers)]
        public int[] Members { get; set; }
    }

    /// <summary>
    /// 获取群组成员－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取群组成员－应答")]
    public class Group_GetMembersEx_Reply : ReplyEntity
    {
        /// <summary>
        /// 成员项
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupMemberItem)]
        public GroupMemberItem[] Items { get; set; }
    }
}
