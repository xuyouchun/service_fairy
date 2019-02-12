using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 批量删除群组成员－请求
    /// </summary>
    [Serializable, DataContract, Summary("批量删除群组成员－请求")]
    public class Group_RemoveMembers_Request : RequestEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 成员
        /// </summary>
        [DataMember, SysFieldDoc(SysField.Users), Summary("需要删除的群组成员")]
        public string Members { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Members, "Members");
        }
    }
}
