using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common;
using ServiceFairy.Entities.UserCenter;
using Common.Utility;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable, DataContract]
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserName)]
        public string UserName { get; set; }

#warning 需要加上姓名
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 项
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserDetailInfo)]
        public string VCard { get; set; }

        /// <summary>
        /// 变化时间
        /// </summary>
        [DataMember, SysFieldDoc(SysField.ChangedTime)]
        public DateTime ChangedTime { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UserId=").Append(UserId).Append(", UserName=")
                .Append(UserName).Append(" ChangedTime=").Append(ChangedTime)
                .Append(" VCard=").Append(VCard);

            return sb.ToString();
        }
    }

    /// <summary>
    /// 用户ID与Name的组合
    /// </summary>
    [Serializable, DataContract]
    public class UserIdName
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserName)]
        public string UserName { get; set; }

        public override string ToString()
        {
            return string.Format("UserId = {0}, UserName = {1}", UserId, UserName);
        }

        public override int GetHashCode()
        {
            return UserId ^ (UserName == null ? 0 : UserName.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(UserIdName))
                return false;

            UserIdName obj2 = (UserIdName)obj;
            return UserId == obj2.UserId && UserName == obj2.UserName;
        }
    }

    /// <summary>
    /// 联系人状态
    /// </summary>
    [Serializable, DataContract, SysFieldDoc(SysField.ContactStatus)]
    public class UserStatus
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, SysFieldDoc(SysField.ContactStatusText)]
        public string Status { get; set; }

        /// <summary>
        /// 状态对应的Url
        /// </summary>
        [DataMember, SysFieldDoc(SysField.ContactStatusUrl)]
        public string StatusUrl { get; set; }

        /// <summary>
        /// 变化时间
        /// </summary>
        [DataMember, SysFieldDoc(SysField.ChangedTime)]
        public DateTime ChangedTime { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserOnline)]
        public bool Online { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("uid:").Append(UserId);
            sb.Append(" ").Append(Online ? "online" : "offline");
            if (!string.IsNullOrEmpty(Status))
                sb.Append(" ").Append(Status);

            if (!string.IsNullOrEmpty(StatusUrl))
                sb.Append(" ").Append(StatusUrl);

            sb.Append(" [").Append(ChangedTime).Append("]");
            return sb.ToString();
        }
    }
}
