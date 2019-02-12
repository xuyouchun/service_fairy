using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Package.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Utility;
using Common.Contracts;
using ServiceFairy.Entities.Navigation;
using System.Diagnostics;
using Common.Package;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 与服务器端的连接
    /// </summary>
    public class SystemConnection : IDisposable, IServiceClientProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigation">导航服务器地址</param>
        /// <param name="communicationType">网络协议</param>
        /// <param name="dataFormat">数据格式</param>
        /// <param name="direction">通信方向</param>
        /// <param name="target">调用目标</param>
        public SystemConnection(CommunicationOption navigation, CommunicationType communicationType = CommunicationType.Unknown,
            CommunicationDirection direction = CommunicationDirection.None, DataFormat dataFormat = DataFormat.Unknown, ServiceEndPoint target = null)
        {
            Contract.Requires(navigation != null);

            _navigation = navigation;
            _communicationType = communicationType;
            _dataFormat = dataFormat;
            _direction = direction;
            _target = target;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigation">导航服务器地址</param>
        /// <param name="communicationType">网络协议</param>
        /// <param name="direction">通信方向</param>
        /// <param name="dataFormat">数据格式</param>
        /// <param name="target">调用目标</param>
        public SystemConnection(ServiceAddress navigation, CommunicationType communicationType = CommunicationType.Unknown,
            CommunicationDirection direction = CommunicationDirection.None, DataFormat dataFormat = DataFormat.Unknown, ServiceEndPoint target = null)
            : this(new CommunicationOption(navigation), communicationType, direction, dataFormat, target)
        {

        }

        public SystemConnection()
        {

        }

        private readonly CommunicationOption _navigation;
        private readonly CommunicationType _communicationType;
        private readonly DataFormat _dataFormat;
        private readonly ServiceEndPoint _target;
        private readonly CommunicationDirection _direction;
        private readonly WcfService _service = new WcfService();

        private CommunicationOption[] _proxyList;
        private CommunicationOption _currentProxy;

        private readonly object _thisLock = new object();

        /// <summary>
        /// 获取代理服务器列表
        /// </summary>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public CommunicationOption[] GetProxyList(bool refresh = false)
        {
            if (_proxyList == null || refresh)
            {
                _proxyList = _GetProxyList();
            }

            return _proxyList;
        }

        private CommunicationOption[] _GetProxyList()
        {
            return InvokerBase.GetProxyList(_navigation, _communicationType, _direction, _dataFormat, throwErrorWhenEmpty: true);
        }

        /// <summary>
        /// 当前代理
        /// </summary>
        public CommunicationOption CurrentProxy
        {
            get
            {
                CommunicationOption[] options = GetProxyList();
                if (_currentProxy == null || !options.Contains(_currentProxy))
                {
                    if (options.Length == 0)
                        return null;

                    _currentProxy = options.PickOneRandom();  // 随机选取一个
                    _RaiseCurrentProxyChangedEvent();
                }

                return _currentProxy;
            }
            set
            {
                if (_currentProxy != value)
                {
                    lock (_thisLock)
                    {
                        _currentProxy = value;
                        if (_sfClient != null)
                            _sfClient.Dispose();

                        _sfClient = null;
                        _RaiseCurrentProxyChangedEvent();
                    }
                }
            }
        }

        /// <summary>
        /// 当前代理变化
        /// </summary>
        public event EventHandler CurrentProxyChanged;

        private void _RaiseCurrentProxyChangedEvent()
        {
            var eh = CurrentProxyChanged;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        private volatile IServiceClient _sfClient;

        /// <summary>
        /// 创建一个ServiceClient
        /// </summary>
        /// <returns></returns>
        public IServiceClient CreateServiceClient()
        {
            lock (_thisLock)
            {
                IServiceClient sf = _sfClient;
                if (sf != null)
                    return sf;

                if (_sfClient == null)
                {
                    CommunicationOption proxy = CurrentProxy;
                    WcfConnection con = _service.Connect(proxy.Address, proxy.Type, proxy.Duplex);
                    con.Open();

                    _sfClient = new ServiceClientProxy(this, new ServiceFairyClient(con, dataFormat: _dataFormat, disposeIt: true));
                }

                return _sfClient;
            }
        }

        #region Class ServiceClientProxy ...

        private class ServiceClientProxy : IServiceClient
        {
            public ServiceClientProxy(SystemConnection owner, IServiceClient innerServiceClient)
            {
                _owner = owner;
                _innerServiceClient = innerServiceClient;
            }

            private readonly SystemConnection _owner;
            private IServiceClient _innerServiceClient;

            public ServiceResult Call(string method, object input, CallingSettings settings = null)
            {
                return _Invoke(() => _innerServiceClient.Call(method, input, settings));
            }

            public ServiceResult<object> Call(string method, object input, Type entityType, CallingSettings settings = null)
            {
                return _Invoke(() => _innerServiceClient.Call(method, input, entityType, settings));
            }

            private T _Invoke<T>(Func<T> func)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public IServiceClientReceiverHandler RegisterReceiver(string method, Type entityType, IServiceClientReceiver receiver)
            {
                return _innerServiceClient.RegisterReceiver(method, entityType, receiver);
            }

            public void Dispose()
            {
                _innerServiceClient.Dispose();
            }
        }

        #endregion

        IServiceClient IObjectProvider<IServiceClient>.Get()
        {
            return CreateServiceClient();
        }

        public void Dispose()
        {
            _service.Dispose();
        }
    }
}
