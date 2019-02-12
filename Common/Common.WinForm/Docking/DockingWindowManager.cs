using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Contracts;
using System.Diagnostics.Contracts;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using Common.Package;
using Common.Utility;
using Common.Contracts.Service;

namespace Common.WinForm.Docking
{
    /// <summary>
    /// 停靠窗口的管理器
    /// </summary>
    public class DockingWindowManager : MarshalByRefObjectEx
    {
        public DockingWindowManager(DockPanel dockPanel, DockingWindowLayout defaultLayout)
        {
            Contract.Requires(dockPanel != null);

            _dockPanel = dockPanel;
            _defaultLayout = defaultLayout ?? new DockingWindowLayout();
        }

        private readonly DockPanel _dockPanel;
        private readonly DockingWindowLayout _defaultLayout;
        private readonly Dictionary<UniqueId, DockContentEx> _windows = new Dictionary<UniqueId, DockContentEx>();

        /// <summary>
        /// 获取指定类型的窗体
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DockContentEx[] GetDockContents(Type type)
        {
            Contract.Requires(type != null);

            return _windows.Values.Where(v => type.IsInstanceOfType(v)).ToArray();
        }

        /// <summary>
        /// 获取第一个指定类型的窗体，如果不存在则返回空引用
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DockContentEx GetDockContent(Type type)
        {
            Contract.Requires(type != null);

            return _windows.Values.FirstOrDefault(v => type.IsInstanceOfType(v));
        }

        /// <summary>
        /// 获取指定类型的窗体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetDockContents<T>()
            where T : DockContentEx
        {
            return _windows.Values.OfType<T>().ToArray();
        }

        /// <summary>
        /// 获取指定类型的第一个窗体，如果不存在则返回空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDockContent<T>()
        {
            return _windows.Values.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// 获取所有窗体
        /// </summary>
        /// <returns></returns>
        public DockContentEx[] GetAll()
        {
            return _windows.Values.ToArray();
        }

        /// <summary>
        /// 显示默认的布局
        /// </summary>
        public void ShowDefaultLayout(IDockingWindowFactory windowFactory = null)
        {
            ShowLayout(_defaultLayout, windowFactory);
        }

        /// <summary>
        /// 显示默认的布局
        /// </summary>
        /// <param name="dockContents"></param>
        public void ShowDefaultLayout(IEnumerable<DockContentEx> dockContents)
        {
            Contract.Requires(dockContents != null);

            ShowDefaultLayout(new DockingWindowFactory(dockContents));
        }

        /// <summary>
        /// 显示窗口布局
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="dockContents"></param>
        public void ShowLayout(DockingWindowLayout layout, IEnumerable<DockContentEx> dockContents)
        {
            Contract.Requires(layout != null && dockContents != null);
            ShowLayout(layout, new DockingWindowFactory(dockContents));
        }

        /// <summary>
        /// 显示窗口布局
        /// </summary>
        /// <param name="layout"></param>
        public void ShowLayout(DockingWindowLayout layout, IDockingWindowFactory windowFactory = null)
        {
            Contract.Requires(layout != null);

            windowFactory = windowFactory ?? new DefaultDockingWindowFactory();
            _windows.ForEach(item => item.Value.Hide());

            _dockPanel.DockLeftPortion = layout.DockLeftPortion;
            _dockPanel.DockRightPortion = layout.DockRightPortion;
            _dockPanel.DockTopPortion = layout.DockTopPortion;
            _dockPanel.DockBottomPortion = layout.DockBottomPortion;

            List<KeyValuePair<DockingWindowLayoutState, DockContentEx>> items = new List<KeyValuePair<DockingWindowLayoutState, DockContentEx>>();
            foreach (DockingWindowLayoutState state in layout.GetWindowStates())
            {
                items.Add(new KeyValuePair<DockingWindowLayoutState, DockContentEx>(state, _CreateDockContent(state, windowFactory)));
            }

            foreach (var item in items.Where(item => item.Value != null))
            {
                _SetState(item.Key, item.Value);
            }
        }

        private void _SetState(DockingWindowLayoutState state, DockContentEx dce)
        {
            if (state.ParentUniqueId == null)
            {
                dce.Show(_dockPanel, state.DockState);
            }
            else
            {
                DockContentEx parent = _windows.GetOrDefault(state.ParentUniqueId);
                if (parent == null)
                    return;

                //dce.Show(parent, dce.DockState);
            }
        }

        private DockContentEx _CreateDockContent(DockingWindowLayoutState state, IDockingWindowFactory windowFactory)
        {
            try
            {
                DockContentEx dce = (state.UniqueId) == null ? null : _windows.GetOrDefault(state.UniqueId);
                if (dce == null)
                {
                    dce = windowFactory.CreateWindow(state.Type);
                    if (dce == null)
                        return null;

                    dce.SetLayoutState(state);
                }

                if (state.UniqueId != null && dce.GetUniqueId() != state.UniqueId)
                    throw new InvalidDataException("DockingContentEx唯一标识错误");

                state.UniqueId = dce.GetUniqueId();
                _windows[state.UniqueId] = dce;
                return dce;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取窗口布局
        /// </summary>
        public DockingWindowLayout GetLayout()
        {
            return new DockingWindowLayout(_windows.Values.Select(w => w.GetLayoutState()).ToArray());
        }
    }
}
