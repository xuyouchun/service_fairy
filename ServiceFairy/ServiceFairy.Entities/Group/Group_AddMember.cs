using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 添加群组成员－请求
    /// </summary>
    [Serializable, DataContract]
    public class Group_AddMember_Request : RequestEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int Member { get; set; }
    }
}
