using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 更新通信录－请求
    /// </summary>
    [Serializable, DataContract, Summary("更新通信录－请求")]
    public class User_UpdateContactList_Request : RequestEntity
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
}
