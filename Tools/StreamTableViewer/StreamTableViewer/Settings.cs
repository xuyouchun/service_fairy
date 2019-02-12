using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WinForm.Docking;
using Common.WinForm.Docking.DockingWindows;
using WeifenLuo.WinFormsUI.Docking;

namespace StreamTableViewer
{
    static class Settings
    {
        /// <summary>
        /// 创建默认的布局
        /// </summary>
        /// <returns></returns>
        public static DockingWindowLayout CreateDefaultDockingWindowLayout()
        {
            const int leftWidth = 200, rightWidth = 250, topHeight = 200, bottomHeight = 200;

            return new DockingWindowLayout(new DockingWindowLayoutState[] {
                new DockingWindowLayoutState {  // Navigation
                    Type = typeof(NavigationDockingWindow), AutoHidePortion = leftWidth, DockState = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft,
                    DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                },
                new DockingWindowLayoutState {  // Output
                    Type = typeof(OutputDockingWindow), AutoHidePortion = bottomHeight, DockState = DockState.DockBottomAutoHide,
                    DockAreas = DockAreas.DockBottom | DockAreas.Document | DockAreas.DockTop
                },
                new DockingWindowLayoutState {  // Property
                    Type = typeof(PropertyDockingWindow), AutoHidePortion = rightWidth, DockState = DockState.DockRight,
                    DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                },
            }) {
                DockLeftPortion = leftWidth, DockRightPortion = rightWidth, DockTopPortion = topHeight, DockBottomPortion = bottomHeight,
            };
        }
    }
}
