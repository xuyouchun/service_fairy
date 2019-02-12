using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Configuration;
using Common.WinForm.Docking;
using Common.WinForm.Docking.DockingWindows;
using WeifenLuo.WinFormsUI.Docking;
using Common.Utility;
using Common.Communication.Wcf;

namespace ServiceFairy.Management.WinForm
{
    static class Settings
    {
        /// <summary>
        /// 默认的导航地址
        /// </summary>
        public static CommunicationOption[] GetNavigations()
        {
            List<CommunicationOption> ops = new List<CommunicationOption>();
            string s = ConfigurationManager.AppSettings.Get("navigations");
            if (s != null)
            {
                foreach (string item in s.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    CommunicationOption op;
                    if (CommunicationOption.TryParse(item, out op))
                        ops.Add(op);
                }
            }

            if (ops.Count == 0)
                ops.Add(CommunicationOption.Parse("net.http://localhost:8090"));

            return ops.ToArray();
        }

        /// <summary>
        /// 创建默认的布局
        /// </summary>
        /// <returns></returns>
        public static DockingWindowLayout CreateDefaultDockingWindowLayout()
        {
            const int leftWidth = 200, rightWidth = 250, topHeight = 200, bottomHeight = 200;

            return new DockingWindowLayout(new DockingWindowLayoutState[] {
                new DockingWindowLayoutState {  // Tree
                    Type = typeof(TreeViewDockingWindow), AutoHidePortion = leftWidth, DockState = DockState.DockLeft,
                    DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                },
                new DockingWindowLayoutState {  // Navigation
                    Type = typeof(NavigationDockingWindow), AutoHidePortion = leftWidth, DockState = DockState.DockLeft,
                    DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                },
                new DockingWindowLayoutState {  // Output
                    Type = typeof(OutputDockingWindow), AutoHidePortion = bottomHeight, DockState = DockState.DockBottomAutoHide,
                    DockAreas = DockAreas.DockBottom | DockAreas.Document | DockAreas.DockTop
                },
                new DockingWindowLayoutState {  // Property
                    Type = typeof(PropertyDockingWindow), AutoHidePortion = rightWidth, DockState = DockState.DockRightAutoHide,
                    DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                },
            }) {
                DockLeftPortion = leftWidth, DockRightPortion = rightWidth, DockTopPortion = topHeight, DockBottomPortion = bottomHeight,
            };
        }

        /// <summary>
        /// 默认打开的路径
        /// </summary>
        public static string DefaultOpen
        {
            get { return CommonUtility.GetFromAppConfig<string>("defaultOpen", ""); }
        }
    }
}
