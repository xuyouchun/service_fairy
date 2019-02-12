using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm.Docking;
using WeifenLuo.WinFormsUI.Docking;
using Common.WinForm.Docking.DockingWindows;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Client.WinForm
{
    class MyServiceFairyClientApplication : ServiceFairyClientApplication
    {
        public MyServiceFairyClientApplication()
        {
            
        }

        private DockingWindowLayout _CreateDockingWindowLayout()
        {
            const int leftWidth = 200, rightWidth = 250, topHeight = 200, bottomHeight = 200;
            DockingWindowLayout layout = new DockingWindowLayout(new DockingWindowLayoutState[] {
                new DockingWindowLayoutState {
                    Type = typeof(OutputDockingWindow), AutoHidePortion = bottomHeight, DockState = DockState.DockBottomAutoHide,
                    DockAreas = DockAreas.DockBottom | DockAreas.Document | DockAreas.DockTop,
                },
                new DockingWindowLayoutState {
                    Type = typeof(NavigationForm), AutoHidePortion = leftWidth, DockState = DockState.DockLeft,
                    DockAreas = DockAreas.DockLeft | DockAreas.DockRight,
                },
            }) {
                DockLeftPortion = leftWidth, DockRightPortion = rightWidth, DockTopPortion = topHeight, DockBottomPortion = bottomHeight,
            };

            return layout;
        }

        public override void Run(Action<string, string[]> callback, System.Threading.WaitHandle waitHandle)
        {
            MainForm mainForm = new MainForm();

            UserContextManager userCtxMgr = new UserContextManager(Settings.NavigationUrl);
            ClientContext ctx = new ClientContext(mainForm.DockPanel, userCtxMgr);
            NavigationForm navForm = new NavigationForm(ctx);
            new DockingWindowManager(mainForm.DockPanel, _CreateDockingWindowLayout()).ShowDefaultLayout(new DockContentEx[] { navForm });
            navForm.AddRange(Settings.GetUserInfos());

            Application.Run(mainForm);
        }
    }
}
