using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using BhFairy.Entities.ContactsBackup;
using Common.Contracts.Service;
using Common.Contracts;
using System.Diagnostics.Contracts;

namespace BhFairy.ApplicationInvoke
{
    /// <summary>
    /// 联系人备份服务
    /// </summary>
    public static class ContactsBackupService
    {
        /// <summary>
        /// 开始上传联系人备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<ContactsBackup_BeginUpload_Reply> BeginUpload(IServiceClient serviceClient, ContactsBackup_BeginUpload_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<ContactsBackup_BeginUpload_Reply>(BhNames.ServiceNames.ContactsBackup + "/BeginUpload", req, settings);
        }

        /// <summary>
        /// 上传联系人备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Upload(IServiceClient serviceClient, ContactsBackup_Upload_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(BhNames.ServiceNames.ContactsBackup + "/Upload", req, settings);
        }

        /// <summary>
        /// 结束联系人备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult EndUpload(IServiceClient serviceClient, ContactsBackup_EndUpload_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(BhNames.ServiceNames.ContactsBackup + "/EndUpload", req, settings);
        }

        /// <summary>
        /// 获取联系人备份数据的列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<ContactsBackup_GetList_Reply> GetList(IServiceClient serviceClient, ContactsBackup_GetList_Request req,
             CallingSettings settings = null)
        {
            return serviceClient.Call<ContactsBackup_GetList_Reply>(BhNames.ServiceNames.ContactsBackup + "/GetList", req, settings);
        }

        /// <summary>
        /// 开始下载联系人备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<ContactsBackup_BeginDownload_Reply> BeginDownload(IServiceClient serviceClient, ContactsBackup_BeginDownload_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<ContactsBackup_BeginDownload_Reply>(BhNames.ServiceNames.ContactsBackup + "/BeginDownload", req, settings);
        }

        /// <summary>
        /// 下载联系人备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<ContactsBackup_Download_Reply> Download(IServiceClient serviceClient, ContactsBackup_Download_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<ContactsBackup_Download_Reply>(BhNames.ServiceNames.ContactsBackup + "/Download", req, settings);
        }

        /// <summary>
        /// 取消下载联系人备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult CancelDownload(IServiceClient serviceClient, ContactsBackup_CancelDownload_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(BhNames.ServiceNames.NameCardSharing + "/CancelDownload", req, settings);
        }

        /// <summary>
        /// 删除联系人备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Delete(IServiceClient serviceClient, ContactsBackup_Delete_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(BhNames.ServiceNames.ContactsBackup + "/Delete", req, settings);
        }

        /// <summary>
        /// 取消上传备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult CancelUpload(IServiceClient serviceClient, ContactsBackup_CancelUpload_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(BhNames.ServiceNames.ContactsBackup + "/CancelUpload", req, settings);
        }

        /// <summary>
        /// 暂停上传备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult PauseUpload(IServiceClient serviceClient, ContactsBackup_PauseUpload_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(BhNames.ServiceNames.ContactsBackup + "/PauseUpload", req, settings);
        }

        /// <summary>
        /// 结束下载通信录备份数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult EndDownload(IServiceClient serviceClient, ContactsBackup_EndDownload_Request req,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(BhNames.ServiceNames.ContactsBackup + "/EndDownload", req, settings);
        }
    }
}
