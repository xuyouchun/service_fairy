using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 修改群组详细信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Group_ModifyGroupInfo_Request : RequestEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 群组详细信息
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupDetailInfo)]
        public Dictionary<string, string> Items { get; set; }
    }
}
