using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Deploy;

namespace ServiceFairy.SystemInvoke
{
    partial class CoreInvoker
    {
        private DeployInvoker _deploy;

        /// <summary>
        /// Deploy Service
        /// </summary>
        public DeployInvoker Deploy
        {
            get { return _deploy ?? (_deploy = new DeployInvoker(this)); }
        }
        
        /// <summary>
        /// Deploy Service
        /// </summary>
        public class DeployInvoker : Invoker
        {
            public DeployInvoker(CoreInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 寻找指定服务的信道
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppInvokeInfo[]> SearchServicesSr(ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                Deploy_SearchServices_Request req = new Deploy_SearchServices_Request() { ServiceDesc = serviceDesc };
                var sr = DeployService.SearchServices(Sc, req, settings);

                return CreateSr(sr, r => r.AppInvokeInfos);
            }

            /// <summary>
            /// 寻找指定服务的信道
            /// </summary>
            /// <param name="serviceDesc">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppInvokeInfo[] SearchServices(ServiceDesc serviceDesc, CallingSettings settings = null)
            {
                return InvokeWithCheck(SearchServicesSr(serviceDesc, settings));
            }

            /// <summary>
            /// 获取指定终端的部署信息
            /// </summary>
            /// <param name="clientIds">客户端ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppClientDeployInfo[]> GetDeployMapSr(Guid[] clientIds, CallingSettings settings = null)
            {
                var sr = DeployService.GetDeployMap(Sc, new Deploy_GetDeployMap_Request() { ClientIDs = clientIds }, settings);

                return CreateSr(sr, r => r.DeployInfos);
            }

            /// <summary>
            /// 获取指定终端的部署信息
            /// </summary>
            /// <param name="clientId">客户端ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppClientDeployInfo> GetDeployMapSr(Guid clientId, CallingSettings settings = null)
            {
                var sr = GetDeployMapSr(new[] { clientId }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取指定终端的部署信息
            /// </summary>
            /// <param name="clientIds">客户端ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppClientDeployInfo[] GetDeployMap(Guid[] clientIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetDeployMapSr(clientIds, settings));
            }

            /// <summary>
            /// 获取指定终端的部署信息
            /// </summary>
            /// <param name="clientId">客户端ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppClientDeployInfo GetDeployMap(Guid clientId, CallingSettings settings = null)
            {
                var infos = GetDeployMap(new[] { clientId }, settings);
                return infos == null ? null : infos.FirstOrDefault();
            }

            /// <summary>
            /// 获取全部终端的部署信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppClientDeployInfo[]> GetAllDeployMapSr(CallingSettings settings = null)
            {
                return GetDeployMapSr(null, settings);
            }

            /// <summary>
            /// 获取全部终端的部署信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppClientDeployInfo[] GetAllDeployMap(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllDeployMapSr(settings));
            }

            /// <summary>
            /// 获取所有服务终端的唯一标识
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Guid[]> GetAllClientIdsSr(CallingSettings settings = null)
            {
                var sr = DeployService.GetAllClientIds(Sc, settings);
                return CreateSr(sr, r => r.ClientIds);
            }

            /// <summary>
            /// 获取所有服务终端的唯一标识
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Guid[] GetAllClientIds(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllClientIdsSr(settings));
            }
        }
    }
}
