using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using System.Collections.Concurrent;
using Common.Utility;
using Common.Package;
using ServiceFairy.Entities.File;
using ServiceFairy.SystemInvoke;
using Common;
using BhFairy.Entities.ContactsBackup;
using Common.File.UnionFile;

namespace BhFairy.Service.ContactsBackup.Components
{
    /// <summary>
    /// 联系人备份信息扫描器
    /// </summary>
    [AppComponent("联系人备份数据扫描器", "扫描通信录备份的状态，并删除多余的备份")]
    class ContactsBackupScannerAppComponent : TimerAppComponentBase
    {
        public ContactsBackupScannerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;
        private readonly ConcurrentQueue<int> _userIds = new ConcurrentQueue<int>();

        public void AddModifiedUser(int userId)
        {
            _userIds.Enqueue(userId);
        }

        protected override void OnExecuteTask(string taskName)
        {
            int[] userIds = _userIds.DequeueAll();

            foreach (int userId in userIds.Distinct())
            {
                try
                {
                    _Scan(userId);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        private void _Scan(int userId)
        {
            CallingSettings settings = TokenContext.GetTokenContext(userId, _service.Invoker).Settings;
            DirectoryInfoItem dInfoItem = _service.Invoker.File.GetDirectoryInfo(Utility.MakeBkDirectory(userId),
                GetDirectoryInfosOption.File, "*" + Utility.FILE_EXT, settings);

            UnionFileInfo[] dInfos = dInfoItem.FileInfos;
            if (dInfos.Length > MAX_BACKUP_COUNT)
            {
                IEnumerable<UnionFileInfo> expiredInfos =
                    dInfos.OrderBy(dInfo => dInfo.CreationTime).Take(dInfos.Length - MAX_BACKUP_COUNT);

                string[] expiredPaths = expiredInfos.ToArray(info => info.Path);
                _service.Invoker.File.Delete(expiredPaths, settings);
                LogManager.LogMessage(string.Format("删除{0}个通信录备份", expiredPaths.Length));
            }
        }

        private const int MAX_BACKUP_COUNT = 10;
    }
}
