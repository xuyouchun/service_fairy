using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using System.Diagnostics.Contracts;
using Common.WinForm.Docking.DockingWindows;
using Common.Utility;
using Common.Contracts.Service;

namespace Common.WinForm
{
    /// <summary>
    /// 列表视图管理器
    /// </summary>
    public class ListViewWindowManager : MarshalByRefObjectEx, IListViewWindowManager
    {
        public ListViewWindowManager(Func<IListViewWindow> listViewCreator)
        {
            Contract.Requires(listViewCreator != null);

            _listViewCreator = listViewCreator;
        }

        public ListViewWindowManager()
            : this(() => new ListViewDockingWindow())
        {

        }

        private readonly Func<IListViewWindow> _listViewCreator;
        private readonly HashSet<IListViewWindow> _windows = new HashSet<IListViewWindow>();
        private IListViewWindow _currentWindow;

        public IListViewWindow GetCurrent(bool autoCreate = false)
        {
            if (_currentWindow == null)
            {
                if (_windows.Count > 0)
                    _currentWindow = _windows.First();
                else if (autoCreate)
                    _windows.Add(_currentWindow = _listViewCreator());
            }

            return _currentWindow;
        }

        public IListViewWindow CreateNew(bool setCurrent)
        {
            IListViewWindow window = _listViewCreator();
            _windows.Add(window);

            if (setCurrent)
                SetCurrent(window);

            return window;
        }

        public void SetCurrent(IListViewWindow window)
        {
            if (window == null)
            {
                _currentWindow = null;
            }
            else
            {
                if (!_windows.Contains(window))
                    _windows.Add(window);

                _currentWindow = window;
                _currentWindow.Activate();
            }

            CurrentChanged.RaiseEvent(this);
        }

        public void Close(IListViewWindow window)
        {
            Contract.Requires(window != null);

            _windows.Remove(window);
            if (_currentWindow == window)
                _currentWindow = _windows.FirstOrDefault();
        }

        public IListViewWindow[] GetAll()
        {
            return _windows.ToArray();
        }

        /// <summary>
        /// 当前列表窗口发生变化
        /// </summary>
        public event EventHandler CurrentChanged;
    }
}
