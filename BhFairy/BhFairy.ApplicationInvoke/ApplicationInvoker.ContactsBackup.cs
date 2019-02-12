using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using BhFairy.Entities.ContactsBackup;
using Common;

namespace BhFairy.ApplicationInvoke
{
    partial class ApplicationInvoker
    {
        private ContactsBackupInvoker _contactsBackup;

        /// <summary>
        /// 联系人备份
        /// </summary>
        public ContactsBackupInvoker ContactsBackup
        {
            get { return _contactsBackup ?? (_contactsBackup = new ContactsBackupInvoker(this)); }
        }

        /// <summary>
        /// 联系人备份
        /// </summary>
        public class ContactsBackupInvoker : Invoker
        {
            public ContactsBackupInvoker(ApplicationInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 开始上传备份数据
            /// </summary>
            /// <param name="columnHeaders"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<string> BeginUploadSr(string[] columnHeaders, CallingSettings settings = null)
            {
                var sr = ContactsBackupService.BeginUpload(Sc,
                    new ContactsBackup_BeginUpload_Request() { ColumnHeaders = columnHeaders }, settings);

                return CreateSr(sr, r => r.Token);
            }

            /// <summary>
            /// 开始上传备份数据
            /// </summary>
            /// <param name="columnHeaders"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public string BeginUpload(string[] columnHeaders, CallingSettings settings = null)
            {
                return InvokeWithCheck(BeginUploadSr(columnHeaders, settings));
            }

            /// <summary>
            /// 上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="contacts"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult UploadSr(string token, CbContact[] contacts, CallingSettings settings = null)
            {
                return ContactsBackupService.Upload(Sc,
                    new ContactsBackup_Upload_Request() { Token = token, Contacts = contacts }, settings
                );
            }

            /// <summary>
            /// 上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="contacts"></param>
            /// <param name="settings"></param>
            public void Upload(string token, CbContact[] contacts, CallingSettings settings = null)
            {
                InvokeWithCheck(UploadSr(token, contacts, settings));
            }

            /// <summary>
            /// 上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="contact"></param>
            /// <param name="settings"></param>
            public ServiceResult UploadSr(string token, CbContact contact, CallingSettings settings = null)
            {
                return UploadSr(token, new[] { contact }, settings);
            }

            /// <summary>
            /// 上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="contact"></param>
            /// <param name="settings"></param>
            public void Upload(string token, CbContact contact, CallingSettings settings = null)
            {
                InvokeWithCheck(UploadSr(token, contact, settings));
            }

            /// <summary>
            /// 结束上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult EndUploadSr(string token, CallingSettings settings = null)
            {
                return ContactsBackupService.EndUpload(Sc,
                    new ContactsBackup_EndUpload_Request() { Token = token }, settings);
            }

            /// <summary>
            /// 结束上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            public void EndUpload(string token, CallingSettings settings = null)
            {
                InvokeWithCheck(EndUploadSr(token, settings));
            }

            /// <summary>
            /// 获取备份数据列表
            /// </summary>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<ContactBackupInfo[]> GetListSr(CallingSettings settings = null)
            {
                var sr = ContactsBackupService.GetList(Sc, new ContactsBackup_GetList_Request(), settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取备份数据列表
            /// </summary>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ContactBackupInfo[] GetList(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetListSr(settings));
            }

            /// <summary>
            /// 开始下载备份数据列表
            /// </summary>
            /// <param name="token"></param>
            /// <param name="columnHeaders"></param>
            /// <param name="info"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<string> BeginDownloadSr(string token, out string[] columnHeaders, out ContactBackupInfo info,
                CallingSettings settings = null)
            {
                var sr = ContactsBackupService.BeginDownload(Sc,
                    new ContactsBackup_BeginDownload_Request() { Name = token }, settings);

                if (sr.Succeed && sr.Result != null)
                {
                    columnHeaders = sr.Result.ColumnHeaders;
                    info = sr.Result.Info;
                }
                else
                {
                    columnHeaders = null;
                    info = null;
                }

                return CreateSr(sr, r => r.Token);
            }

            /// <summary>
            /// 开始下载
            /// </summary>
            /// <param name="token"></param>
            /// <param name="columnHeaders"></param>
            /// <param name="info"></param>
            /// <param name="settings"></param>
            public string BeginDownload(string token, out string[] columnHeaders, out ContactBackupInfo info,
                CallingSettings settings = null)
            {
                return InvokeWithCheck(BeginDownloadSr(token, out columnHeaders, out info, settings));
            }

            /// <summary>
            /// 下载联系人备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="start"></param>
            /// <param name="count"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<CbContact[]> DownloadSr(string token, int start, int count, CallingSettings settings = null)
            {
                var sr = ContactsBackupService.Download(Sc,
                    new ContactsBackup_Download_Request() { Token = token, Start = start, Count = count }, settings);

                return CreateSr(sr, r => r.Contacts);
            }

            /// <summary>
            /// 下载联系人备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="start"></param>
            /// <param name="count"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public CbContact[] Download(string token, int start, int count, CallingSettings settings = null)
            {
                return InvokeWithCheck(DownloadSr(token, start, count, settings));
            }

            /// <summary>
            /// 下载联系人备份数据
            /// </summary>
            /// <param name="toke"></param>
            /// <param name="index"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<CbContact> DownloadSr(string token, int index, CallingSettings settings = null)
            {
                var sr = DownloadSr(token, index, 1, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 下载联系人备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="index"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public CbContact Download(string token, int index, CallingSettings settings = null)
            {
                return InvokeWithCheck(DownloadSr(token, index, settings));
            }

            /// <summary>
            /// 取消下载备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult CancelDownloadSr(string token, CallingSettings settings = null)
            {
                return ContactsBackupService.CancelDownload(Sc,
                    new ContactsBackup_CancelDownload_Request() { Token = token }, settings);
            }

            /// <summary>
            /// 取消下载备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            public void CancelDownload(string token, CallingSettings settings = null)
            {
                InvokeWithCheck(CancelDownloadSr(token, settings));
            }

            /// <summary>
            /// 删除联系人备份数据
            /// </summary>
            /// <param name="names"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult DeleteSr(string[] names, CallingSettings settings = null)
            {
                return ContactsBackupService.Delete(Sc, new ContactsBackup_Delete_Request() {
                    Names = names
                }, settings);
            }

            /// <summary>
            /// 删除联系人备份数据
            /// </summary>
            /// <param name="names"></param>
            /// <param name="settings"></param>
            public void Delete(string[] names, CallingSettings settings = null)
            {
                InvokeWithCheck(DeleteSr(names, settings));
            }

            /// <summary>
            /// 删除联系人备份数据
            /// </summary>
            /// <param name="name"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult DeleteSr(string name, CallingSettings settings = null)
            {
                return DeleteSr(new[] { name }, settings);
            }

            /// <summary>
            /// 删除联系人备份数据
            /// </summary>
            /// <param name="name"></param>
            /// <param name="settings"></param>
            public void Delete(string name, CallingSettings settings = null)
            {
                InvokeWithCheck(DeleteSr(name, settings));
            }

            /// <summary>
            /// 取消上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult CancelUploadSr(string token, CallingSettings settings = null)
            {
                return ContactsBackupService.CancelUpload(Sc,
                    new ContactsBackup_CancelUpload_Request() { Token = token }, settings);
            }

            /// <summary>
            /// 取消上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            public void CancelUpload(string token, CallingSettings settings = null)
            {
                InvokeWithCheck(CancelUploadSr(token, settings));
            }

            /// <summary>
            /// 暂停上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult PauseUploadSr(string token, CallingSettings settings = null)
            {
                return ContactsBackupService.PauseUpload(Sc,
                    new ContactsBackup_PauseUpload_Request() { Token = token }, settings);
            }

            /// <summary>
            /// 暂停上传备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            public void PauseUpload(string token, CallingSettings settings = null)
            {
                InvokeWithCheck(PauseUploadSr(token, settings));
            }

            /// <summary>
            /// 结束下载通信录备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult EndDownloadSr(string token, CallingSettings settings = null)
            {
                return ContactsBackupService.EndDownload(Sc,
                    new ContactsBackup_EndDownload_Request() { Token = token }, settings);
            }

            /// <summary>
            /// 结束下载通信录备份数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            public void EndDownload(string token, CallingSettings settings = null)
            {
                InvokeWithCheck(EndDownloadSr(token, settings));
            }
        }
    }
}
