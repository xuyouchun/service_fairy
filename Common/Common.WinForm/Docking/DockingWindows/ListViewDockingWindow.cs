using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts.UIObject;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using System.Collections;

namespace Common.WinForm.Docking.DockingWindows
{
    /// <summary>
    /// 列表显示控件
    /// </summary>
    public partial class ListViewDockingWindow : DockContentEx, IListViewWindow
    {
        public ListViewDockingWindow(IUIOperation operation = null)
        {
            InitializeComponent();
            _SetButtonStatus();

            _SetDefaultTimerInterval();
        }

        const int _pageSize = 50;
        const string TITLE = "TITLE_1006F127-34FE-46B6-AB45-F612EBE88534";
        private readonly IUIOperation _uiOperation;

        public void ShowList(IUIObjectExecuteContext executeContext, IListViewData data, bool isRefreshModel)
        {
            _ShowList(executeContext, data, !isRefreshModel, !isRefreshModel);
            _historyManager.Add(new HistoryItem(executeContext, data), isRefreshModel);
            _SetButtonStatus();
        }

        private ColumnHeaderEx[] _chex;

        private void _ShowList(IUIObjectExecuteContext executeContext, IListViewData data, bool clearSorter, bool clearHeaders)
        {
            Contract.Requires(data != null);

            try
            {
                base.SetWindowInfo(UIWindowInfo.FromServiceObject(data.Owner));

                _listView.SuspendLayout();
                int rowCount = _listView.Items.Count;
                _Clear(clearSorter, clearHeaders);

                // 创建列头及数据项
                _headers = new Dictionary<string, ColumnHeaderEx>();
                List<ListViewItem> items = new List<ListViewItem>();
                _headers.Add(TITLE, new ColumnHeaderEx() { Text = "名称", Weight = int.MinValue, Name = TITLE });
                foreach (IServiceObject so in data.GetRange(0, data.Count))
                {
                    items.Add(_CreateListViewItem(executeContext, so, _headers));
                }

                // 将列头排序并添加进去
                _chex = (!_chex.IsNullOrEmpty() && !clearHeaders && rowCount > 0) ? _chex : _headers.Values.OrderBy(v => v.Weight).ToArray();
                _listView.Columns.AddRange(_chex);

                // 计算各列合适的宽度，并调节各项的顺序
                _ReviseWidth(_chex, items);
                _ReviseSubItemsOrder(_chex, items);
                _listView.Items.AddRange(items.ToArray());

                // 上下文菜单
                if (data.Owner != null)
                    _rootContextMenuStrip = ServiceObjectContextMenuStrip.FromServiceObject(executeContext, data.Owner);
            }
            finally
            {
                _listView.ResumeLayout();
            }

            CurrentChanged.RaiseEvent(this);
        }

        private void _ReviseWidth(ColumnHeaderEx[] chs, List<ListViewItem> items)
        {
            using (Graphics g = Graphics.FromHwnd(Handle))
            {
                for (int k = 0; k < chs.Length; k++)
                {
                    chs[k].Width = _GetWidth(g, items, k,
                        k == 0 ? 150 : 60,
                        (k == 0) ? 1000 : (k < chs.Length - 1) ? 500 : 20000)
                      + ((k == 0 && _listView.SmallImageList != null) ? _listView.SmallImageList.ImageSize.Width + 5 : 5);
                }
            }
        }

        private int _GetWidth(Graphics g, List<ListViewItem> items, int index, int minWidth, int maxWidth = int.MaxValue)
        {
            if (items.Count == 0)
                return minWidth;

            int[] widths = new int[items.Count];
            for (int k = 0, count = Math.Min(60, items.Count); k < count; k++)
            {
                var subItems = items[k].SubItems;
                string s = ((index < subItems.Count) ? subItems[index].Text : "") ?? string.Empty;
                widths[k] = (int)g.MeasureString(s, Font).Width;
            }

            if (maxWidth == int.MaxValue)
                return Math.Max(minWidth, widths.Max());

            int width = minWidth;
            float rate = 1.0f;
            while (width < maxWidth)
            {
                if (((float)widths.Count(w => w <= width) / widths.Length) >= rate)
                    return width;

                width += 20;
                rate -= 0.005f;
            }

            return width;
        }

        private ServiceObjectContextMenuStrip _rootContextMenuStrip;
        private Dictionary<string, ColumnHeaderEx> _headers;

