using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using Common.Package;
using Common.Framework.TrayPlatform;
using ServiceFairy.Client;
using Common.Utility;

namespace ServiceFairy.TrayManagement
{
    class TraySystemInvoker : IDisposable
    {
        public TraySystemInvoker(CommunicationOption communicationOption, Guid clientId)
        {
            _clientId = clientId;
            _sfClient = new ServiceFairyClient(_wcfService.Connect(communicationOption.Address, communicationOption.Type));
            _systemInvoker = SystemInvoker.FromServiceClient(_sfClient);

            _allServiceDescs = new AutoLoad<ServiceDesc[]>(_LoadServiceDescs);
            _appClientInfo = new AutoLoad<AppClientInfo>(_LoadAppClientInfo);
        }

        private readonly WcfService _wcfService = new WcfService();
        private readonly SystemInvoker _systemInvoker;
        private readonly ServiceFairyClient _sfClient;
        private readonly Guid _clientId;

        private readonly AutoLoad<ServiceDesc[]> _allServiceDescs;

        private ServiceDesc[] _LoadServiceDescs()
        {
            return _systemInvoker.Master.GetAllServices(); ;
        }

        /// <summary>
        /// 获取所有的服务
        /// </summary>
        /// <returns></returns>
        public ServiceDesc[] GetAllServiceDescs()
        {
            return _allServiceDescs.Value;
        }

        private readonly AutoLoad<AppClientInfo> _appClientInfo;

        private AppClientInfo _LoadAppClientInfo()
        {
            return _systemInvoker.Master.GetClientInfo(_clientId, true);
        }

        /// <summary>
        /// 所有运行的服务
        /// </summary>
        /// <returns></returns>
        public ServiceDesc[] GetRunningServices()
        {
            AppClientInfo clientInfo = _appClientInfo.Value;
            if (clientInfo == null || clientInfo.ServiceInfos == null)
                return null;

            return clientInfo.ServiceInfos.SelectFromList(c => c.ServiceDesc);
        }

        /// <summary>
        /// 所有开启的端口
        /// </summary>
        /// <returns></returns>
        public CommunicationOption[] GetOpenedCommunicationOptions()
        {
            AppClientInfo clientInfo = _appClientInfo.Value;
            if (clientInfo == null)
                return null;

            return clientInfo.Communications;
        }

        public void Dispose()
        {
            _wcfService.Dispose();
        }
    }
}
