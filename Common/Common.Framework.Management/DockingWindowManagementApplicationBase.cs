using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.WinForm.Docking.DockingWindows;
using Common.WinForm.Docking;
using Common.Package;
using Common.Framework.Management.DockingWindow;
using Common.Utility;
using System.Windows.Forms;
using Common.Contracts.UIObject;
using Common.WinForm;
using WeifenLuo.WinFormsUI.Docking;

namespace Common.Framework.Management
{
    /// <summary>
    /// 基于可停靠窗口的应用
    /// </summary>
    public abstract class DockingWindowManagementApplicationBase : ManagementApplicationBase
    {
        public DockingWindowManagementApplicationBase()
        {
            _outputWindow = new Lazy<IOutputWindow>(CreateOutputWindow);
        }

        /// <summary>
        /// 创建默认的布局
        /// </summary>
        /// <returns></returns>
        protected abstract IObjectProvider<DockingWindowContext> CreateDockingWindowContext();

        /// <summary>
        /// 创建导航项
        /// </summary>
        /// <returns></returns>
        protected abstract INavigation CreateNavigation();

        /// <summary>
        /// 创建列表显示窗口创建器
        /// </summary>
        /// <returns></returns>
        protected abstract IListViewWindowManager CreateListViewWindowManager();

        protected sealed override IOutput CreateOutput()
        {
            return _outputWindow.Value;
        }

        protected abstract IOutputWindow CreateOutputWindow();

        private readonly Lazy<IOutputWindow> _outputWindow;

        /// <summary>
        /// 创建可停靠窗体的创建工厂
        /// </summary>
        /// <returns></returns>
        protected abstract IDockingWindowFactory CreateDockingWindowFactory();

        /// <summary>
        /// 创建UI界面操作
        /// </summary>
        /// <returns></returns>
        protected abstract IUIOperation CreateUIOperation();

        protected override IServiceProvider GetServiceProvider()
        {
            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IObjectProvider<DockingWindowContext>), CreateDockingWindowContext());
            sp.AddService(typeof(INavigation), CreateNavigation());
            sp.AddService(typeof(IDockingWindowFactory), CreateDockingWindowFactory());
            sp.AddService(typeof(IListViewWindowManager), CreateListViewWindowManager());
            sp.AddService(typeof(IOutputWindow), CreateOutputWindow());
            sp.AddService(typeof(IUIOperation), CreateUIOperation());

            return ServiceProvider.Combine(base.GetServiceProvider(), sp);
        }

        protected override void OnRun(ManagementContext context)
        {
            IServiceProvider sp = context.ServiceProvider;
            DockingWindowContext windowContext = sp.GetService<IObjectProvider<DockingWindowContext>>(true).Get();
            UIObjectExecuteContext executeContext = new UIObjectExecuteContext(sp);

            windowContext.MainForm.Load += delegate { _OnLoad(sp, context, windowContext, executeContext); };

            Application.Run(windowContext.MainForm);
        }

        private void _OnLoad(IServiceProvider sp, ManagementContext mgCtx, DockingWindowContext dwCtx, UIObjectExecuteContext exeCtx)
        {
            try
            {
                if (OnPreLoad(mgCtx, exeCtx))
                {
                    dwCtx.DockingWindowManager.ShowDefaultLayout(sp.GetService<IDockingWindowFactory>());
                    sp.GetService<INavigation>(true).Show(exeCtx, sp.GetService<IServiceObjectProvider>(true));

                    DockPanel dp = mgCtx.DockingWindowContext.DockPanel;
                    dp.ActiveDocumentChanged += delegate { _SetCurrent(exeCtx, dp.ActiveDocument as IListViewWindow); };
                    mgCtx.DockingWindowContext.MainForm.FormClosing += delegate(object sender, FormClosingEventArgs e) {
                        if (!mgCtx.DockingWindowContext.DockingWindowManager.GetAll().All(dc => dc.ClosingNotify()))
                            e.Cancel = true;
                    };

                    if (!OnPostLoad(mgCtx, exeCtx))
                        dwCtx.MainForm.Close();
                }
                else
                {
                    dwCtx.MainForm.Close();
                }
            }
            catch (Exception ex)
            {
                if (!(ex is UserCancelException))
                    ErrorDialog.Show(dwCtx.MainForm, ex);

                dwCtx.MainForm.Close();
            }
        }

        private void _SetCurrent(UIObjectExecuteContext exeCtx, IListViewWindow listWindow)
        {
            IListViewWindowManager lvMgr = exeCtx.ServiceProvider.GetService<IListViewWindowManager>();
            INavigation nav = exeCtx.ServiceProvider.GetService<INavigation>();
            IListViewData lvd;
            IServiceObject so = (listWindow == null) ? null : (lvd = listWindow.GetCurrent()) == null ? null : lvd.Owner;

            if (lvMgr != null)
                lvMgr.SetCurrent(listWindow);

            if (nav != null)
                nav.SetCurrent(exeCtx, so);
        }

        protected virtual bool OnPreLoad(ManagementContext context, IUIObjectExecuteContext exeCtx)
        {
            return true;
        }

        protected virtual bool OnPostLoad(ManagementContext context, IUIObjectExecuteContext exeCtx)
        {
            return true;
        }
    }
}
