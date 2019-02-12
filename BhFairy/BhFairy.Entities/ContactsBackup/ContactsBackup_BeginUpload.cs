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
    /// 开始备份－请求
    /// </summary>
    [Serializable, DataContract, Summary("开始备份－请求")]
    public class ContactsBackup_BeginUpload_Request : RequestEntity
    {
        /// <summary>
        /// 列名
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_ColumnHeaders)]
        public string[] ColumnHeaders { get; set; }
    }

    /// <summary>
    /// 开始备份－应答
    /// </summary>
    [Serializable, DataContract, Summary("开始备份－应答")]
    public class ContactsBackup_BeginUpload_Reply : ReplyEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Token)]
        public string Token { get; set; }   
    }
}
