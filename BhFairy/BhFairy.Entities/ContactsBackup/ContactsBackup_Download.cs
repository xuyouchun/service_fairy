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
    /// 下载通信录－请求
    /// </summary>
    [Serializable, DataContract, Summary("下载通信录－请求")]
    public class ContactsBackup_Download_Request : RequestEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Token)]
        public string Token { get; set; }

        /// <summary>
        /// 起始位置
        /// </summary>
        [DataMember, Summary("下载联系人的起始位置")]
        public int Start { get; set; }

        /// <summary>
        /// 要下载的数量
        /// </summary>
        [DataMember, Summary("下载联系人的数量")]
        public int Count { get; set; }
    }

    /// <summary>
    /// 下载通信录－应答
    /// </summary>
    [Serializable, DataContract, Summary("下载通信录－应答")]
    public class ContactsBackup_Download_Reply : ReplyEntity
    {
        /// <summary>
        /// 通信录
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Contacts)]
        public CbContact[] Contacts { get; set; }
    }
}
