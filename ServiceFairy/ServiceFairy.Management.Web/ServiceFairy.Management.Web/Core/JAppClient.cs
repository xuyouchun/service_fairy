using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceFairy.Entities.Master;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;
using ServiceFairy.Client;
using Common.Package;
using Common.Utility;
using ServiceFairy.Entities.Sys;
using Common.Contracts;
using Common;

namespace ServiceFairy.Management.Web.Core
{
    public class JAppClient
    {
        public JAppClient(SystemInvoker invoker, ClientDesc clientDesc)
        {
            _invoker = invoker;
            ClientDesc = clientDesc;
            _appClientInfo = new AutoLoad<AppClientInfo>(
                () => invoker.Master.GetClientInfo(clientDesc.ClientID, true), TimeSpan.FromSeconds(30));
        }

        private readonly SystemInvoker _invoker;

        /// <summary>
        /// 终端标识
        /// </summary>
        public ClientDesc ClientDesc { get; set; }

        public AppClientInfo AppClientInfo
        {
            get { return _appClientInfo.Value; }
        }

        private readonly AutoLoad<AppClientInfo> _appClientInfo;

        public ServiceDesc[] GetServices()
        {
            return AppClientInfo.ServiceInfos.ToArray(si => si.ServiceDesc);
        }
    }
}