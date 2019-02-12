using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 获取群组信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetGroupInfo_Request : RequestEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember]
        public int GroupId { get; set; }
    }

    /// <summary>
    /// 获取群组信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetGroupInfo_Reply : ReplyEntity
    {
        /// <summary>
        /// 群组信息
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupInfos)]
        public GroupInfo Info { get; set; }
    }
}
