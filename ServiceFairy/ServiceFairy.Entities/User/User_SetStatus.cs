using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 设置用户状态－请求
    /// </summary>
    [Serializable, DataContract, Summary("设置用户状态－请求")]
    public class User_SetStatus_Request : RequestEntity
    {
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
    }
}
