using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using Common.WinForm;
using System.Threading;
using Common.Utility;
using ServiceFairy.SystemInvoke;
using Common.Package.Service;
using Common.Contracts;
using Common.Communication.Wcf;

namespace ServiceFairy.Management.WinForm
{
    /// <summary>
    /// SericeObjectProvider的代理类，用于惰性初始化
    /// </summary>
    class SmServiceObjectProviderProxy : MarshalByRefObjectEx, IServiceObjectProvider
    {
        public SmServiceObjectProviderProxy(IWin32Window window)
        {
            _window = window;
            _serviceObjectProvider = new Lazy<IServiceObjectProvider>(_CreateServiceObjectProvider, true);
        }

        public void SetServiceCommunicationOption(CommunicationOption navigation, SystemInvoker invoker)
        {
            Contract.Requires(invoker != null);
            _navigation = navigation;
            _invoker = invoker;
        }

        private CommunicationOption _navigation;
        private SystemInvoker _invoker;
        private readonly IWin32Window _window;

        private readonly Lazy<IServiceObjectProvider> _serviceObjectProvider;

        private IServiceObjectProvider _CreateServiceObjectProvider()
        {
            return WinFormUtility.InvokeWithProgressWindow<IServiceObjectProvider>(_window,
                 () => _LoadServiceObjectProvider(_invoker), string.Format("正在连接服务器 {0} ...", _navigation),
                 showCancelButton: true, throwError: true, topMost: true);
        }

        private IServiceObjectProvider _LoadServiceObjectProvider(SystemInvoker invoker)
        {
            SfManagementContext mgrCtx = new SfManagementContext(invoker);
            IServiceObjectProvider provider = SmServiceObjectProvider.Load(mgrCtx);

            return provider;
        }

        public IServiceObjectTree GetTree()
        {
            IServiceObjectProvider soProvider = _serviceObjectProvider.Value;
            return soProvider == null ? null : soProvider.GetTree();
        }
    }
}