        private void _ReviseSubItemsOrder(ColumnHeaderEx[] chex, List<ListViewItem> lvItems)
        {
            int lineIndex = 0;
            foreach (ListViewItem lvItem in lvItems)
            {
                var dict = lvItem.SubItems.Cast<ListViewItem.ListViewSubItem>().ToDictionary(v => v.Name);
                lvItem.SubItems.Clear();

                for (int k = 0, length = chex.Length; k < length; k++)
                {
                    ColumnHeaderEx che = chex[k];
                    ListViewItem.ListViewSubItem subItem;
                    if (dict.TryGetValue(che.Name, out subItem))
                    {
                        if (che.Name == TITLE)
                        {
                            lvItem.SubItems[0].Text = subItem.Text;
                            lvItem.SubItems[0].Name = TITLE;
                            lvItem.SubItems[0].Tag = subItem.Tag;
                        }
                        else
                        {
                            lvItem.SubItems.Add(subItem);
                        }

                        ((ListViewSubItemTag)subItem.Tag).LineIndex = lineIndex;
                    }
                    else
                    {
                        lvItem.SubItems.Add(new ListViewItem.ListViewSubItem(lvItem, "")
                            { Name = che.Name, Tag = new ListViewSubItemTag { LineIndex = lineIndex } });
                    }
                }

                lineIndex++;
            }
        }

        private static string _GetFullName(ServiceObjectInfo info)
        {
            return info.Namespace + "/" + info.Name;
        }

        private static string _GetText(ServiceObjectInfo info)
        {
            return StringUtility.GetFirstNotNullOrWhiteSpaceString(info.Title, info.Name) ?? string.Empty;
        }

        private ColumnHeaderEx _CreateColumnHeader(ServiceObjectProperty property)
        {
            IUIObject uiObj = property.GetUIObject();
            ServiceObjectInfo info = property.Info;
            ColumnHeaderEx h = new ColumnHeaderEx() {
                Text = _GetText(info), Weight = info.Weight,
                Name = info.Name, TextAlign = HorizontalAlignment.Left
            };

            if (uiObj != null)
            {
                //h.ImageIndex = _AddImage(h.ImageList, uiObj);
            }

            return h;
        }

        class ColumnHeaderEx : ColumnHeader
        {
            public int Weight { get; set; }

            public SortOrder Order = SortOrder.None;
        }

        class ListViewSubItemTag
        {
            public object Value;
            public int LineIndex;
        }

        private static string _ToText(object obj)
        {
            string s = obj.ToStringIgnoreNull();
            return s.Replace("\r", "").Replace("\n", "");
        }

        private ListViewItem _CreateListViewItem(IUIObjectExecuteContext executeContext, IServiceObject so, Dictionary<string, ColumnHeaderEx> headers)
        {
            ListViewItem item = new ListViewItem(_GetText(so.Info));
            item.SubItems[0].Name = TITLE;
            item.SubItems[0].Tag = new ListViewSubItemTag() { Value = item.Text };

            foreach (ServiceObjectProperty property in so.GetProperties())
            {
                ServiceObjectInfo info = property.Info;
                headers.GetOrSet(info.Name, name => _CreateColumnHeader(property));

                object value = so.GetPropertyValue(property.Info.Name);
                ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(item, _ToText(value));
                subItem.Name = info.Name;
                subItem.Tag = new ListViewSubItemTag() { Value = value };
                item.SubItems.Add(subItem);
            }

            IUIObject uiObj = so.GetUIObject();
            Lazy<ServiceObjectContextMenuStrip> ms = null;
            if (uiObj != null)
            {
                _AddImage(_listView.LargeImageList, uiObj);
                item.ImageIndex = _AddImage(_listView.SmallImageList, uiObj);

                // 上下文菜单
                ms = new Lazy<ServiceObjectContextMenuStrip>(() => ServiceObjectContextMenuStrip.FromServiceObject(executeContext, so));
            }

            item.Tag = new ListItemTag(ms, so, executeContext, item, this);
            return item;
        }

        private int _AddImage(ImageList imgList, IUIObjectImageLoader imageLoader)
        {
            Image image = imageLoader.GetImageOrTransparent(imgList.ImageSize);
            Dictionary<Image, int> dict = imgList.Tag as Dictionary<Image, int>;
            if (dict == null)
                imgList.Tag = (dict = new Dictionary<Image, int>());

            int index;
            if (!dict.TryGetValue(image, out index))
            {
                imgList.Images.Add(image);
                index = imgList.Images.Count - 1;
                dict.Add(image, index);
            }

            return index;
        }

