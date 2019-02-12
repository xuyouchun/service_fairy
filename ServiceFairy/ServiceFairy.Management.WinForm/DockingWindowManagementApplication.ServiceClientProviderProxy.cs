using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common.Contracts.Service;
using Common.WinForm;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Management.WinForm
{
    partial class DockingWindowManagementApplication
    {
        class ServiceClientProviderProxy : IServiceClientProvider
        {
            public ServiceClientProviderProxy(IServiceClientProvider scProvider, IWin32Window window)
            {
                _scProvider = scProvider;
                _uiThread = Thread.CurrentThread;
                _window = window;
            }

            private readonly IServiceClientProvider _scProvider;
            private readonly Thread _uiThread;
            private readonly IWin32Window _window;
            private Sid _sid;

            public IServiceClient Get()
            {
                return new ServiceClientProxy(this, _uiThread, _scProvider.Get(), _window);
            }

            #region Class ServiceClientProxy ...

            class ServiceClientProxy : IServiceClient
            {
                public ServiceClientProxy(ServiceClientProviderProxy owner, Thread uiThread, IServiceClient serviceClient, IWin32Window window)
                {
                    _owner = owner;
                    _serviceClient = serviceClient;
                    _uiThread = uiThread;
                    _window = window;
                }

                private readonly ServiceClientProviderProxy _owner;
                private readonly IServiceClient _serviceClient;
                private readonly Thread _uiThread;
                private readonly IWin32Window _window;
                private const string _title = "正在调用：{0}";

                private static readonly TimeSpan _showDelay = TimeSpan.FromSeconds(1);

                private ServiceResult _Check(ServiceResult sr)
                {
                    if (!sr.Sid.IsEmpty())
                        _owner._sid = sr.Sid;

                    return sr;
                }

                private ServiceResult<object> _Check(ServiceResult<object> sr)
                {
                    _Check((ServiceResult)sr);
                    return sr;
                }

                private void _DealSettings(ref CallingSettings settings)
                {
                    if (settings == null)
                        settings = new CallingSettings();

                    if (settings.Sid.IsEmpty())
                        settings.Sid = _owner._sid;
                }

                public ServiceResult Call(string method, object input, CallingSettings settings = null)
                {
                    _DealSettings(ref settings);
                    if (_uiThread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
                        return _Check(_serviceClient.Call(method, input, settings));

                    return WinFormUtility.InvokeWithProgressWindow(_window,
                        () => _Check(_serviceClient.Call(method, input, settings)), string.Format(_title, method),
                        _showDelay, true, true);
                }

                public ServiceResult<object> Call(string method, object input, Type replyType, CallingSettings settings = null)
                {
                    _DealSettings(ref settings);
                    if (_uiThread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
                        return _Check(_serviceClient.Call(method, input, replyType, settings));

                    return WinFormUtility.InvokeWithProgressWindow(_window,
                        () => _Check(_serviceClient.Call(method, input, replyType, settings)), string.Format(_title, method),
                        _showDelay, true, true);
                }

                public IServiceClientReceiverHandler RegisterReceiver(string method, Type entityType, IServiceClientReceiver receiver)
                {
                    return _serviceClient.RegisterReceiver(method, entityType, receiver);
                }

                public void Dispose()
                {
                    _serviceClient.Dispose();
                }
            }

            #endregion
        }
    }
}
