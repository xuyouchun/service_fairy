using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using BhFairy.Entities.ContactsBackup;
using Common.Contracts;

namespace BhFairy.Entities.ContactsBackup
{
    /// <summary>
    /// 通信录备份数据
    /// </summary>
    [Serializable, DataContract, Summary("通信录备份数据")]
    public class ContactBackupData
    {
        /// <summary>
        /// 信息
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Info)]
        public ContactBackupInfo Info { get; set; }

        /// <summary>
        /// 列头
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_ColumnHeaders)]
        public string[] ColumnHeaders { get; set; }
    }
}
