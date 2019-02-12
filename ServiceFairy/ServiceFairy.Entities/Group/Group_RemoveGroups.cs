using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 批量删除群组－请求
    /// </summary>
    [Serializable, DataContract]
    public class Group_RemoveGroups_Request : RequestEntity
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
}
