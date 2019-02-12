using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities;
using Common.Contracts.Service;
using ServiceFairy.Client;
using Common.Utility;
using Common;
using ServiceFairy.Entities.Master;
using Common.Communication.Wcf;

namespace ServiceFairy.SystemInvoke
{
    partial class CoreInvoker
    {
        private MasterInvoker _master;

        /// <summary>
        /// Master Service
        /// </summary>
        public MasterInvoker Master
        {
            get { return _master ?? (_master = new MasterInvoker(this)); }
        }

        /// <summary>
        /// Master Service
        /// </summary>
        public class MasterInvoker : Invoker
        {
            public MasterInvoker(CoreInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 终端初始化
            /// </summary>
            /// <param name="client">终端</param>
            /// <param name="clientInfo">终端信息</param>
            /// <param name="initServices">初始运行的服务</param>
            /// <param name="initCommunicationOptions">初始开启的信道</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Master_InitClient_Reply> InitClientSr(ServiceEndPoint client, AppClientInfo clientInfo,
                ServiceDesc[] initServices, CommunicationOption[] initCommunicationOptions, CallingSettings settings = null)
            {
                Contract.Requires(client != null);
                Master_InitClient_Request req = new Master_InitClient_Request() {
                    Caller = client.ServiceDesc, ClientID = client.ClientId,
                    AppClientInfo = clientInfo, InitServices = initServices, InitCommunicationOptions = initCommunicationOptions
                };

                return MasterService.InitClient(Sc, req, settings);
            }

            /// <summary>
            /// 终端初始化
            /// </summary>
            /// <param name="client">终端</param>
            /// <param name="clientInfo">终端信息</param>
            /// <param name="initServices">初始运行的服务</param>
            /// <param name="initCommunicationOptions">初始开启的信道</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Master_InitClient_Reply InitClient(ServiceEndPoint client, AppClientInfo clientInfo,
                ServiceDesc[] initServices, CommunicationOption[] initCommunicationOptions, CallingSettings settings = null)
            {
                return InvokeWithCheck(InitClientSr(client, clientInfo, initServices, initCommunicationOptions, settings));
            }

            /// <summary>
            /// 获取所有服务的安装包信息
            /// </summary>
            /// <param name="onlyCurrent">是否仅返回当前使用的安装包信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployPackageInfo[]> GetAllServiceDeployPackageInfosSr(bool onlyCurrent = true, CallingSettings settings = null)
            {
                var sr = MasterService.GetServiceDeployPackageInfos(Sc,
                    new Master_GetServiceDeployPackageInfos_Request() { OnlyCurrent = onlyCurrent }, settings);

                return CreateSr(sr, r => r.ServiceDescDeployPackageInfos);
            }

            /// <summary>
            /// 获取所有服务的安装包信息
            /// </summary>
            /// <returns></returns>
            /// <param name="onlyCurrent">是否仅返回当前使用的安装包信息</param>
            /// <param name="settings">调用设置</param>
            public ServiceDeployPackageInfo[] GetAllServiceDeployPackageInfos(bool onlyCurrent = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllServiceDeployPackageInfosSr(onlyCurrent, settings));
            }

            /// <summary>
            /// 获取指定服务的安装包信息
            /// </summary>
            /// <param name="serviceDescs">服务</param>
            /// <param name="onlyCurrent">是否仅返回当前使用的安装包信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployPackageInfo[]> GetServiceDeployPackageInfosSr(ServiceDesc[] serviceDescs, bool onlyCurrent = true, CallingSettings settings = null)
            {
                Contract.Requires(serviceDescs != null);

                var sr = MasterService.GetServiceDeployPackageInfos(Sc,
                    new Master_GetServiceDeployPackageInfos_Request() { ServiceDescs = serviceDescs, OnlyCurrent = onlyCurrent }, settings);

                return CreateSr(sr, r => r.ServiceDescDeployPackageInfos);
            }

