using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 获取联系人的状态－请求
    /// </summary>
    [Serializable, DataContract, Summary("获取联系人的状态－请求")]
    public class User_GetStatus_Request : RequestEntity
    {
        /// <summary>
        /// 用户
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds)]
        public int[] UserIds { get; set; }

        public override void Validate()
        {
            base.Validate();
            EntityValidate.ValidateNull(UserIds, "UserIds");
        }
    }

    /// <summary>
    /// 获取联系人的状态－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取联系人的状态－应答")]
    public class User_GetStatus_Reply : ReplyEntity
    {
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, SysFieldDoc(SysField.ContactStatus)]
        public UserStatus[] Status { get; set; }
    }
}