        public void Clear()
        {
            _Clear(true, true);
        }

        private void _Clear(bool clearSorter, bool clearHeaders)
        {
            foreach (ListViewItem listViewItem in _listView.Items)
            {
                IDisposable disable = listViewItem.Tag as IDisposable;
                if (disable != null)
                    disable.Dispose();
            }

            _listView.Items.Clear();
            _ClearImageList(_listView.LargeImageList);
            _ClearImageList(_listView.SmallImageList);
            _listView.Groups.Clear();
            _listView.Columns.Clear();

            _rootContextMenuStrip = null;

            if (clearHeaders)
                _headers = null;

            if (clearSorter)
            {
                _listView.ListViewItemSorter = null;
                _listView.Sorting = SortOrder.None;
            }
        }

        private void _ClearImageList(ImageList imgList)
        {
            imgList.Images.Clear();
            IDictionary dict = imgList.Tag as IDictionary;
            if (dict != null)
                dict.Clear();
        }

        private void ListViewDockingWindow_Load(object sender, EventArgs e)
        {
            _SetViewTypeCheckedState(_listView.View);
        }

        private void _ShowError(Exception ex, bool showDialog = true)
        {
            LogManager.LogError(ex);

            if (_uiOperation != null)
            {
                if (showDialog)
                    _uiOperation.ShowError(ex);
                else
                    _uiOperation.ShowPopup("", ex.Message, "", UIOperationPopupType.Error);
            }
            else
            {
                if (showDialog)
                    ErrorDialog.Show(this, ex);
            }
        }

        private void _SetViewTypeCheckedState(View view)
        {
            var items = _tsViewType.DropDownItems.OfType<ToolStripMenuItem>();
            ToolStripItem ts = items.First(item => (string)item.Tag == view.ToString());
            items.ForEach(item => item.Checked = (item == ts));
            _tsViewType.Text = ts.Text;
        }

        private void _tsViewType_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            View view = e.ClickedItem.Tag.ToType<View>(View.LargeIcon);
            _listView.View = view;
            _SetViewTypeCheckedState(view);
        }

        private ListItemTag _GetListItemTag(Point point)
        {
            ListViewItem lvItem = _listView.GetItemAt(point.X, point.Y);
            return lvItem == null ? null : lvItem.Tag as ListItemTag;
        }

        private ServiceObjectContextMenuStrip _GetMenuStrip(Point point)
        {
            ListItemTag itemTag = _GetListItemTag(point);
            return itemTag == null ? null : itemTag.MenuStrip.Value;
        }

        private void _listView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ServiceObjectContextMenuStrip ms = (Control.ModifierKeys == Keys.Shift) ? _rootContextMenuStrip :
                (_GetMenuStrip(e.Location) ?? _rootContextMenuStrip);

