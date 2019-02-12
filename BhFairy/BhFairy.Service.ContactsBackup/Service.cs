using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Components;
using BhFairy.Service.ContactsBackup.Components;
using BhFairy.Components;
using BhFairy.Entities.ContactsBackup;

namespace BhFairy.Service.ContactsBackup
{
    /// <summary>
    /// 通信录备份
    /// </summary>
    [AppEntryPoint, AppService(BhNames.ServiceNames.ContactsBackup, "1.0", "通信录备份", desc: "备份用户的通信录并提供下载")]
    class Service : BhAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                ContactsBackup = new ContactsBackupAppComponent(this),
                ContactsBackupScanner = new ContactsBackupScannerAppComponent(this),
            });

            this.LoadStatusCodeInfosFromType(typeof(ContactsBackupStatusCode));
        }

        /// <summary>
        /// 联系人备份管理器
        /// </summary>
        public ContactsBackupAppComponent ContactsBackup { get; private set; }

        /// <summary>
        /// 联系人备份数据扫描器
        /// </summary>
        public ContactsBackupScannerAppComponent ContactsBackupScanner { get; private set; }
    }
}
