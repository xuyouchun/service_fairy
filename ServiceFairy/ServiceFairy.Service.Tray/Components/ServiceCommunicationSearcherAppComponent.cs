using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Framework.TrayPlatform;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.SystemInvoke;
using Common.Utility;
using Common.Package;
using Common.Package.Service;
using Common;
using System.Threading;

namespace ServiceFairy.Service.Tray.Components
{
    /// <summary>
    /// 寻找服务的信道
    /// </summary>
    [AppComponent("服务搜索策略", "搜索服务的部署位置")]
    class ServiceCommunicationSearcherAppComponent : AppComponent, IServiceCommunicationSearcher
    {
        public ServiceCommunicationSearcherAppComponent(Service service)
            : base(service)
        {
            _service = service;
            _invoker = SystemInvoker.FromServiceClient(service.Context.ServiceClient);
        }

        private readonly Service _service;
        private readonly SystemInvoker _invoker;

        private T _Call<T>(Func<ServiceResult<T>> action) where T : class
        {
            var sr = action();
            if (sr.Succeed)
                return sr.Result;

            if (sr.StatusCode != (int)ServerErrorCode.NotFound)
                throw sr.CreateException();

            return null;
        }

        /// <summary>
        /// 寻找指定服务的信道
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public AppInvokeInfo[] Search(ServiceDesc serviceDesc)
        {
            return _Call<AppInvokeInfo[]>(() => _invoker.Deploy.SearchServicesSr(serviceDesc));
        }

        /// <summary>
        /// 寻找指定终端的信道
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        public AppInvokeInfo[] Search(Guid[] clientIds)
        {
            AppClientDeployInfo[] dInfos = _Call<AppClientDeployInfo[]>(() => _invoker.Deploy.GetDeployMapSr(clientIds));
            return dInfos == null ? null : dInfos.ToArray(_Converter);
        }

        private static AppInvokeInfo _Converter(AppClientDeployInfo dInfo)
        {
            return new AppInvokeInfo() { ClientID = dInfo.ClientId, ServiceDescs = dInfo.Services.ToArray(v => v.ServiceDesc), CommunicateOptions = dInfo.CommunicateOptions };
        }

        /// <summary>
        /// 寻找所有终端的信道
        /// </summary>
        /// <returns></returns>
        public AppInvokeInfo[] SearchAll()
        {
            AppClientDeployInfo[] dInfos = _Call<AppClientDeployInfo[]>(() => _invoker.Deploy.GetAllDeployMapSr());
            return dInfos == null ? null : dInfos.ToArray(_Converter);
        }
    }
}
