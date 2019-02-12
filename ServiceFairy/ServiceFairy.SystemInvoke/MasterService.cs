using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// Server Service
    /// </summary>
    public static class MasterService
    {
        /// <summary>
        /// 初始化客户端
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="clientId"></param>
        /// <param name="serviceDesc"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ServiceResult<Master_InitClient_Reply> InitClient(IServiceClient serviceClient,
            Master_InitClient_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_InitClient_Reply>(SFNames.ServiceNames.Master + "/InitClient", request, settings);
        }

        /// <summary>
        /// 获取部署地图
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_GetDeployMap_Reply> DownloadDeployMap(IServiceClient serviceClient,
            Master_GetDeployMap_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_GetDeployMap_Reply>(SFNames.ServiceNames.Master + "/DownloadDeployMap", request, settings);
        }

        /// <summary>
        /// 下载配置文件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_DownloadConfiguration_Reply> DownloadConfiguration(IServiceClient serviceClient,
            Master_DownloadConfiguration_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_DownloadConfiguration_Reply>(SFNames.ServiceNames.Master + "/DownloadConfiguration", request, settings);
        }

        /// <summary>
        /// 下载配置文件与部署策略的版本号
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_GetAllVersions_Reply> GetAllVersions(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_GetAllVersions_Reply>(SFNames.ServiceNames.Master + "/GetAllVersions", null, settings);
        }

        /// <summary>
        /// 获取全部安装包的信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_GetServiceDeployPackageInfos_Reply> GetServiceDeployPackageInfos(IServiceClient serviceClient,
            Master_GetServiceDeployPackageInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_GetServiceDeployPackageInfos_Reply>(SFNames.ServiceNames.Master + "/GetServiceDeployPackageInfos", request, settings);
        }

        /// <summary>
        /// 下载指定的安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_DownloadServiceDeployPackage_Reply> DownloadServiceDeployPackage(IServiceClient serviceClient,
            Master_DownloadServiceDeployPackage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_DownloadServiceDeployPackage_Reply>(SFNames.ServiceNames.Master + "/DownloadServiceDeployPackage", request, settings);
        }

        /// <summary>
        /// 部署服务的安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult DeployServicePackage(IServiceClient serviceClient,
            Master_DeployServicePackage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Master + "/DeployServicePackage", request, settings);
        }

        /// <summary>
        /// 删除服务的安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult DeleteServiceDeployPackages(IServiceClient serviceClient,
            Master_DeleteServiceDeployPackages_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Master + "/DeleteServiceDeployPackages", request, settings);
        }

        /// <summary>
        /// Station Service的心跳服务
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult StationHeartBeat(IServiceClient serviceClient,
            Master_StationHeartBeat_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Master + "/StationHeartBeat", request, settings);
        }

        /// <summary>
        /// 获取服务终端列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_GetClientList_Reply> GetClientInfoList(IServiceClient serviceClient,
            Master_GetClientList_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_GetClientList_Reply>(SFNames.ServiceNames.Master + "/GetClientInfoList", request, settings);
        }

        /// <summary>
        /// 修改部署方式
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_AdjustDeployMap_Reply> AdjustDeployMap(IServiceClient serviceClient,
            Master_AdjustDeployMap_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_AdjustDeployMap_Reply>(SFNames.ServiceNames.Master + "/AdjustDeployMap", request, settings);
        }

        /// <summary>
        /// 下载服务的UI界面信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_DownloadServiceUIInfo_Reply> DownloadServiceUIInfo(IServiceClient serviceClient,
            Master_DownloadServiceUIInfo_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_DownloadServiceUIInfo_Reply>(SFNames.ServiceNames.Master + "/DownloadServiceUIInfo", request, settings);
        }

        /// <summary>
        /// 获取所有服务终端的描述信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_GetAllClientDesc_Reply> GetAllClientDescs(IServiceClient serviceClient,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_GetAllClientDesc_Reply>(SFNames.ServiceNames.Master + "/GetAllClientDescs", null, settings);
        }

        /// <summary>
        /// 获取服务的部署信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_GetServiceDeployInfos_Reply> GetServiceDeployInfos(IServiceClient serviceClient,
            Master_GetServiceDeployInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_GetServiceDeployInfos_Reply>(SFNames.ServiceNames.Master + "/GetServiceDeployInfos", request, settings);
        }

        /// <summary>
        /// 上传服务安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult UploadServiceDeployPackage(IServiceClient serviceClient,
            Master_UploadServiceDeployPackage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Master + "/UploadServiceDeployPackage", request, settings);
        }

        /// <summary>
        /// 获取服务的部署进度
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="waitForExit"></param>
        /// <returns></returns>
        public static ServiceResult<Master_GetServiceDeployProgress_Reply> GetServiceDeployProgress(IServiceClient serviceClient,
            Master_GetServiceDeployProgress_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_GetServiceDeployProgress_Reply>(SFNames.ServiceNames.Master + "/GetServiceDeployProgress", request, settings);
        }

        /// <summary>
        /// 获取Platform安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_GetPlatformDeployPackageInfos_Reply> GetPlatformDeployPackageInfos(IServiceClient serviceClient,
            Master_GetPlatformDeployPackageInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_GetPlatformDeployPackageInfos_Reply>(SFNames.ServiceNames.Master + "/GetPlatformDeployPackageInfos", request, settings);
        }

        /// <summary>
        /// 上传Platform安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult UploadPlatformDeployPackage(IServiceClient serviceClient,
            Master_UploadPlatformDeployPackage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Master + "/UploadPlatformDeployPackage", request, settings);
        }

        /// <summary>
        /// 删除Platform安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult DeletePlatformDeployPackages(IServiceClient serviceClient,
            Master_DeletePlatformDeployPackages_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Master + "/DeletePlatformDeployPackages", request, settings);
        }

        /// <summary>
        /// 部署Platform安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult DeployPlatformPackage(IServiceClient serviceClient,
            Master_DeployPlatformPackage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Master + "/DeployPlatformPackage", request, settings);
        }

        /// <summary>
        /// 下载平台安装包
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_DownloadPlatformDeployPackage_Reply> DownloadPlatformDeployPackage(
            IServiceClient serviceClient, Master_DownloadPlatformDeployPackage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_DownloadPlatformDeployPackage_Reply>(SFNames.ServiceNames.Master + "/DownloadPlatformDeployPackage", request, settings);
        }

        /// <summary>
        /// 获取平台部署的进度
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Master_GetPlatformDeployProgress_Reply> GetPlatformDeployProgress(
            IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Master_GetPlatformDeployProgress_Reply>(SFNames.ServiceNames.Master + "/GetPlatformDeployProgress", null, settings);
        }
    }
}
