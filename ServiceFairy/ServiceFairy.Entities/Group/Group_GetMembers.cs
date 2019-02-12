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
    public class Group_GetMembers_Request : RequestEntity
    {
        /// <summary>
        /// 群组Id
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 群组信息版本号
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupVersion), Remarks("如果服务器上的群组信息与该值相等，则不返回成员信息")]
        public long Version { get; set; }
    }

    /// <summary>
    /// 获取群组成员－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取群组成员－应答")]
    public class Group_GetMembers_Reply : ReplyEntity
    {
        /// <summary>
        /// 成员
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupMemberId)]
        public int[] MemberIds { get; set; }

        /// <summary>
        /// 群组信息版本号
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupVersion)]
        public long Version { get; set; }
    }
}
