using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 创建群组－请求
    /// </summary>
    [Serializable, DataContract, Summary("创建群组－请求")]
    public class Group_CreateGroup_Request : RequestEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupName)]
        public string Name { get; set; }

#warning 添加一个群组详细信息
        [DataMember]
        public Dictionary<string, string> Details { get; set; }
    }

    /// <summary>
    /// 创建群组－应答
    /// </summary>
    [Serializable, DataContract, Summary("创建群组－应答")]
    public class Group_CreateGroup_Reply : ReplyEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }
    }
}
