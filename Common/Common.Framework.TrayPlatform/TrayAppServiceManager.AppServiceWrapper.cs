using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;

namespace Common.Framework.TrayPlatform
{
    partial class TrayAppServiceManager
    {
        class AppServiceWrapper : MarshalByRefObjectEx, IAppService
        {
            public AppServiceWrapper(TrayAppServiceManager owner, IAppService svr, TrayAppServiceInfoCollection container, TrayAppServiceInfo info)
            {
                _owner = owner;
                _srv = svr;
                _container = container;
                _info = info;
            }

            private IAppService _srv;
            private TrayAppServiceInfo _info;
            private TrayAppServiceInfoCollection _container;
            private readonly TrayAppServiceManager _owner;

            public AppServiceInfo Init(IServiceProvider sp, AppServiceInitModel initModel)
            {
                return _srv.Init(sp, initModel);
            }

            public void Start()
            {
                LogManager.LogMessage("正在启动服务 " + _info.AppServiceInfo.ServiceDesc + " ...");
                _srv.Start();
            }

            public void Stop()
            {
                LogManager.LogMessage("正在停止服务 " + _info.AppServiceInfo.ServiceDesc + " ...");
                _srv.Stop();
            }

            public AppServiceStatus Status
            {
                get { return _srv.Status; }
            }

            public ICommunicate Communicate
            {
                get { return _srv.Communicate; }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);

                if (Status == AppServiceStatus.Running)
                    Stop();

                _container.Remove(_info.AppServiceInfo.ServiceDesc.Version);

                _info.DisposeAll();

                string serviceName = _info.AppServiceInfo.ServiceDesc.Name;
                if (_owner._dict[serviceName].Count == 0)
                    _owner._dict.Remove(serviceName);

                _srv.Dispose();
                _info.AppDomain.SafeUnload();
            }

            public Contracts.ObjectProperty[] GetAllProperties()
            {
                return _srv.GetAllProperties();
            }

            public Contracts.ObjectPropertyValue GetPropertyValue(string propertyName)
            {
                return _srv.GetPropertyValue(propertyName);
            }

            public void SetPropertyValue(Contracts.ObjectPropertyValue value)
            {
                _srv.SetPropertyValue(value);
            }

            public bool Wait(AppServiceWaitType waitType, int millsecondsTimeout)
            {
                return _srv.Wait(waitType, millsecondsTimeout);
            }

            ~AppServiceWrapper()
            {
                Dispose();
            }
        }
    }
}
