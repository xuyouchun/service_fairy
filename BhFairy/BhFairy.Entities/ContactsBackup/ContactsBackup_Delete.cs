using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using System.Runtime.Serialization;
using Common.Contracts;

namespace BhFairy.Entities.ContactsBackup
{
    /// <summary>
    /// 删除联系人备份数据－请求
    /// </summary>
    [Serializable, DataContract, Summary("删除联系人备份数据－请求")]
    public class ContactsBackup_Delete_Request : RequestEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Name)]
        public string[] Names { get; set; }
    }
}
