using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 获取我的所有群组版本号－应答
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetMyGroupVersions_Reply : ReplyEntity
    {
        /// <summary>
        /// 群组ID与版本号的键值对
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupVersion)]
        public Dictionary<int, long> Versions { get; set; }
    }
}
