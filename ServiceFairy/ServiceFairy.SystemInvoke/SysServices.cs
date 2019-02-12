using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Sys;

namespace ServiceFairy.SystemInvoke
{
    public static class SysServices
    {
        /// <summary>
        /// 将事件通知给订阅者
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult OnEvent(IServiceClient serviceClient,
            Sys_OnEvent_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call("Sys_OnEvent", request, settings);
        }

        /// <summary>
        /// 插件调用
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_OnAddinCall_Reply> AddinCall(IServiceClient serviceClient,
            Sys_OnAddinCall_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_OnAddinCall_Reply>("Sys_AddinCall", request, settings);
        }

        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAddinsInfos_Reply> GetAddinsInfos(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAddinsInfos_Reply>("Sys_GetAddinsInfos", null, settings);
        }

        /// <summary>
        /// 获取所有的接口信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAppCommandInfos_Reply> GetAppCommandInfos(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAppCommandInfos_Reply>("Sys_GetAppCommandInfos", null, settings);
        }

        /// <summary>
        /// 获取消息描述信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAppMessageInfos_Reply> GetAppMessageInfos(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAppMessageInfos_Reply>("Sys_GetAppMessageInfos", null, settings);
        }

        /// <summary>
        /// 获取消息参数示例及文档
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAppMessageDocs_Reply> GetAppMessageDocs(IServiceClient serviceClient,
            Sys_GetAppMessageDocs_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAppMessageDocs_Reply>("Sys_GetAppMessageDocs", request, settings);
        }

        /// <summary>
        /// 获取状态码描述信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAppStatusCodeInfos_Reply> GetAppStatusCodeInfos(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAppStatusCodeInfos_Reply>("Sys_GetAppStatusCodeInfos", null, settings);
        }

        /// <summary>
        /// 获取所有组件的信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAppComponentInfos_Reply> GetAppComponentInfos(IServiceClient serviceClient,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAppComponentInfos_Reply>("Sys_GetAppComponentInfos", null, settings);
        }

        /// <summary>
        /// 用于测试接口是否可用
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Hello(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call("Sys_Hello", null, settings);
        }

        /// <summary>
        /// 获取接口参数的示例与文档
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAppCommandDocs_Reply> GetAppCommandDocs(IServiceClient serviceClient,
            Sys_GetAppCommandDocs_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAppCommandDocs_Reply>("Sys_GetAppCommandDocs", request, settings);
        }

        /// <summary>
        /// 获取组件的所有属性
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAppComponentProperties_Reply> GetAppComponentProperties(IServiceClient serviceClient,
            Sys_GetAppComponentProperties_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAppComponentProperties_Reply>("Sys_GetAppComponentProperties", request, settings);
        }

        /// <summary>
        /// 获取组件的属性值
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAppComponentPropertyValues_Reply> GetAppComponentPropertyValues(IServiceClient serviceClient,
            Sys_GetAppComponentPropertyValues_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAppComponentPropertyValues_Reply>("Sys_GetAppComponentPropertyValues", request, settings);
        }

        /// <summary>
        /// 设置组件属性的值
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult SetAppComponentPropertyValues(IServiceClient serviceClient,
            Sys_SetAppComponentPropertyValues_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call("Sys_SetAppComponentPropertyValues", request, settings);
        }

        /// <summary>
        /// 获取服务基路径中的文件信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="req"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_GetAppServiceFileInfos_Reply> GetAppServiceFileInfos(
            IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_GetAppServiceFileInfos_Reply>("Sys_GetAppServiceFileInfos", null, settings);
        }

        /// <summary>
        /// 下载服务的文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_DownloadAppServiceFiles_Reply> DownloadAppServiceFiles(IServiceClient serviceClient,
            Sys_DownloadAppServiceFiles_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_DownloadAppServiceFiles_Reply>("Sys_DownloadAppServiceFiles", request, settings);
        }

        /// <summary>
        /// 判断指定的接口是否存在
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Sys_ExistsCommand_Reply> ExistsCommand(IServiceClient serviceClient,
            Sys_ExistsCommand_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Sys_ExistsCommand_Reply>("Sys_ExistsCommand", request, settings);
        }   
    }
}
