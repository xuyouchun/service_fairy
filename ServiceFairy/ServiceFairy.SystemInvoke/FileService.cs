using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.File;
using Common.Package.Service;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public static class FileService
    {
        /// <summary>
        /// 获取文件的路由信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_GetRouteInfo_Reply> GetRouteInfo(IServiceClient serviceClient,
            File_GetRouteInfo_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_GetRouteInfo_Reply>(SFNames.ServiceNames.File + "/GetRouteInfo", request, settings);
        }

        /// <summary>
        /// 开始上传文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_BeginUpload_Reply> BeginUpload(IServiceClient serviceClient,
            File_BeginUpload_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_BeginUpload_Reply>(SFNames.ServiceNames.File + "/BeginUpload", request, settings);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_Upload_Reply> Upload(IServiceClient serviceClient,
            File_Upload_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_Upload_Reply>(SFNames.ServiceNames.File + "/Upload", request, settings);
        }

        /// <summary>
        /// 一次上传全部文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult UploadAll(IServiceClient serviceClient, File_UploadAll_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.File + "/UploadAll", request, settings);
        }

        /// <summary>
        /// 开始下载文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_BeginDownload_Reply> BeginDownload(IServiceClient serviceClient,
            File_BeginDownload_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_BeginDownload_Reply>(SFNames.ServiceNames.File + "/BeginDownload", request, settings);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_Download_Reply> Download(IServiceClient serviceClient,
            File_Download_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_Download_Reply>(SFNames.ServiceNames.File + "/Download", request, settings);
        }

        /// <summary>
        /// 一次下载全部文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_DownloadAll_Reply> DownloadAll(IServiceClient serviceClient,
            File_DownloadAll_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_DownloadAll_Reply>(SFNames.ServiceNames.File + "/DownloadAll", request, settings);
        }

        /// <summary>
        /// 直接下载文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_DownloadDirect_Reply> DownloadDirect(IServiceClient serviceClient,
            File_DownloadDirect_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_DownloadDirect_Reply>(SFNames.ServiceNames.File + "/DownloadDirect", request, settings);
        }

        /// <summary>
        /// 直接上传文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_UploadDirect_Reply> UploadDirect(IServiceClient serviceClient,
            File_UploadDirect_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_UploadDirect_Reply>(SFNames.ServiceNames.File + "/UploadDirect", request, settings);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Delete(IServiceClient serviceClient,
            File_Delete_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.File + "/Delete", request, settings);
        }

        /// <summary>
        /// 开始上传StreamTable文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_BeginUploadStreamTable_Reply> BeginUploadStreamTable(IServiceClient serviceClient, 
            File_BeginUploadStreamTable_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_BeginUploadStreamTable_Reply>(SFNames.ServiceNames.File + "/BeginUploadStreamTable", request, settings);
        }

        /// <summary>
        /// 上传StreamTable文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult UploadStreamTable(IServiceClient serviceClient, 
            File_UploadStreamTable_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.File + "/UploadStreamTable", request, settings);
        }

        /// <summary>
        /// 开始下载StreamTable
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_BeginDownloadStreamTable_Reply> BeginDownloadStreamTable(IServiceClient serviceClient, 
            File_BeginDownloadStreamTable_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_BeginDownloadStreamTable_Reply>(SFNames.ServiceNames.File + "/BeginDownloadStreamTable", request, settings);
        }

        /// <summary>
        /// 下载StreamTable
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_DownloadStreamTable_Reply> DownloadStreamTable(IServiceClient serviceClient,
            File_DownloadStreamTable_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_DownloadStreamTable_Reply>(SFNames.ServiceNames.File + "/DownloadStreamTable", request, settings);
        }

        /// <summary>
        /// 结束上传或下载
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult End(IServiceClient serviceClient,
            File_End_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.File + "/End", request, settings);
        }

        /// <summary>
        /// 取消上传或下载
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Cancel(IServiceClient serviceClient,
            File_Cancel_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.File + "/Cancel", request, settings);
        }

        /// <summary>
        /// 获取指定目录的信息及子目录和文件信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_GetDirectoryInfos_Reply> GetDirectoryInfos(IServiceClient serviceClient,
            File_GetDirectoryInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_GetDirectoryInfos_Reply>(SFNames.ServiceNames.File + "/GetDirectoryInfos", request, settings);
        }

        /// <summary>
        /// 获取指定文件的信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<File_GetFileInfos_Reply> GetFileInfos(IServiceClient serviceClient,
            File_GetFileInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<File_GetFileInfos_Reply>(SFNames.ServiceNames.File + "/GetFileInfos", request, settings);
        }
    }
}
