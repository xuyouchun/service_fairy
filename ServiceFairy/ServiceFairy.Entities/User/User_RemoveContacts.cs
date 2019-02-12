﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 批量删除联系人－请求
    /// </summary>
    [Serializable, DataContract, Summary("批量删除联系人－请求")]
    public class User_RemoveContacts_Request : RequestEntity
    {
        /// <summary>
        /// 用户名
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
