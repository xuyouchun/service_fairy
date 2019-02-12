using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 批量获取群组信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetGroupInfos_Request : RequestEntity
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

    /// <summary>
    /// 批量获取群组信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetGroupInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 群组信息
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupInfos)]
        public GroupInfo[] Infos { get; set; }
    }
}
