using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.SystemInvoke;
using System.Diagnostics.Contracts;
using BhFairy.Entities.ContactsBackup;
using Common.Utility;
using ServiceFairy.Entities.File;
using Common.Package;
using Common.Package.Storage;
using Common.Contracts.Service;
using Common;
using Common.File.UnionFile;

namespace BhFairy.Service.ContactsBackup.Components
{
    /// <summary>
    /// 联系人备份管理器
    /// </summary>
    class ContactsBackupAppComponent : AppComponent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public ContactsBackupAppComponent(Service service)
            : base(service)
        {
            _service = service;
            _fileInvoker = service.Invoker.File;
            _fc = new FileClient(service.Invoker);
        }

        private readonly Service _service;
        private readonly SystemInvoker.FileInvoker _fileInvoker;
        private readonly FileClient _fc;

        private TokenContext _GetTokenContext(Sid sid)
        {
            UserSessionState uss = _service.UserManager.GetSessionState(sid, throwError: true);
            return TokenContext.GetTokenContext(uss.BasicInfo.UserId, _service.Invoker);
        }

        /// <summary>
        /// 开始备份
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="columnHeaders">列名</param>
        public string BeginUpload(UserSessionState uss, string[] columnHeaders)
        {
            string path = Utility.MakeBkPath(uss.BasicInfo.UserId, Guid.NewGuid().ToString() + Utility.FILE_EXT);

            CallingSettings settings = _fc.CreateCallingSettings(path);
            NewStreamTableInfo tableInfo = new NewStreamTableInfo() {
                Name = "contact_list", Desc = "通信录列表",
                Columns = columnHeaders.ToArray(columnName => new StreamTableColumn(columnName, StreamTableColumnType.String))
            };

            string token = _fileInvoker.BeginUploadStreamTable(path, "contact_backup", "通信录备份", tableInfo, settings);
            TokenContext.SetTokenContext(uss.BasicInfo.UserId, new TokenContext(uss.BasicInfo.UserId, settings));
            return token;
        }

        /// <summary>
        /// 上传通信录
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="token"></param>
        /// <param name="index"></param>
        /// <param name="contacts"></param>
        public void Upload(UserSessionState uss, string token, CbContact[] contacts)
        {
            Contract.Requires(contacts != null);

            TokenContext tokenContext = _GetTokenContext(uss.Sid);
            StreamTableRowData[] rows = contacts.ToArray(c => new StreamTableRowData() { Datas = c.Data });
            _fileInvoker.UploadStreamTable(token, rows, settings: tokenContext.Settings);
        }

        /// <summary>
        /// 结束上传通信录
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="token"></param>
        public void EndUpload(UserSessionState uss, string token)
        {
            TokenContext tokenContext = _GetTokenContext(uss.Sid);
            _fileInvoker.End(token, tokenContext.Settings);

            _service.ContactsBackupScanner.AddModifiedUser(uss.BasicInfo.UserId);
        }

        /// <summary>
        /// 获取备份列表
        /// </summary>
        /// <param name="uss"></param>
        /// <returns></returns>
        public ContactBackupInfo[] GetList(UserSessionState uss)
        {
            TokenContext tokenContext = _GetTokenContext(uss.Sid);

            DirectoryInfoItem dInfoItem = _fileInvoker.GetDirectoryInfo(Utility.MakeBkDirectory(tokenContext.UserId),
                GetDirectoryInfosOption.File, "*" + Utility.FILE_EXT, tokenContext.Settings);

            if (dInfoItem == null || dInfoItem.FileInfos.IsNullOrEmpty())
                return Array<ContactBackupInfo>.Empty;

            return dInfoItem.FileInfos.ToArray(_CreateContactBackupInfo);
        }

        private ContactBackupInfo _CreateContactBackupInfo(UnionFileInfo fi)
        {
            return new ContactBackupInfo() {
                Name = fi.Name, Time = fi.LastModifyTime, Title = fi.LastModifyTime.ToString("yyyy-MM-dd HH:mm:ss"),
                ContactCount = 0,
            };
        }

        /// <summary>
        /// 开始下载备份
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="name"></param>
        /// <param name="basicInfo"></param>
        /// <param name="fileInfo"></param>
        public string BeginDownload(UserSessionState uss, string name, out StreamTableBasicInfo basicInfo, out UnionFileInfo fileInfo)
        {
            TokenContext tokenContext = _GetTokenContext(uss.Sid);

            string token = _fileInvoker.BeginDownloadStreamTable(Utility.MakeBkPath(tokenContext.UserId, name), out basicInfo, out fileInfo, tokenContext.Settings);
            StreamTableBasicTableInfo info = basicInfo.TableInfos.FirstOrDefault(ti => ti.Name == "contact_list");
            if (info == null)
            {
                _fileInvoker.Cancel(token, tokenContext.Settings);
                throw new ServiceException(ServerErrorCode.DataError, "错误的通信录备份文件格式");
            }

            return token;
        }

        /// <summary>
        /// 下载备份
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="token"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public CbContact[] Download(UserSessionState uss, string token, int start, int count)
        {
            TokenContext tokenContext = _GetTokenContext(uss.Sid);

            StreamTableRowData[] rows = _fileInvoker.DownloadStreamTable(token, "contact_list", start, count, tokenContext.Settings);
            return rows.ToArray(r => new CbContact() { Data = r.Datas });
        }

        /// <summary>
        /// 取消下载
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="token"></param>
        public void CancelDownload(UserSessionState uss, string token)
        {
            Contract.Requires(token != null);

            TokenContext tokenContext = _GetTokenContext(uss.Sid);
            _fileInvoker.Cancel(token, tokenContext.Settings);
        }

        /// <summary>
        /// 删除联系人备份数据
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="name"></param>
        public void Delete(UserSessionState uss, string name)
        {
            Contract.Requires(name != null);

            TokenContext tokenContext = _GetTokenContext(uss.Sid);
            _fileInvoker.Delete(Utility.MakeBkPath(tokenContext.UserId, name), tokenContext.Settings);
        }

        /// <summary>
        /// 删除联系人备份数据
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="names"></param>
        public void Delete(UserSessionState uss, string[] names)
        {
            Contract.Requires(names != null);

            TokenContext tokenContext = _GetTokenContext(uss.Sid);
            _fileInvoker.Delete(names.ToArray(name => Utility.MakeBkPath(tokenContext.UserId, name)), tokenContext.Settings);
        }

        /// <summary>
        /// 暂停上传
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="token"></param>
        public void PauseUpload(UserSessionState uss, string token)
        {

        }

        /// <summary>
        /// 取消上传
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="token"></param>
        public void CancelUpload(UserSessionState uss, string token)
        {
            Contract.Requires(token != null);

            TokenContext tokenContext = _GetTokenContext(uss.Sid);
            _fileInvoker.Cancel(token, tokenContext.Settings);
        }

        /// <summary>
        /// 结束上传
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="token"></param>
        public void EndDownload(UserSessionState uss, string token)
        {
            Contract.Requires(token != null);

            TokenContext tokenContext = _GetTokenContext(uss.Sid);
            _fileInvoker.End(token, tokenContext.Settings);
        }
    }
}
