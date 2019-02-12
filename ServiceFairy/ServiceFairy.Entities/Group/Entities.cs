using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 群组信息
    /// </summary>
    [Serializable, DataContract]
    public class GroupInfo
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupId)]
        public int GroupId { get; set; }

        /// <summary>
        /// 群组名称
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupName)]
        public string Name { get; set; }

        /// <summary>
        /// 群组创建者
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupCreator)]
        public int Creator { get; set; }

        /// <summary>
        /// 群组版本号
        /// </summary>
        [DataMember, SysFieldDoc(SysField.GroupVersion)]
        public long Version { get; set; }

#warning 添加Details
        [DataMember]
        public Dictionary<string, string> Details { get; set; }

        public static GroupInfo FromBasicInfo(GroupBasicInfo basicInfo)
        {
            if (basicInfo == null)
                return null;

            return new GroupInfo {
                GroupId = basicInfo.GroupId, Name = basicInfo.Name,
                Creator = basicInfo.Creator, Version = basicInfo.ChangedTime.Ticks
            };
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// 获取群组成员信息
    /// </summary>
    [Serializable, DataContract]
    public class GroupMemberInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }

        /// <summary>
        /// 加入时间
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }
    }
}
