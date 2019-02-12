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
    /// 上传通信录备份数据－请求
    /// </summary>
    [Serializable, DataContract, Summary("上传通信录备份数据－请求")]
    public class ContactsBackup_Upload_Request : RequestEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Token)]
        public string Token { get; set; }

        /// <summary>
        /// 联系人详细信息
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_Contacts)]
        public CbContact[] Contacts { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Contacts, "Contacts");
        }
    }

    /// <summary>
    /// 联系人备份数据
    /// </summary>
    [Serializable, DataContract, Summary("联系人备份数据")]
    public class CbContact
    {
        /// <summary>
        /// 联系人详细信息
        /// </summary>
        [DataMember, AppFieldDoc(AppField.ContactBackup_ContactData)]
        public string[] Data { get; set; }
    }
}
