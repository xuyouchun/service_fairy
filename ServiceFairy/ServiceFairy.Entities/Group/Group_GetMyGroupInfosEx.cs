using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 获取我的群组信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetMyGroupInfosEx_Request : RequestEntity
    {
        /// <summary>
        /// 本地群组版本号
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupVersion), Summary("本地群组版本号")]
        public Dictionary<int, long> LocalVersions { get; set; }
    }

    /// <summary>
    /// 获取我的群组信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Group_GetMyGroupInfosEx_Reply : ReplyEntity
    {
        /// <summary>
        /// 发生变化的群组信息
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupInfos)]
        public GroupInfo[] Infos { get; set; }

        /// <summary>
        /// 已经不存在的群组ID
        /// </summary>
        [DataMember, Summary("已经不存在的群组ID")]
        public int[] NotExistsGroupIds { get; set; }
    }
}
