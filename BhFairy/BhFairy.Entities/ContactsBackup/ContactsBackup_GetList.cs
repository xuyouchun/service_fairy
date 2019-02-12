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
    /// 获取备份列表－请求
    /// </summary>
    [Serializable, DataContract, Summary("获取备份列表－请求")]
    public class ContactsBackup_GetList_Request : RequestEntity
    {

    }

    /// <summary>
    /// 获取备份列表－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取备份列表－应答")]
    public class ContactsBackup_GetList_Reply : ReplyEntity
    {
        /// <summary>
        /// 信息
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Info)]
        public ContactBackupInfo[] Infos { get; set; }
    }

    /// <summary>
    /// 通信录备份信息
    /// </summary>
    [Serializable, DataContract, Summary("通信录备份信息")]
    public class ContactBackupInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackupInfo_Name)]
        public string Name { get; set; }

        /// <summary>
        /// 标题（用于显示）
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackupInfo_Title)]
        public string Title { get; set; }

        /// <summary>
        /// 备份日期
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackupInfo_Time)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 联系人数量
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackupInfo_ContactCount)]
        public int ContactCount { get; set; }
    }
}