            if (ms != null)
            {
                ms.SetEnableState();
                ms.Show(_listView, e.Location);
            }
        }

        private void _listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _DoDefaultAction(_GetListItemTag(e.Location));
        }

        private void _DoDefaultAction(ListItemTag liTag)
        {
            IServiceObject so;
            if (liTag != null && (so = liTag.ServiceObject) != null)
            {
                so.ExecuteDefaultActionWithErrorDialog(liTag.ExecuteContext);
            }
        }

        private void ListViewDockingWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Clear(true, true);
        }

        public override bool ClosingNotify()
        {
            _Clear(true, true);
            return base.ClosingNotify();
        }

        const string AUTO_REFRESH_PREFIX = "AutoRefresh_";

        private ToolStripMenuItem[] _GetAllAutoRefreshMenuItems()
        {
            return _tsRefresh.DropDownItems.OfType<ToolStripMenuItem>()
                .Where(m => (((string)m.Tag) ?? string.Empty).StartsWith(AUTO_REFRESH_PREFIX)).ToArray();
        }

        /// <summary>
        /// 当前列表发生变化
        /// </summary>
        public event EventHandler CurrentChanged;

        private void _tsAutoRefresh_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem current = (ToolStripMenuItem)sender;

            int interval = int.Parse(((string)current.Tag).Substring(AUTO_REFRESH_PREFIX.Length));
            if (interval != _autoRefreshInterval)
                _SetTimerInterval(_autoRefreshInterval = interval);

            _SetAutoRefreshMenuItemState(interval);
        }

        private void _SetDefaultTimerInterval()
        {
            _SetTimerInterval(_autoRefreshInterval);
            _SetAutoRefreshMenuItemState(_autoRefreshInterval);
        }

        private void _SetAutoRefreshMenuItemState(int interval)
        {
            foreach (ToolStripMenuItem menuItem in _GetAllAutoRefreshMenuItems())
            {
                menuItem.Checked = int.Parse(((string)menuItem.Tag).Substring(AUTO_REFRESH_PREFIX.Length)) == interval;
            }
        }

        private void _SetTimerInterval(int interval)
        {
            if (interval == 0)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Interval = interval * 1000;
                _timer.Start();
            }
        }

        private static int _autoRefreshInterval = 300;  // 之后打开的窗口都使用该值

        private void _tsRefresh_ButtonClick(object sender, EventArgs e)
        {
            _RefreshAndResetTimer();
        }

        private void _RefreshAndResetTimer()
        {
            _Refresh();

            if (_timer.Enabled)
            {
                _timer.Stop();
                _timer.Start();  // 重新计时
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            try
            {
                _timer.Stop();
                _Refresh(false);
            }
            finally
            {
                _timer.Start();
            }
        }

        private void _listView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)  // 回车键
            {
                if (_listView.SelectedItems.Count == 0)
                    return;

                _DoDefaultAction(_listView.SelectedItems[0].Tag as ListItemTag);
            }
            else if (e.KeyCode == Keys.Back || (e.Alt && e.KeyCode == Keys.Left)) // 退格键或Alt+左箭头
            {
                _GoBack();
            }
            else if (e.Alt && e.KeyCode == Keys.Right)  // Alt+右箭头
            {
                _GoForward();
            }
            else if (e.KeyCode == Keys.F5)
            {
                _RefreshAndResetTimer();
            }
        }

        private void _listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            HistoryItem hi = _historyManager.GetCurrent();
            if (hi != null)
            {
                hi.SelectedIndex = (e.IsSelected ? e.ItemIndex : -1);
            }
        }

        /// <summary>
        /// 点击列头排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int columnIndex = e.Column;
            ColumnHeaderEx h = _listView.Columns[columnIndex] as ColumnHeaderEx;

            try
            {
                h.Order = h.Order != SortOrder.Descending ? SortOrder.Descending : SortOrder.Ascending;
                _listView.Columns.Cast<ColumnHeaderEx>().Where(ch => ch != h).ForEach(ch => ch.Order = SortOrder.None);
                //_listView.Sorting = h.Order;
                _listView.ListViewItemSorter = new ListViewItemComparer(e.Column, h.Order);
                _listView.Sort();
            }
            catch (Exception ex)
            {
                _ShowError(ex);
            }
        }

        #region Class ListViewItemComparer ...

        class ListViewItemComparer : IComparer<ListViewItem>, IComparer
        {
            public ListViewItemComparer(int columnIndex, SortOrder sortOrder)
            {
                _columnIndex = columnIndex;
                _sortOrder = sortOrder;
            }

            private readonly int _columnIndex;
            private readonly SortOrder _sortOrder;

            private int _Compare(object vx, object vy)
            {
                if (vx == null || vy == null)
                {
                    return vx == null ? (vy == null ? 0 : -1) : 1;
                }

                if (vx is IComparable && vy is IComparable)
                {
                    return ((IComparable)vx).CompareTo(vy);
                }

                return 0;
            }

            public int Compare(ListViewItem x, ListViewItem y)
            {
                ListViewSubItemTag tagx = x.SubItems[_columnIndex].Tag as ListViewSubItemTag;
                ListViewSubItemTag tagy = y.SubItems[_columnIndex].Tag as ListViewSubItemTag;

                if (_sortOrder == SortOrder.None)
                    return tagx.LineIndex.CompareTo(tagy.LineIndex);

                int result = _Compare(tagx.Value, tagy.Value);
                return (_sortOrder == SortOrder.Descending) ? -result : result;
            }

            public int Compare(object x, object y)
            {
                return Compare((ListViewItem)x, (ListViewItem)y);
            }
        }

        #endregion
    }
}
