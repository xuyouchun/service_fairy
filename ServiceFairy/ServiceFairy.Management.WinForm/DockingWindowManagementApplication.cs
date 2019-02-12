using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Framework.Management;
using Common.Contracts;
using Common.Framework.Management.DockingWindow;
using Common.WinForm.Docking;
using Common.Contracts.Service;
using Common.WinForm.Docking.DockingWindows;
using Common.Contracts.UIObject;
using Common.Package;
using WeifenLuo.WinFormsUI.Docking;
using Common.WinForm;
using ServiceFairy.WinForm;
using Common.Package.Service;
using ServiceFairy.SystemInvoke;
using System.Threading;
using System.Windows.Forms;
using ServiceFairy.Entities.Security;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Management.WinForm
{
    partial class DockingWindowManagementApplication : DockingWindowManagementApplicationBase
    {
        public DockingWindowManagementApplication()
        {
            _mainForm = new MainForm();
            _defaultDockingWindowLayout = Settings.CreateDefaultDockingWindowLayout();
            _dockingWindowContext = new DockingWindowContext(_mainForm, _mainForm.MainMenuStrip, _mainForm.StatusStrip,
                _mainForm.DockPanel, new DockingWindowManager(_mainForm.DockPanel, _defaultDockingWindowLayout));

            _listViewWindowManager = new ListViewWindowManager(_CreateListViewWindow);
            _navigation = new NavigationItemCollection(new INavigation[] { _mainForm, _navigationDockingWindow, new NavigationItemAdapter(_treeViewDockingWindow) });
            _soProviderProxy = new SmServiceObjectProviderProxy(_mainForm);
            _uiOperation = _mainForm.CreateUIOperation();
        }

        private readonly DockingWindowLayout _defaultDockingWindowLayout;
        private readonly MainForm _mainForm;
        private readonly DockingWindowContext _dockingWindowContext;

        private readonly OutputDockingWindow _outputDockingWindow = new OutputDockingWindow();
        private readonly PropertyDockingWindow _propertyDockingWindow = new PropertyDockingWindow();
        private readonly NavigationDockingWindow _navigationDockingWindow = new NavigationDockingWindow();
        private readonly TreeViewDockingWindow _treeViewDockingWindow = new TreeViewDockingWindow();
        private readonly INavigation _navigation;

        private IListViewWindow _CreateListViewWindow()
        {
            ListViewDockingWindow window = new ListViewDockingWindow(_uiOperation);
            window.Show(_mainForm.DockPanel, DockState.Document);
            window.FormClosed += delegate { _listViewWindowManager.Close(window); };
            return window;
        }

        // 窗体界面元素
        protected override IObjectProvider<DockingWindowContext> CreateDockingWindowContext()
        {
            return new ObjectProvider<DockingWindowContext>(_dockingWindowContext);
        }

        // ServiceObject提供策略
        protected override IServiceObjectProvider CreateServiceObjectProvider()
        {
            return _soProviderProxy;
        }

        private readonly SmServiceObjectProviderProxy _soProviderProxy;

        // 输出窗口
        protected override IOutputWindow CreateOutputWindow()
        {
            return _outputDockingWindow;
        }

        // 导航窗口
        protected override INavigation CreateNavigation()
        {
            return _navigation;
        }

        private readonly ListViewWindowManager _listViewWindowManager;

        // 列表窗口创建器
        protected override IListViewWindowManager CreateListViewWindowManager()
        {
            return _listViewWindowManager;
        }

        private readonly IUIOperation _uiOperation;

        /// <summary>
        /// 创建UI界面操作
        /// </summary>
        /// <returns></returns>
        protected override IUIOperation CreateUIOperation()
        {
            return _uiOperation;
        }

        // 可停靠窗口创建工厂
        protected override IDockingWindowFactory CreateDockingWindowFactory()
        {
            return new DockingWindowFactory(new Dictionary<Type, DockContentEx> {
                { typeof(OutputDockingWindow), _outputDockingWindow },
                { typeof(PropertyDockingWindow), _propertyDockingWindow },
                { typeof(NavigationDockingWindow), _navigationDockingWindow },
                { typeof(TreeViewDockingWindow), _treeViewDockingWindow },
            });
        }

        private IServiceProvider _sp;
        private MenuStripCommandManager _manager;
        private SmContext _smContext;

        // 保存ServiceProvider
        protected override IServiceProvider GetServiceProvider()
        {
            _sp = base.GetServiceProvider();
            _smContext = new SmContext(_sp);
            return _sp;
        }

        protected override void OnRun(ManagementContext context)
        {
            DockingWindowContext dc = _dockingWindowContext;
            _manager = new MenuStripCommandManager(dc.MenuStrip, this.GetType().Assembly, _smContext);

            base.OnRun(context);
        }

        protected override bool OnPreLoad(ManagementContext context, IUIObjectExecuteContext exeCtx)
        {
            base.OnPreLoad(context, exeCtx);

            while (true)
            {
                try
                {
                    LoginResult result = SmUtility.Login(_mainForm);
                    if (result != null)
                    {
                        SystemConnection sysCon = new SystemConnection(result.Navigation, communicationType: result.CommunicationType, dataFormat: result.DataFormat);
                        SystemInvoker invoker = new SystemInvoker(new ServiceClientProviderProxy(sysCon, _mainForm));

                        string username = result.UserName.Trim();
                        if (username.Length > 0)
                            _Login(invoker, username, result.Password);

                        _soProviderProxy.SetServiceCommunicationOption(result.Navigation, invoker);
                        _mainForm.Text = "Service Fairy Management - " + result.Navigation;
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    ErrorDialog.Show(_mainForm, ex);
                }
            }
        }

        // 登录：首先尝试系统用户，再尝试普通用户
        private void _Login(SystemInvoker invoker, string username, string password)
        {
            ServiceResult sr = invoker.Security.LoginSr(username, password);
            if (!sr.Succeed)
            {
                if (sr.StatusCode == (int)SecurityStatusCode.InvalidUser)
                    invoker.User.Login(username, password);
                else
                    throw sr.CreateException();
            }
        }

        protected override bool OnPostLoad(ManagementContext context, IUIObjectExecuteContext exeCtx)
        {
            // 打开默认的路径
            string defaultOpen = Settings.DefaultOpen;
            if (string.IsNullOrWhiteSpace(defaultOpen))
                return base.OnPostLoad(context, exeCtx);

            IServiceObjectTreeNode node = new ServiceObjectTreeNode(ServiceObject.FromObject(new object()), new[] { context.ServiceObjectProvider.GetTree().Root });
            foreach (string serviceName in defaultOpen.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                IServiceObjectTreeNode subNode = node.FirstOrDefault(n => n.ServiceObject.Info.Name == serviceName.Trim());
                if (subNode == null)
                    break;

                node = subNode;
            }

            if (node != null)
                node.ServiceObject.ExecuteActionWithErrorDialog(exeCtx, ServiceObjectActionType.Open);

            return base.OnPostLoad(context, exeCtx);
        }
    }
}
