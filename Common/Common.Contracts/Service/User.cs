using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 用户的基础信息
    /// </summary>
    [Serializable, DataContract]
    public class UserBasicInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [DataMember]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; set; }

        /// <summary>
        /// 是否已经激活
        /// </summary>
        [DataMember]
        public bool Enable { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }

    /// <summary>
    /// 用户详细信息
    /// </summary>
    [Serializable, DataContract]
    public class UserDetailInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        public DateTime ChangedTime { get; set; }

        /// <summary>
        /// 信息项
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Items { get; set; }
    }

    /// <summary>
    /// 用户状态信息
    /// </summary>
    [Serializable, DataContract]
    public class UserStatusInfo
    {
        public UserStatusInfo()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="status">用户状态</param>
        /// <param name="statusUrl">用户状态对应的Url</param>
        /// <param name="online">是否在线</param>
        /// <param name="changedTime">状态变化时间</param>
        public UserStatusInfo(int userId, string status, string statusUrl, bool online, DateTime changedTime)
        {
            UserId = userId;
            Status = status;
            StatusUrl = statusUrl;
            Online = online;
            ChangedTime = changedTime;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// 用户状态Url
        /// </summary>
        [DataMember]
        public string StatusUrl { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        [DataMember]
        public bool Online { get; set; }

        /// <summary>
        /// 状态变化时间
        /// </summary>
        [DataMember]
        public DateTime ChangedTime { get; set; }
    }

    /// <summary>
    /// 用户的会话状态
    /// </summary>
    [Serializable, DataContract]
    public class UserSessionState
    {
        public UserSessionState()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <param name="securityLevel">安全级别</param>
        /// <param name="basicInfo">基础信息</param>
        public UserSessionState(Sid sid, SecurityLevel securityLevel, UserBasicInfo basicInfo)
        {
            Sid = sid;
            BasicInfo = basicInfo;
            SecurityLevel = securityLevel;
        }

        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; private set; }

        /// <summary>
        /// 安全级别
        /// </summary>
        [DataMember]
        public SecurityLevel SecurityLevel { get; set;  }

        /// <summary>
        /// 用户基础信息
        /// </summary>
        [DataMember]
        public UserBasicInfo BasicInfo { get; set; }
    }
}