            /// <summary>
            /// 获取指定服务的安装包信息
            /// </summary>
            /// <param name="serviceDescs">服务</param>
            /// <param name="onlyCurrent">是否仅返回当前使用的安装包信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDeployPackageInfo[] GetServiceDeployPackageInfos(ServiceDesc[] serviceDescs, bool onlyCurrent = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetServiceDeployPackageInfosSr(serviceDescs, onlyCurrent, settings));
            }

            /// <summary>
            /// 获取服务的安装包信息
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="onlyCurrent">是否仅返回当前所用的安装包信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployPackageInfo[]> GetServiceDeployPackageInfosSr(Guid[] deployPackageIds, CallingSettings settings = null)
            {
                Contract.Requires(deployPackageIds != null);

                var sr = MasterService.GetServiceDeployPackageInfos(Sc,
                    new Master_GetServiceDeployPackageInfos_Request() { DeployPackageIds = deployPackageIds, OnlyCurrent = false }, settings);

                return CreateSr(sr, r => r.ServiceDescDeployPackageInfos);
            }

            /// <summary>
            /// 获取服务的安装包信息
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDeployPackageInfo[] GetServiceDeployPackageInfos(Guid[] deployPackageIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetServiceDeployPackageInfosSr(deployPackageIds, settings));
            }

            /// <summary>
            /// 获取指定服务的安装包信息
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="onlyCurrent">是否只返回当前使用的安装包信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployPackageInfo> GetServiceDeployPackageInfoSr(ServiceDesc serviceDesc, bool onlyCurrent = true, CallingSettings settings = null)
            {
                Contract.Requires(serviceDesc != null);
                var sr = GetServiceDeployPackageInfosSr(new[] { serviceDesc }, onlyCurrent, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取指定服务的安装包信息
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="onlyCurrent">是否只返回当前使用的安装包信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDeployPackageInfo GetServiceDeployPackageInfo(ServiceDesc serviceDesc, bool onlyCurrent = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetServiceDeployPackageInfoSr(serviceDesc, onlyCurrent, settings));
            }

            /// <summary>
            /// 上传服务的安装包
            /// </summary>
            /// <param name="deployPackageInfo">安装包信息</param>
            /// <param name="content">安装包内容</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult UploadServiceDeployPackageSr(ServiceDeployPackageInfo deployPackageInfo, byte[] content, CallingSettings settings = null)
            {
                Contract.Requires(deployPackageInfo != null && content != null);
                return MasterService.UploadServiceDeployPackage(Sc,
                    new Master_UploadServiceDeployPackage_Request() { DeployPackageInfo = deployPackageInfo, Content = content }, settings);
            }

            /// <summary>
            /// 上传服务的安装包
            /// </summary>
            /// <param name="deployPackageInfo">安装包信息</param>
            /// <param name="content">安装包内容</param>
            /// <param name="settings">调用设置</param>
            public void UploadServiceDeployPackage(ServiceDeployPackageInfo deployPackageInfo, byte[] content, CallingSettings settings = null)
            {
                InvokeWithCheck(UploadServiceDeployPackageSr(deployPackageInfo, content));
            }

            /// <summary>
            /// 将服务安装包部署到指定的终端
            /// </summary>
            /// <param name="clientIds">客户端标识</param>
            /// <param name="deployPackageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult DeployServicePackageSr(Guid[] clientIds, Guid deployPackageId, CallingSettings settings = null)
            {
                Contract.Requires(clientIds != null);
                return MasterService.DeployServicePackage(Sc, new Master_DeployServicePackage_Request() {
                    ClientIds = clientIds, DeployPackageId = deployPackageId,
                }, settings);
            }

            /// <summary>
            /// 将服务的安装包部署到指定的终端
            /// </summary>
            /// <param name="clientIds">客户端标识</param>
            /// <param name="deployPackageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            public void DeployServicePackage(Guid[] clientIds, Guid deployPackageId, CallingSettings settings = null)
            {
                InvokeWithCheck(DeployServicePackageSr(clientIds, deployPackageId, settings));
            }

            /// <summary>
            /// 将服务的安装包部署到所有的终端
            /// </summary>
            /// <param name="deployPackageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult DeployServicePackageToAllClientsSr(Guid deployPackageId, CallingSettings settings = null)
            {
                return MasterService.DeployServicePackage(Sc, new Master_DeployServicePackage_Request() {
                    DeployPackageId = deployPackageId,
                }, settings);
            }

            /// <summary>
            /// 将服务的安装包部署到所有的终端
            /// </summary>
            /// <param name="deployPackageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            public void DeployServicePackageToAllClients(Guid deployPackageId, CallingSettings settings = null)
            {
                InvokeWithCheck(DeployServicePackageToAllClientsSr(deployPackageId, settings));
            }

            /// <summary>
            /// 下载服务安装包
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<DeployPackage> DownloadServiceDeployPackageSr(ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                var sr = MasterService.DownloadServiceDeployPackage(Sc, new Master_DownloadServiceDeployPackage_Request() {
                    ServiceDesc = serviceDesc
                }, settings);

                return CreateSr(sr, r => r.DeployPackage);
            }

            /// <summary>
            /// 下载服务安装包
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public DeployPackage DownloadServiceDeployPackage(ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                return InvokeWithCheck(DownloadServiceDeployPackageSr(serviceDesc, settings));
            }

            /// <summary>
            /// 下载服务安装包
            /// </summary>
            /// <param name="packageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<DeployPackage> DownloadServiceDeployPackageSr(Guid packageId, CallingSettings settings = null)
            {
                var sr = MasterService.DownloadServiceDeployPackage(Sc, new Master_DownloadServiceDeployPackage_Request() {
                    DeployPackageId = packageId
                }, settings);

                return CreateSr(sr, r => r.DeployPackage);
            }

            /// <summary>
            /// 下载服务安装包
            /// </summary>
            /// <param name="packageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public DeployPackage DownloadServiceDeployPackage(Guid packageId, CallingSettings settings = null)
            {
                return InvokeWithCheck(DownloadServiceDeployPackageSr(packageId, settings));
            }

            /// <summary>
            /// 删除服务安装包
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult DeleteServiceDeployPackagesSr(Guid[] deployPackageIds, CallingSettings settings = null)
            {
                Contract.Requires(deployPackageIds != null);

                return MasterService.DeleteServiceDeployPackages(Sc,
                    new Master_DeleteServiceDeployPackages_Request() { DeployIds = deployPackageIds }, settings);
            }

            /// <summary>
            /// 删除服务安装包
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="settings">调用设置</param>
            public void DeleteServiceDeployPackages(Guid[] deployPackageIds, CallingSettings settings = null)
            {
                InvokeWithCheck(DeleteServiceDeployPackagesSr(deployPackageIds, settings));
            }

            /// <summary>
            /// 获取全部的服务终端信息
            /// </summary>
            /// <param name="clientIds">安装包标识</param>
            /// <param name="includeDetail">是否包含详细信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppClientInfo[]> GetClientInfoListSr(Guid[] clientIds = null, bool includeDetail = false, CallingSettings settings = null)
            {
                Contract.Requires(clientIds != null);

                var sr = MasterService.GetClientInfoList(Sc,
                    new Master_GetClientList_Request() { ClientIds = clientIds, IncludeDetail = includeDetail }, settings);

                return CreateSr(sr, r => r.AppClientInfos, Array<AppClientInfo>.Empty);
            }

            /// <summary>
            /// 获取全部的服务终端信息
            /// </summary>
            /// <param name="clientIds">终端ID</param>
            /// <param name="includeDetail">是否包含详细信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppClientInfo[] GetClientInfoList(Guid[] clientIds = null, bool includeDetail = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetClientInfoListSr(clientIds, includeDetail, settings));
            }

            /// <summary>
            /// 获取指定服务终端的详细信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="includeDetail">是否包含详细信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppClientInfo> GetClientInfoSr(Guid clientId, bool includeDetail = true, CallingSettings settings = null)
            {
                var sr = GetClientInfoListSr(new[] { clientId }, includeDetail, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取指定服务终端的详细信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="includeDetail">是否包含详细信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppClientInfo GetClientInfo(Guid clientId, bool includeDetail = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetClientInfoSr(clientId, includeDetail));
            }

            /// <summary>
            /// 获取全部安装包的信息
            /// </summary>
            /// <param name="onlyCurrent">是否只返回当前使用的安装包信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployPackageInfo[]> GetAllPackageInfosSr(bool onlyCurrent = true, CallingSettings settings = null)
            {
                var sr = MasterService.GetServiceDeployPackageInfos(Sc, new Master_GetServiceDeployPackageInfos_Request() { OnlyCurrent = onlyCurrent });
                return CreateSr(sr, r => r.ServiceDescDeployPackageInfos, Array<ServiceDeployPackageInfo>.Empty);
            }

            /// <summary>
            /// 获取全部安装包的信息
            /// </summary>
            /// <param name="onlyCurrent">是否只返回当前使用的安装包信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDeployPackageInfo[] GetAllPackageInfos(bool onlyCurrent = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllPackageInfosSr(onlyCurrent, settings));
            }

            /// <summary>
            /// 获取全部的服务
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDesc[]> GetAllServicesSr(CallingSettings settings = null)
            {
                var sr = GetAllPackageInfosSr(true, settings);
                return CreateSr(sr, r => r.SelectDistinct(pInfo => pInfo.ServiceDesc).ToArray());
            }

            /// <summary>
            /// 获取全部服务
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDesc[] GetAllServices(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllServicesSr(settings));
            }

            /// <summary>
            /// 修改部署地图
            /// </summary>
            /// <param name="infos">变化</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult AdjustDeployMapSr(AppClientAdjustInfo[] infos, CallingSettings settings = null)
            {
                Contract.Requires(infos != null);

                var sr = MasterService.AdjustDeployMap(Sc, new Master_AdjustDeployMap_Request() {
                    AdjustInfos = infos
                }, settings);

                return sr;
            }

            /// <summary>
            /// 修改部署地图
            /// </summary>
            /// <param name="info">修改</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult AdjustDeployMapSr(AppClientAdjustInfo info, CallingSettings settings = null)
            {
                Contract.Requires(info != null);
                return AdjustDeployMapSr(new[] { info }, settings);
            }

            /// <summary>
            /// 修改部署地图
            /// </summary>
            /// <param name="infos">修改</param>
            /// <param name="settings">调用设置</param>
            public void AdjustDeployMap(AppClientAdjustInfo[] infos, CallingSettings settings = null)
            {
                Contract.Requires(infos != null);
                AdjustDeployMapSr(infos, settings);
            }

            /// <summary>
            /// 修改部署地图
            /// </summary>
            /// <param name="info">修改</param>
            /// <param name="settings">调用设置</param>
            public void AdjustDeployMap(AppClientAdjustInfo info, CallingSettings settings = null)
            {
                Contract.Requires(info != null);
                AdjustDeployMapSr(new[] { info }, settings);
            }

            #region StartService(Guid[] clientIds, ServiceDesc[] serviceDescs, CallingSettings settings = null) ...

            /// <summary>
            /// 在指定的终端启动指定的服务
            /// </summary>
            /// <param name="clientIds">终端Id</param>
            /// <param name="serviceDescs">服务</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult StartServiceSr(Guid[] clientIds, ServiceDesc[] serviceDescs, CallingSettings settings = null)
            {
                Contract.Requires(serviceDescs != null && clientIds != null);

                return AdjustDeployMapSr(clientIds.ToArray(
                    clientId => new AppClientAdjustInfo() { ClientID = clientId, ServicesToStart = serviceDescs }));
            }

            /// <summary>
            /// 在指定的终端启动指定的服务
            /// </summary>
            /// <param name="clientIds">终端ID</param>
            /// <param name="serviceDescs">服务</param>
            /// <param name="settings">调用设置</param>
            public void StartService(Guid[] clientIds, ServiceDesc[] serviceDescs, CallingSettings settings = null)
            {
                InvokeWithCheck(StartServiceSr(clientIds, serviceDescs, settings));
            }

            #endregion

            #region StartService(Guid clientId, ServiceDesc serviceDesc, CallingSettings settings = null) ...

            /// <summary>
            /// 启动服务
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult StartServiceSr(Guid clientId, ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                Contract.Requires(serviceDesc != null);
                return StartServiceSr(new[] { clientId }, new[] { serviceDesc }, settings);
            }

            /// <summary>
            /// 启动服务
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            public void StartService(Guid clientId, ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                Contract.Requires(serviceDesc != null);

                StartService(new[] { clientId }, new[] { serviceDesc }, settings);
            }

            #endregion

            #region StartService(ServiceEndPoint endpoint, CallingSettings settings = null) ...

            /// <summary>
            /// 启动服务
            /// </summary>
            /// <param name="endpoint">服务终端</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult StartServiceSr(ServiceEndPoint endpoint, CallingSettings settings = null)
            {
                Contract.Requires(endpoint != null);

                return StartServiceSr(endpoint.ClientId, endpoint.ServiceDesc, settings);
            }

            /// <summary>
            /// 启动服务
            /// </summary>
            /// <param name="endpoint">服务终端</param>
            /// <param name="settings">调用设置</param>
            public void StartService(ServiceEndPoint endpoint, CallingSettings settings = null)
            {
                InvokeWithCheck(StartServiceSr(endpoint, settings));
            }

            #endregion

            #region StopService(Guid[] clientIds, ServiceDesc[] serviceDescs, CallingSettings settings = null) ...

            /// <summary>
            /// 在指定的终端停止指定的服务
            /// </summary>
            /// <param name="serviceDescs">服务</param>
            /// <param name="clientIds">终端ID</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult StopServiceSr(Guid[] clientIds, ServiceDesc[] serviceDescs, CallingSettings settings = null)
            {
                Contract.Requires(serviceDescs != null && clientIds != null);

                return AdjustDeployMapSr(clientIds.ToArray(
                    clientId => new AppClientAdjustInfo() { ClientID = clientId, ServicesToStop = serviceDescs }), settings);
            }

            /// <summary>
            /// 在指定的终端启动指定的服务
            /// </summary>
            /// <param name="clientIds">终端ID</param>
            /// <param name="serviceDescs">服务</param>
            /// <param name="settings">调用设置</param>
            public void StopService(Guid[] clientIds, ServiceDesc[] serviceDescs, CallingSettings settings = null)
            {
                InvokeWithCheck(StopServiceSr(clientIds, serviceDescs, settings));
            }

            #endregion

            #region StopService(Guid clientId, ServiceDesc serviceDesc, CallingSettings settings = null) ...

            /// <summary>
            /// 停止服务
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult StopServiceSr(Guid clientId, ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                Contract.Requires(serviceDesc != null);
                return StopServiceSr(new[] { clientId }, new ServiceDesc[] { serviceDesc }, settings);
            }

            /// <summary>
            /// 停止服务
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            public void StopService(Guid clientId, ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                Contract.Requires(serviceDesc != null);
                StopService(new[] { clientId }, new ServiceDesc[] { serviceDesc }, settings);
            }

            #endregion

            #region StopService(ServiceEndPoint endpoint, CallingSettings settings = null) ...

            /// <summary>
            /// 停止服务
            /// </summary>
            /// <param name="endpoint">服务终端</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult StopServiceSr(ServiceEndPoint endpoint, CallingSettings settings = null)
            {
                Contract.Requires(endpoint != null);

                return StopServiceSr(endpoint.ClientId, endpoint.ServiceDesc, settings);
            }

            /// <summary>
            /// 停止服务
            /// </summary>
            /// <param name="endpoint">服务终端</param>
            /// <param name="settings">调用设置</param>
            public void StopService(ServiceEndPoint endpoint, CallingSettings settings = null)
            {
                InvokeWithCheck(StopServiceSr(endpoint, settings));
            }

            #endregion

            #region OpenCommunicationSr(Guid clientId, CommunicationOption option, CallingSettings settings = null) ...

            /// <summary>
            /// 开启信道
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="options">信道</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult OpenCommunicationSr(Guid clientId, CommunicationOption[] options, CallingSettings settings = null)
            {
                Contract.Requires(options != null);

                return AdjustDeployMapSr(new AppClientAdjustInfo[] {
                    new AppClientAdjustInfo() {
                        ClientID = clientId,
                        CommunicationsToOpen = options,
                    }
                }, settings);
            }

            /// <summary>
            /// 开启信道
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="option">信道</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult OpenCommunicationSr(Guid clientId, CommunicationOption option, CallingSettings settings = null)
            {
                Contract.Requires(option != null);

                return OpenCommunicationSr(clientId, new[] { option }, settings);
            }

            #endregion

            #region OpenCommunication(Guid clientId, CommunicationOption option, CallingSettings settings = null) ...

            /// <summary>
            /// 开启信道
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="options">信道</param>
            /// <param name="settings">调用设置</param>
            public void OpenCommunication(Guid clientId, CommunicationOption[] options, CallingSettings settings = null)
            {
                Contract.Requires(options != null);
                InvokeWithCheck(OpenCommunicationSr(clientId, options, settings));
            }

            /// <summary>
            /// 开启信道
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="option">信道</param>
            /// <param name="settings">调用设置</param>
            public void OpenCommunication(Guid clientId, CommunicationOption option, CallingSettings settings = null)
            {
                Contract.Requires(option != null);

                OpenCommunication(clientId, new[] { option }, settings);
            }

            #endregion

            #region CloseCommunicationSr(Guid clientId, ServiceAddress serviceAddress, CallingSettings settings = null) ...

            /// <summary>
            /// 关闭信道
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceAddresses">服务地址</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult CloseCommunicationSr(Guid clientId, ServiceAddress[] serviceAddresses, CallingSettings settings = null)
            {
                Contract.Requires(serviceAddresses != null);

                return AdjustDeployMapSr(new AppClientAdjustInfo[] {
                    new AppClientAdjustInfo() {
                        ClientID = clientId,
                        CommunicationsToClose = serviceAddresses,
                    }
                }, settings);
            }

            /// <summary>
            /// 关闭信道
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceAddress">服务地址</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult CloseCommunicationSr(Guid clientId, ServiceAddress serviceAddress, CallingSettings settings = null)
            {
                Contract.Requires(serviceAddress != null);

                return CloseCommunicationSr(clientId, new[] { serviceAddress }, settings);
            }

            #endregion

            #region Class CloseCommunication(Guid clientId, ServiceAddress[] serviceAddresses, CallingSettings settings = null) ...

            /// <summary>
            /// 开启信道
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceAddresses">服务地址</param>
            /// <param name="settings">调用设置</param>
            public void CloseCommunication(Guid clientId, ServiceAddress[] serviceAddresses, CallingSettings settings = null)
            {
                Contract.Requires(serviceAddresses != null);
                var sr = CloseCommunicationSr(clientId, serviceAddresses, settings);
                Validate(sr);
            }

            /// <summary>
            /// 开启信道
            /// </summary>
            /// <param name="clientId"></param>
            /// <param name="serviceAddress"></param>
            /// <param name="settings"></param>
            public void CloseCommunication(Guid clientId, ServiceAddress serviceAddress, CallingSettings settings = null)
            {
                Contract.Requires(serviceAddress != null);

                CloseCommunication(clientId, new[] { serviceAddress }, settings);
            }

            #endregion

            /// <summary>
            /// Register服务的心跳
            /// </summary>
            /// <param name="clientInfos">终端信息</param>
            /// <param name="disconnectedClientIds">已断开连接的终端ID</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult StationHeartBeatSr(AppClientInfo[] clientInfos, Guid[] disconnectedClientIds, CallingSettings settings = null)
            {
                return MasterService.StationHeartBeat(Sc, new Master_StationHeartBeat_Request() {
                    AppClientInfos = clientInfos, DisconnectedClientIds = disconnectedClientIds
                }, settings);
            }

            /// <summary>
            /// Station服务的心跳
            /// </summary>
            /// <param name="clientInfos">终端信息</param>
            /// <param name="disconnectedClientIds">已断开连接的终端ID</param>
            /// <param name="settings">调用设置</param>
            public void StationHeartBeat(AppClientInfo[] clientInfos, Guid[] disconnectedClientIds, CallingSettings settings = null)
            {
                InvokeWithCheck(StationHeartBeatSr(clientInfos, disconnectedClientIds, settings));
            }

            /// <summary>
            /// 下载服务的管理界面
            /// </summary>
            /// <param name="serviceDescs">服务描述</param>
            /// <param name="includeAssembly">是否下载程序集</param>
            /// <param name="includeResource">是否下载资源</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceUIInfo[]> DownloadServiceUIInfoSr(ServiceDesc[] serviceDescs, bool includeAssembly = true,
                bool includeResource = true, CallingSettings settings = null)
            {
                Contract.Requires(serviceDescs != null);

                 var sr = MasterService.DownloadServiceUIInfo(Sc,
                    new Master_DownloadServiceUIInfo_Request() { ServiceDescs = serviceDescs,
                        IncludeAssembly = includeAssembly, IncludeResource = includeResource,
                    }, settings);

                 return CreateSr<ServiceUIInfo[]>(sr, sr.Result == null ? null : sr.Result.ServiceUIInfos);
            }

            /// <summary>
            /// 下载服务的管理界面
            /// </summary>
            /// <param name="serviceDescs">服务</param>
            /// <param name="includeAssembly">是否下载程序集</param>
            /// <param name="includeAssembly">是否下载资源</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceUIInfo[] DownloadServiceUIInfo(ServiceDesc[] serviceDescs, bool includeAssembly = true,
                bool includeResource = true, CallingSettings settings = null)
            {
                Contract.Requires(serviceDescs != null);

                return InvokeWithCheck(DownloadServiceUIInfoSr(serviceDescs, includeAssembly, includeResource, settings));
            }

            /// <summary>
            /// 下载服务的管理界面
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="includeAssembly">是否下载程序集</param>
            /// <param name="includeResource">是否下载资源</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceUIInfo> DownloadServiceUIInfoSr(ServiceDesc serviceDesc,
                bool includeAssembly = true, bool includeResource = true, CallingSettings settings = null)
            {
                Contract.Requires(serviceDesc != null);

                var sr = DownloadServiceUIInfoSr(new ServiceDesc[] { serviceDesc }, includeAssembly, includeResource, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 下载服务的管理界面
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="includeAssembly">是否下载程序集</param>
            /// <param name="includeResource">是否下载资源</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceUIInfo DownloadServiceUIInfo(ServiceDesc serviceDesc, bool includeAssembly = true,
                bool includeResource = true, CallingSettings settings = null)
            {
                Contract.Requires(serviceDesc != null);

                ServiceUIInfo[] r = DownloadServiceUIInfo(new ServiceDesc[] { serviceDesc }, includeAssembly, includeResource);
                return r.FirstOrDefault();
            }

            /// <summary>
            /// 获取所有终端的描述信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ClientDesc[]> GetAllClientDescSr(CallingSettings settings = null)
            {
                var sr = MasterService.GetAllClientDescs(Sc, settings);
                return CreateSr(sr, r => r.ClientDescs);
            }

            /// <summary>
            /// 获取所有终端的描述信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ClientDesc[] GetAllClientDesc(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllClientDescSr(settings));
            }

            /// <summary>
            /// 获取服务的部署信息
            /// </summary>
            /// <param name="serviceDescs">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployInfo[]> GetServiceDeployInfosSr(ServiceDesc[] serviceDescs, CallingSettings settings = null)
            {
                Contract.Requires(serviceDescs != null);

                var sr = MasterService.GetServiceDeployInfos(Sc, new Master_GetServiceDeployInfos_Request() { ServiceDescs = serviceDescs }, settings);
                return CreateSr(sr, r => r.ServiceDeployInfos);
            }

            /// <summary>
            /// 获取服务的部署信息
            /// </summary>
            /// <param name="serviceDescs">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDeployInfo[] GetServiceDeployInfos(ServiceDesc[] serviceDescs, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetServiceDeployInfosSr(serviceDescs, settings));
            }

            /// <summary>
            /// 获取所有服务的部署信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployInfo[]> GetAllServiceDeployInfosSr(CallingSettings settings = null)
            {
                var sr = MasterService.GetServiceDeployInfos(Sc, new Master_GetServiceDeployInfos_Request(), settings);
                return CreateSr(sr, r => r.ServiceDeployInfos);
            }

            /// <summary>
            /// 获取所有服务的部署信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDeployInfo[] GetAllServiceDeployInfos(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllServiceDeployInfosSr(settings));
            }

            /// <summary>
            /// 获取服务的部署信息
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployInfo> GetServiceDeployInfoSr(ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                Contract.Requires(serviceDesc != null);

                var sr = GetServiceDeployInfosSr(new[] { serviceDesc }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取服务的部署信息
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDeployInfo GetServiceDeployInfo(ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetServiceDeployInfoSr(serviceDesc, settings));
            }

            /// <summary>
            /// 获取服务的部署进度
            /// </summary>
            /// <param name="serviceDescs">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployProgress[]> GetServiceDeployProgressSr(ServiceDesc[] serviceDescs, CallingSettings settings = null)
            {
                Contract.Requires(serviceDescs != null);

                var sr = MasterService.GetServiceDeployProgress(Sc, new Master_GetServiceDeployProgress_Request() {
                    ServiceDescs = serviceDescs
                }, settings);

                return CreateSr(sr, r => r.Progresses);
            }

            /// <summary>
            /// 获取服务的部署进度
            /// </summary>
            /// <param name="serviceDescs">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDeployProgress[] GetServiceDeployProgress(ServiceDesc[] serviceDescs, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetServiceDeployProgressSr(serviceDescs, settings));
            }

            /// <summary>
            /// 获取服务的部署进度
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDeployProgress[]> GetServiceDeployProgressSr(ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                Contract.Requires(serviceDesc != null);
                return GetServiceDeployProgressSr(new[] { serviceDesc }, settings);
            }

            /// <summary>
            /// 获取服务的部署进度
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDeployProgress[] GetServiceDeployProgress(ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetServiceDeployProgressSr(serviceDesc, settings));
            }

            /// <summary>
            /// 获取平台的安装包信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<PlatformDeployPackageInfo[]> GetAllPlatformDeployPackageInfosSr(CallingSettings settings = null)
            {
                var sr = MasterService.GetPlatformDeployPackageInfos(Sc, new Master_GetPlatformDeployPackageInfos_Request(), settings);
                return CreateSr(sr, r => r.PackageInfos);
            }

            /// <summary>
            /// 获取平台的安装包信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public PlatformDeployPackageInfo[] GetAllPlatformDeployPackageInfos(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllPlatformDeployPackageInfosSr(settings));
            }

            /// <summary>
            /// 获取指定的平台安装包信息
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<PlatformDeployPackageInfo[]> GetPlatformDeployPackageInfosSr(Guid[] deployPackageIds, CallingSettings settings = null)
            {
                Contract.Requires(deployPackageIds != null);
                var sr = MasterService.GetPlatformDeployPackageInfos(Sc, new Master_GetPlatformDeployPackageInfos_Request() { Ids = deployPackageIds }, settings);
                return CreateSr(sr, r => r.PackageInfos);
            }

            /// <summary>
            /// 获取指定的平台安装包信息
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public PlatformDeployPackageInfo[] GetPlatformDeployPackageInfos(Guid[] deployPackageIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetPlatformDeployPackageInfosSr(deployPackageIds, settings));
            }

            /// <summary>
            /// 获取指定的平台安装包信息
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<PlatformDeployPackageInfo> GetPlatformDeployPackageInfoSr(Guid deployPackageIds, CallingSettings settings = null)
            {
                var sr = GetPlatformDeployPackageInfosSr(new[] { deployPackageIds }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取指定平台安装包信息
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public PlatformDeployPackageInfo GetPlatformDeployPackageInfo(Guid deployPackageIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetPlatformDeployPackageInfoSr(deployPackageIds, settings));
            }

            /// <summary>
            /// 上传平台安装包
            /// </summary>
            /// <param name="info">平台安装包信息</param>
            /// <param name="content">安装包</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult UploadPlatformDeployPackageSr(PlatformDeployPackageInfo info, byte[] content, CallingSettings settings = null)
            {
                Contract.Requires(info != null && content != null);

                var sr = MasterService.UploadPlatformDeployPackage(Sc,
                    new Master_UploadPlatformDeployPackage_Request() { Content = content, DeployPackageInfo = info }, settings);

                return sr;
            }

            /// <summary>
            /// 上传平台安装包
            /// </summary>
            /// <param name="info">平台安装包信息</param>
            /// <param name="content">安装包</param>
            /// <param name="settings">调用设置</param>
            public void UploadPlatformDeployPackage(PlatformDeployPackageInfo info, byte[] content, CallingSettings settings = null)
            {
                InvokeWithCheck(UploadPlatformDeployPackageSr(info, content, settings));
            }

            /// <summary>
            /// 删除平台安装包
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult DeletePlatformDeployPackagesSr(Guid[] deployPackageIds, CallingSettings settings = null)
            {
                Contract.Requires(deployPackageIds != null);

                return MasterService.DeletePlatformDeployPackages(Sc,
                    new Master_DeletePlatformDeployPackages_Request() { DeployIds = deployPackageIds }, settings);
            }

            /// <summary>
            /// 删除平台安装包
            /// </summary>
            /// <param name="deployPackageIds">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void DeletePlatformDeployPackages(Guid[] deployPackageIds, CallingSettings settings = null)
            {
                InvokeWithCheck(DeletePlatformDeployPackagesSr(deployPackageIds, settings));
            }

            /// <summary>
            /// 将平台安装包部署到指定的终端
            /// </summary>
            /// <param name="clientIds">终端标识</param>
            /// <param name="deployPackageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult DeployPlatformPackageSr(Guid[] clientIds, Guid deployPackageId, CallingSettings settings = null)
            {
                Contract.Requires(clientIds != null);

                return MasterService.DeployPlatformPackage(Sc, new Master_DeployPlatformPackage_Request() {
                    ClientIds = clientIds, DeployPackageId = deployPackageId,
                }, settings);
            }

            /// <summary>
            /// 将平台安装包部署到指定的终端
            /// </summary>
            /// <param name="clientIds">终端标识</param>
            /// <param name="deployPackageId">安装包ID</param>
            /// <<param name="settings">调用设置</param>
            public void DeployPlatformPackage(Guid[] clientIds, Guid deployPackageId, CallingSettings settings = null)
            {
                InvokeWithCheck(DeployPlatformPackageSr(clientIds, deployPackageId, settings));
            }

            /// <summary>
            /// 将平台安装包部署到全部终端
            /// </summary>
            /// <param name="deployPackageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult DeployPlatformPackageToAllClientsSr(Guid deployPackageId, CallingSettings settings = null)
            {
                return MasterService.DeployPlatformPackage(Sc, new Master_DeployPlatformPackage_Request() {
                    ClientIds = null, DeployPackageId = deployPackageId
                }, settings);
            }

            /// <summary>
            /// 将平台安装包部署到全部终端
            /// </summary>
            /// <param name="deployPackageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            public void DeployPlatformPackageToAllClients(Guid deployPackageId, CallingSettings settings = null)
            {
                InvokeWithCheck(DeployPlatformPackageToAllClientsSr(deployPackageId));
            }

            /// <summary>
            /// 下载平台安装包
            /// </summary>
            /// <param name="deployPackageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<DeployPackage> DownloadPlatformDeployPackageSr(Guid deployPackageId, CallingSettings settings = null)
            {
                var sr = MasterService.DownloadPlatformDeployPackage(Sc, new Master_DownloadPlatformDeployPackage_Request() {
                    DeployPackageId = deployPackageId
                }, settings);

                return CreateSr(sr, r => r.DeployPackage);
            }

            /// <summary>
            /// 下载平台安装包
            /// </summary>
            /// <param name="deployPackageId">安装包ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public DeployPackage DownloadPlatformDeployPackage(Guid deployPackageId, CallingSettings settings = null)
            {
                return InvokeWithCheck(DownloadPlatformDeployPackageSr(deployPackageId, settings));
            }

            /// <summary>
            /// 获取平台安装包的部署进度
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<PlatformDeployProgress[]> GetPlatformDeployProgressSr(CallingSettings settings = null)
            {
                var sr = MasterService.GetPlatformDeployProgress(Sc, settings);
                return CreateSr(sr, r => r.Progresses);
            }

            /// <summary>
            /// 获取平台安装包的部署进度
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public PlatformDeployProgress[] GetPlatformDeployProgress(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetPlatformDeployProgressSr(settings));
            }
        }
    }
}
