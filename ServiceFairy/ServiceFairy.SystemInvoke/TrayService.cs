using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 终端服务
    /// </summary>
    public static class TrayService
    {
        private static string _GetMethod(string methodName)
        {
            return SFNames.ServiceNames.Tray + "/" + methodName;
        }

        /// <summary>
        /// 获取该终端上运行的所有服务
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetAllServices_Reply> GetAllServices(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Tray_GetAllServices_Reply>(_GetMethod("GetAllServices"), null, settings);
        }

        /// <summary>
        /// 获取本地日志组
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetLocalLogGroups_Reply> GetLocalLogGroups(IServiceClient serviceClient, Tray_GetLocalLogGroups_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Tray_GetLocalLogGroups_Reply>(_GetMethod("GetLocalLogGroups"), request, settings);
        }

        /// <summary>
        /// 获取本地指定组的日志
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetLocalLogItems_Reply> GetLocalLogItems(IServiceClient serviceClient,
            Tray_GetLocalLogItems_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Tray_GetLocalLogItems_Reply>(_GetMethod("GetLocalLogItems"), request, settings);
        }

        /// <summary>
        /// 删除日志组
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult DeleteLocalLogGroups(IServiceClient serviceClient,
            Tray_DeleteLocalLogGroups_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(_GetMethod("DeleteLocalLogGroups"), request, settings);
        }

        /// <summary>
        /// 根据时间获取本地日志
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetLocalLogItemsByTime_Reply> GetLocalLogItemsByTime(IServiceClient serviceClient,
            Tray_GetLocalLogItemsByTime_Request request, CallingSettings settings)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Tray_GetLocalLogItemsByTime_Reply>(_GetMethod("GetLocalLogItemsByTime"), request, settings);
        }

        /// <summary>
        /// 获取系统日志分组
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetSystemLogGroups_Reply> GetSystemLogGroups(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Tray_GetSystemLogGroups_Reply>(_GetMethod("GetSystemLogGroups"), settings);
        }

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetSystemLogItems_Reply> GetSystemLogItems(IServiceClient serviceClient, Tray_GetSystemLogItems_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Tray_GetSystemLogItems_Reply>(_GetMethod("GetSystemLogItems"), request, settings);
        }

        /// <summary>
        /// 获取文件系统根目录信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_FsGetRootDirectoryInfos_Reply> FsGetRootDirectoryInfos(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Tray_FsGetRootDirectoryInfos_Reply>(_GetMethod("FsGetRootDirectoryInfos"), null, settings);
        }

        /// <summary>
        /// 获取指定目录的信息及子目录、所包含的文件信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="?"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_FsGetDirectoryInfos_Reply> FsGetDirectoryInfos(IServiceClient serviceClient,
            Tray_FsGetDirectoryInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Tray_FsGetDirectoryInfos_Reply>(_GetMethod("FsGetDirectoryInfos"), request, settings);
        }

        /// <summary>
        /// 下载文件系统中的文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_FsDownloadFile_Reply> FsDownloadFile(IServiceClient serviceClient,
            Tray_FsDownloadFile_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Tray_FsDownloadFile_Reply>(_GetMethod("FsDownloadFile"), request, settings);
        }

        /// <summary>
        /// 删除文件或目录
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult FsDeletePath(IServiceClient serviceClient,
            Tray_FsDeletePath_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("FsDeletePath"), request, settings);
        }

        /// <summary>
        /// 获取进程列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetProcesses_Reply> GetProcesses(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Tray_GetProcesses_Reply>(_GetMethod("GetProcesses"), null, settings);
        }

        /// <summary>
        /// 结束进程
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult KillProcesses(IServiceClient serviceClient,
            Tray_KillProcesses_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("KillProcesses"), request, settings);
        }

        /// <summary>
        /// 启动新进程
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult StartProcesses(IServiceClient serviceClient,
            Tray_StartProcesses_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("StartProcesses"), request, settings);
        }

        /// <summary>
        /// 获取线程列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetThreads_Reply> GetThreads(IServiceClient serviceClient,
            Tray_GetThreads_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Tray_GetThreads_Reply>(_GetMethod("GetThreads"), request, settings);
        }

        /// <summary>
        /// 获取系统属性
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetSystemProperties_Reply> GetSystemProperties(IServiceClient serviceClient,
            Tray_GetSystemProperties_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Tray_GetSystemProperties_Reply>(_GetMethod("GetSystemProperties"), request, settings);
        }

        /// <summary>
        /// 读取环境变量
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Tray_GetSystemEnvironmentVariables_Reply> GetSystemEnvironmentVariables(IServiceClient serviceClient,
            Tray_GetSystemEnvironmentVariables_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Tray_GetSystemEnvironmentVariables_Reply>(_GetMethod("GetSystemEnvironmentVariables"), request, settings);
        }

        /// <summary>
        /// 设置环境变量
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult SetSystemEnvironmentVariables(IServiceClient serviceClient,
            Tray_SetSystemEnvironmentVariables_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("SetSystemEnvironmentVariables"), request, settings);
        }
    }
}
