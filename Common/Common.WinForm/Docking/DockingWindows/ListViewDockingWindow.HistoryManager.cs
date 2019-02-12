using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using Common.Contracts;
using System.Windows.Forms;
using System.Drawing;

namespace Common.WinForm.Docking.DockingWindows
{
	partial class ListViewDockingWindow
	{
        #region Class HistoryItem ...

        class HistoryItem : MarshalByRefObjectEx
        {
            public HistoryItem(IUIObjectExecuteContext executeContext, IListViewData data)
            {
                ExecuteContext = executeContext;
                Data = data;
                WindowInfo = UIWindowInfo.FromServiceObject(data.Owner);
                SelectedIndex = -1;
            }

            public IUIObjectExecuteContext ExecuteContext { get; private set; }
            public IListViewData Data { get; private set; }
            public UIWindowInfo WindowInfo { get; private set; }
            public int SelectedIndex { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public override string ToString()
            {
                return WindowInfo.Title;
            }

            public void CopyFrom(HistoryItem hi)
            {
                ExecuteContext = hi.ExecuteContext;
                Data = hi.Data;
                WindowInfo = hi.WindowInfo;
            }
        }

        #endregion

        #region Class HistoryManager ...

        class HistoryManager : MarshalByRefObjectEx
        {
            public HistoryManager(int maxCount)
            {
                _maxCount = maxCount;
            }

            private List<HistoryItem> _historyItems = new List<HistoryItem>();

            private int _historyIndex = -1, _maxCount;

            public HistoryItem Forward()
            {
                if (_historyItems.Count == 0)
                    return null;

                if (_historyIndex < _historyItems.Count - 1)
                    _historyIndex++;

                return _historyItems[_historyIndex];
            }

            public HistoryItem[] GetForwardList()
            {
                if (_historyItems.Count == 0)
                    return new HistoryItem[0];

                return _historyItems.Range(_historyIndex + 1);
            }

            public bool AtHome()
            {
                return _historyIndex <= 0;
            }

            public HistoryItem[] GetBackList()
            {
                if (_historyItems.Count == 0)
                    return new HistoryItem[0];

                HistoryItem[] items = _historyItems.Range(0, _historyIndex);
                Array.Reverse(items);
                return items;
            }

            public HistoryItem Back()
            {
                if (_historyItems.Count == 0)
                    return null;

                if (_historyIndex > 0)
                    _historyIndex--;

                return _historyItems[_historyIndex];
            }

            public bool AtEnd()
            {
                return _historyIndex >= _historyItems.Count - 1;
            }

            public void Add(HistoryItem historyItem, bool isRefresh)
            {
                HistoryItem hi;
                if (!isRefresh || (hi = GetCurrent()) == null)
                {
                    if (_historyItems.Count >= _maxCount)
                    {
                        _historyItems.RemoveAt(0);
                        _historyIndex--;
                    }

                    _historyItems.RemoveRange(_historyIndex + 1, (_historyItems.Count - (_historyIndex + 1)));
                    _historyItems.Add(historyItem);
                    _historyIndex = _historyItems.Count - 1;
                }
                else
                {
                    hi.CopyFrom(historyItem);
                }
            }

            public void SetCurrent(HistoryItem historyItem)
            {
                _historyIndex = _historyItems.IndexOf(historyItem);
            }

            public HistoryItem GetCurrent()
            {
                if (_historyItems.Count == 0)
                    return null;

                return _historyItems[_historyIndex];
            }
        }

        #endregion

        private readonly HistoryManager _historyManager = new HistoryManager(15);

        private void _ShowList(HistoryItem historyItem, bool refresh = false, bool showErrorDialog = true)
        {
            try
            {
                if (refresh)
                    historyItem.Data.Refresh();

                _ShowList(historyItem.ExecuteContext, historyItem.Data, !refresh, !refresh);
                int selectedIndex = historyItem.SelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < _listView.Items.Count)
                {
                    _listView.Items[selectedIndex].Selected = true;
                    _listView.FocusedItem = _listView.Items[selectedIndex];

                    _listView.EnsureVisible(selectedIndex);
                }
            }
            catch (Exception ex)
            {
                _ShowError(ex, showErrorDialog);
            }
        }

