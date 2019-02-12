using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 获取我的所有群组信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetMyGroupInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 群组信息
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupInfos)]
        public GroupInfo[] Infos { get; set; }
    }
}
