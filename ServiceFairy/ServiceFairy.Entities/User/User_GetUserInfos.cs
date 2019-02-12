using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 获取指定联系人的信息－请求
    /// </summary>
    [Serializable, DataContract, Summary("获取指定联系人的信息－请求")]
    public class User_GetUserInfos_Request : RequestEntity
    {
        /// <summary>
        /// 变化时间（仅获取在此之后发生变化的联系人信息）
        /// </summary>
        [DataMember, SysFieldDoc(SysField.Since)]
        public DateTime Since { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember, SysFieldDoc(SysField.Users)]
        public string Users { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Users, "Users");
        }
    }

    /// <summary>
    /// 获取指定联系人的信息－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取指定联系人的信息－应答")]
    public class User_GetUserInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserInfo)]
        public UserInfo[] Infos { get; set; }
    }
}
