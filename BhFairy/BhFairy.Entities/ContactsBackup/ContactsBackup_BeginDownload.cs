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
    /// 开始下载通信录－请求
    /// </summary>
    [Serializable, DataContract, Summary("开始下载通信录－请求")]
    public class ContactsBackup_BeginDownload_Request : RequestEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Name)]
        public string Name { get; set; }
    }

    /// <summary>
    /// 开始下载通信录－应答
    /// </summary>
    [Serializable, DataContract, Summary("开始下载通信录－应答")]
    public class ContactsBackup_BeginDownload_Reply : ReplyEntity
    {
        /// <summary>
        /// 列头
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_ColumnHeaders)]
        public string[] ColumnHeaders { get; set; }

        /// <summary>
        /// 通信录备份的信息
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Info)]
        public ContactBackupInfo Info { get; set; }

        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Token)]
        public string Token { get; set; }
    }
}
