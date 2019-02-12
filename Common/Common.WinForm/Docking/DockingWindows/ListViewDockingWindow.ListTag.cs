using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.Package;

namespace Common.WinForm.Docking.DockingWindows
{
	partial class ListViewDockingWindow
	{
        class ListItemTag : MarshalByRefObjectEx, IDisposable
        {
            public ListItemTag(Lazy<ServiceObjectContextMenuStrip> menuStrip,
                IServiceObject serviceObject, IUIObjectExecuteContext executeContext, ListViewItem listViewItem, ListViewDockingWindow window)
            {
                MenuStrip = menuStrip;
                ServiceObject = serviceObject;
                ExecuteContext = executeContext;
                ListViewItem = listViewItem;
                _window = window;

                _BindEvents(_refreshable = serviceObject as IRefreshableServiceObject);
            }

            private void _BindEvents(IRefreshableServiceObject refreshable)
            {
                if (refreshable != null)
                {
                    refreshable.Error += new ErrorEventHandler(refreshable_Error);
                    refreshable.Refresh += new EventHandler(refreshable_Refresh);
                }
            }

            private void refreshable_Refresh(object sender, EventArgs e)
            {
                if (ListViewItem.ListView == null)
                    return;

                ListViewItem.ListView.BeginInvoke(delegate {
                    if (_refreshable.IsDisposed)
                    {
                        ListViewItem.Remove();
                        Dispose();
                    }
                    else
                    {
                        _Refresh();
                    }
                });
            }

            private void refreshable_Error(object sender, ErrorEventArgs e)
            {
                
            }

            private void _UnbindEvents(IRefreshableServiceObject refreshable)
            {
                if (refreshable != null)
                {
                    refreshable.Error -= new ErrorEventHandler(refreshable_Error);
                    refreshable.Refresh -= new EventHandler(refreshable_Refresh);
                }
            }

            private void _Refresh()
            {
                if (_window._headers == null)
                    return;

                ListView listView = ListViewItem.ListView;
                try
                {
                    if (listView != null)
                        listView.SuspendLayout();

                    foreach (KeyValuePair<string, ColumnHeaderEx> header in _window._headers)
                    {
                        var subItem = ListViewItem.SubItems[header.Key];
                        if (subItem == null)
                            continue;

                        if (subItem.Name == TITLE)
                        {
                            subItem.Text = _GetText(ServiceObject.Info);
                        }
                        else
                        {
                            object value = ServiceObject.GetPropertyValue(subItem.Name);
                            if (value != null)
                                subItem.Text = value.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
                finally
                {
                    if (listView != null)
                        listView.ResumeLayout();
                }
            }

            private IRefreshableServiceObject _refreshable;

            public Lazy<ServiceObjectContextMenuStrip> MenuStrip { get; private set; }

            public IServiceObject ServiceObject { get; private set; }

            public IUIObjectExecuteContext ExecuteContext { get; private set; }

            public ListViewItem ListViewItem { get; private set; }

            private readonly ListViewDockingWindow _window;

            public void Dispose()
            {
                _UnbindEvents(_refreshable);
            }
        }
	}
}
