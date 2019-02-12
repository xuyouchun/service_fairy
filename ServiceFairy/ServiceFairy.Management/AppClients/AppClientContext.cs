using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Client;
using Common.Package;
using ServiceFairy.Entities;
using Common.Contracts.Service;
using Common;
using ServiceFairy.Entities.Master;
using Common.Communication.Wcf;

namespace ServiceFairy.Management.AppClients
{
    class AppClientContext
    {
        public AppClientContext(SfManagementContext mgrCtx, ClientDesc clientDesc)
        {
            MgrCtx = mgrCtx;
            ClientDesc = clientDesc;

            _appClientInfo = new AutoLoad<AppClientInfo>(delegate() {
                AppClientInfo info = mgrCtx.Invoker.Master.GetClientInfo(clientDesc.ClientID);
                if (info == null)
                    throw new ApplicationException("该终端不存在：ID=" + clientDesc.ClientID);
                return info;
            }, TimeSpan.FromSeconds(5));
        }

        private readonly AutoLoad<AppClientInfo> _appClientInfo;

        public SfManagementContext MgrCtx { get; private set; }

        public AppClientInfo AppClientInfo { get { return _appClientInfo.Value; } }

        public ClientDesc ClientDesc { get; private set; }

        /// <summary>
        /// 运行的所有服务
        /// </summary>
        public ServiceInfo[] ServiceInfos
        {
            get
            {
                return AppClientInfo.ServiceInfos ?? Array<ServiceInfo>.Empty;
            }
        }

        /// <summary>
        /// 获取指定的服务
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public ServiceInfo GetServiceInfo(ServiceDesc sd)
        {
            return ServiceInfos.FirstOrDefault(si => si.ServiceDesc == sd);
        }

        /// <summary>
        /// 是否包含指定的服务
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public bool ExistServiceInfo(ServiceDesc sd)
        {
            return GetServiceInfo(sd) != null;
        }

        /// <summary>
        /// 所有信道
        /// </summary>
        public CommunicationOption[] CommunicationOptions
        {
            get
            {
                return AppClientInfo.Communications ?? Array<CommunicationOption>.Empty;
            }
        }

        /// <summary>
        /// 获取指定的信道
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        public CommunicationOption GetCommunicationOption(ServiceAddress sa)
        {
            return CommunicationOptions.FirstOrDefault(co => co.Address == sa);
        }

        /// <summary>
        /// 是否包含指定的信道
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        public bool ExistCommunication(ServiceAddress sa)
        {
            return GetCommunicationOption(sa) != null;
        }

        /// <summary>
        /// 所有调用列表
        /// </summary>
        public AppInvokeInfo[] InvokeInfos
        {
            get { return AppClientInfo.InvokeInfos ?? Array<AppInvokeInfo>.Empty; }
        }

        /// <summary>
        /// 获取指定终端的调用列表
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public AppInvokeInfo GetInvokeInfo(Guid clientId)
        {
            return InvokeInfos.FirstOrDefault(iv => iv.ClientID == clientId);
        }

        /// <summary>
        /// 是否包含指定终端的调用列表
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool ExistInvokeInfo(Guid clientId)
        {
            return GetInvokeInfo(clientId) != null;
        }
    }
}
