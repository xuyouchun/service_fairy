using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 添加联系人－请求
    /// </summary>
    [Serializable, DataContract]
    public class User_AddContacts_Request : RequestEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserNames), Remarks("用户名与一个不重复整数的组合")]
        public string[] UserNames { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserNames, "UserNames");
        }
    }

    /// <summary>
    /// 添加联系人－应答
    /// </summary>
    [Serializable, DataContract]
    public class User_AddContacts_Reply : ReplyEntity
    {
        /// <summary>
        /// 对应的UserId
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds), Remarks("UserId与请求参数中该用户名对应的不重复整数的组合")]
        public string[] UserIds { get; set; }
    }

    /*

    /// <summary>
    /// 批量添加联系人－请求
    /// </summary>
    [Serializable, DataContract, Summary("批量添加联系人－请求")]
    public class User_AddContacts_Request : RequestEntity
    {
        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserNames)]
        public string[] UserNames { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserNames, "UserNames");
        }
    }

    /// <summary>
    /// 批量添加联系人－应答
    /// </summary>
    [Serializable, DataContract, Summary("批量添加联系人－应答")]
    public class User_AddContacts_Reply : ReplyEntity
    {
        /// <summary>
        /// 对应的UserId
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds)]
        public int[] UserIds { get; set; }
    }*/
}
