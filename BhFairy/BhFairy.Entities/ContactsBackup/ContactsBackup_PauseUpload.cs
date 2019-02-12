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
    /// 暂停上传通信录－请求
    /// </summary>
    [Serializable, DataContract, Summary("暂停上传通信录－请求")]
    public class ContactsBackup_PauseUpload_Request : RequestEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Token)]
        public string Token { get; set; }
    }
}