        private void _tsBack_Click(object sender, EventArgs e)
        {
            _GoBack();
        }

        private void _GoBack()
        {
            if (_historyManager.AtHome())
                return;

            HistoryItem historyItem = _historyManager.Back();
            if (historyItem != null)
                _ShowList(historyItem, false);

            _SetButtonStatus();
        }

        private void _SetButtonStatus()
        {
            HistoryItem hi;

            _tsBack.Enabled = !_historyManager.AtHome();
            _tsForward.Enabled = !_historyManager.AtEnd();
            _tsRefresh.Enabled = (_historyManager.GetCurrent() != null);
            _tsUp.Enabled = (hi = _historyManager.GetCurrent()) != null && hi.Data.HasParent;
        }

        private void _tsForward_Click(object sender, EventArgs e)
        {
            _GoForward();
        }

        private void _GoForward()
        {
            if (_historyManager.AtEnd())
                return;

            HistoryItem historyItem = _historyManager.Forward();
            if (historyItem != null)
                _ShowList(historyItem, false);

            _SetButtonStatus();
        }

        private void _tsBack_DropDownOpening(object sender, EventArgs e)
        {
            _ShowHistoryMenuItems(_tsBack, _historyManager.GetBackList());
        }

        private void _ShowHistoryMenuItems(ToolStripSplitButton splitButton, HistoryItem[] historyItems)
        {
            splitButton.DropDownItems.Clear();
            foreach (HistoryItem historyItem in historyItems)
            {
                HistoryItem hs = historyItem;
                Image image = hs.WindowInfo.ImageLoader.GetImageOrTransparent(16);
                ToolStripMenuItem menuItem = new ToolStripMenuItem(hs.ToString(), image);
                splitButton.DropDownItems.Add(menuItem);
                menuItem.Click += delegate(object sender, EventArgs e) {
                    _historyManager.SetCurrent(hs);
                    _ShowList(hs, false);
                    _SetButtonStatus();
                };
            }
        }

        private void _tsForward_DropDownOpening(object sender, EventArgs e)
        {
            _ShowHistoryMenuItems(_tsForward, _historyManager.GetForwardList());
        }

        private void _tsUp_Click(object sender, EventArgs e)
        {
            HistoryItem hs = _historyManager.GetCurrent();
            IListViewData data;
            if (hs == null || !hs.Data.HasParent || (data = hs.Data.GetParentData()) == null)
                return;

            ShowList(hs.ExecuteContext, data, false);
        }

        private void _Refresh(bool showError = true)
        {
            HistoryItem hs = _historyManager.GetCurrent();
            if (hs != null)
            {
                if (_listView.SelectedIndices.Count > 0)
                {
                    int index = _listView.SelectedIndices[0];
                    int topIndex = _listView.View == View.Details ? _listView.TopItem.Index : 0;
                    _ShowList(hs, true, showError);
                    if (index < _listView.Items.Count)
                    {
                        _listView.Items[index].Selected = true;
                        _listView.FocusedItem = _listView.Items[index];
                        _listView.EnsureVisible(index);

                        if (_listView.View == View.Details && topIndex < _listView.Items.Count)
                            _listView.TopItem = _listView.Items[topIndex];
                    }
                }
                else
                {
                    _ShowList(hs, true, showError);
                }
            }
        }

        /// <summary>
        /// 事件
        /// </summary>
        public event EventHandler<ListViewWindowActionEventArgs> OnAction;

        private void _RaiseOnEvent(ListViewWindowActionType actionType)
        {
            var eh = OnAction;
            if (eh != null)
            {
                HistoryItem hi = _historyManager.GetCurrent();
                if (hi == null)
                    eh(this, new ListViewWindowActionEventArgs(null, null, actionType));
                else
                    eh(this, new ListViewWindowActionEventArgs(hi.ExecuteContext, hi.Data, actionType));
            }
        }

        /// <summary>
        /// 获了当前的数据列表
        /// </summary>
        /// <returns></returns>
        public IListViewData GetCurrent()
        {
            HistoryItem hi = _historyManager.GetCurrent();
            return hi == null ? null : hi.Data;
        }
	}
}
