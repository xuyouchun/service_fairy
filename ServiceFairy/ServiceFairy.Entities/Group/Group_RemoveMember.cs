using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 删除群组成员－请求
    /// </summary>
    [Serializable, DataContract]
    public class Group_RemoveMember_Request : RequestEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupMemberId)]
        public int MemberId { get; set; }
    }
}
