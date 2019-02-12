using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 添加联系人－请求
    /// </summary>
    [Serializable, DataContract, Summary("添加联系人－请求")]
    public class User_AddContact_Request : RequestEntity
    {
        /// <summary>
        /// 联系人用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserName)]
        public string UserName { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserName, "UserName");
        }
    }

    /// <summary>
    /// 添加联系人－应答
    /// </summary>
    [Serializable, DataContract, Summary("添加联系人－应答")]
    public class User_AddContact_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }
    }
}
