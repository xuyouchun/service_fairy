using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 批量添加群组成员－请求
    /// </summary>
    [Serializable, DataContract, Summary("批量添加群组成员－请求")]
    public class Group_AddMembers_Request : RequestEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 群组成员
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupMembers)]
        public string Members { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Members, "Members");
        }
    }
}
