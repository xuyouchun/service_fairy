using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 组的会话状态
    /// </summary>
    [Serializable, DataContract]
    public class GroupSessionState
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember, Summary("群组ID")]
        public int GroupId { get; set; }

        /// <summary>
        /// 组的创建者
        /// </summary>
        [DataMember, Summary("创建者")]
        public int Creator { get; set; }

        /// <summary>
        /// 组成员
        /// </summary>
        [DataMember, Summary("群组成员")]
        public int[] Members { get; set; }
    }

    /// <summary>
    /// 组的基础信息
    /// </summary>
    [Serializable, DataContract]
    public class GroupBasicInfo
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, Summary("群组ID"), Remarks("为一个整型数字")]
        public int GroupId { get; set; }

        /// <summary>
        /// 群组名称
        /// </summary>
        [DataMember, Summary("群组名称")]
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember, Summary("群组创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 群组信息或成员变化时间
        /// </summary>
        [DataMember, Summary("群组信息或成员变化时间")]
        public DateTime ChangedTime { get; set; }

#warning 需要返回版本号
        public long Version { get; set; }

        /// <summary>
        /// 创建者ID
        /// </summary>
        [DataMember, Summary("群组创建者")]
        public int Creator { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember, Summary("是否启用")]
        public bool Enable { get; set; }
    }

    /// <summary>
    /// 群组详细信息
    /// </summary>
    [Serializable, DataContract]
    public class GroupDetailInfo
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, Summary("群组ID")]
        public int GroupId { get; set; }

        /// <summary>
        /// 信息项
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Items { get; set; }
    }
}
