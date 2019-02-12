using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 获取指定群组的版本号－请求
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetGroupVersions_Request : RequestEntity
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
    /// 获取指定群组的版本号－应答
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetGroupVersions_Reply : ReplyEntity
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupVersion)]
        public long[] Versions { get; set; }
    }
}
