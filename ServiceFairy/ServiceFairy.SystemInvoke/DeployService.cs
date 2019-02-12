using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using ServiceFairy.Entities.Deploy;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 安装与部署服务
    /// </summary>
    public static class DeployService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.Deploy + "/" + method;
        }

        /// <summary>
        /// 获取安装包列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="serviceNames"></param>
        /// <returns></returns>
        public static ServiceResult<Deploy_GetPackageList_Reply> GetPackageList(IServiceClient serviceClient,
            Deploy_GetPackageList_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Deploy_GetPackageList_Reply>(_GetMethod("GetPackageList"), request, settings);
        }

        /// <summary>
        /// 获取指定服务所有可能的调用列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static ServiceResult<Deploy_SearchServices_Reply> SearchServices(IServiceClient serviceClient,
            Deploy_SearchServices_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Deploy_SearchServices_Reply>(_GetMethod("SearchServices"), request, settings);
        }

        /// <summary>
        /// 获取指定客户端的部署地图
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static ServiceResult<Deploy_GetDeployMap_Reply> GetDeployMap(IServiceClient serviceClient,
            Deploy_GetDeployMap_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Deploy_GetDeployMap_Reply>(_GetMethod("GetDeployMap"), request, settings);
        }

        /// <summary>
        /// 下载安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ServiceResult<Deploy_DownloadDeployPackage_Reply> DownloadDeployPackage(IServiceClient serviceClient,
            Deploy_DownloadDeployPackage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Deploy_DownloadDeployPackage_Reply>(_GetMethod("DownloadDeployPackage"), request, settings);
        }

        /// <summary>
        /// 获取所有的服务终端唯一标识
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Deploy_GetAllClientIds_Reply> GetAllClientIds(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Deploy_GetAllClientIds_Reply>(_GetMethod("GetAllClientIds"), null, settings);
        }
    }
}
