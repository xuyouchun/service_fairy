using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 获取我的所有群组ID－应答
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetMyGroups_Reply : ReplyEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupIds)]
        public int[] GroupIds { get; set; }
    }
}
