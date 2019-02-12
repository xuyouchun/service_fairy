using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.WinForm
{
    /// <summary>
    /// ToolStrip菜单项组
    /// </summary>
    public class ToolStripMenuItemGroup : IDisposable
    {
        public ToolStripMenuItemGroup(ToolStripMenuItem[] items, bool? clickOnCheck = true)
        {
            Contract.Requires(items != null);
            _items = items;
            _ApplyEvents(clickOnCheck);
        }

        public ToolStripMenuItemGroup(ToolStripMenuItem parent, bool? clickOnCheck = true)
        {
            Contract.Requires(parent != null);
            _items = parent.DropDownItems.OfType<ToolStripMenuItem>().ToArray();
            _ApplyEvents(clickOnCheck);
        }

        private void _ApplyEvents(bool? clickOnCheck)
        {
            foreach (ToolStripMenuItem item in _items)
            {
                item.CheckedChanged += new EventHandler(item_CheckedChanged);
                if (clickOnCheck != null)
                    item.CheckOnClick = (bool)clickOnCheck;

                if (item.Checked)
                    _current = item;
            }
        }

        private void _RemoveEvents()
        {
            foreach (ToolStripMenuItem item in _items)
            {
                item.CheckedChanged -= new EventHandler(item_CheckedChanged);
            }
        }

        private void item_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem curItem = (ToolStripMenuItem)sender;
            if (!curItem.Checked)
            {
                if (curItem == _current)
                    Current = null;

                return;
            }

            foreach (ToolStripMenuItem item in _items)
            {
                if (item != curItem)
                    item.Checked = false;
            }

            Current = curItem;
        }

        private readonly ToolStripMenuItem[] _items;

        public ToolStripMenuItem Current
        {
            get
            {
                return _current;
            }
            set
            {
                if (value != _current)
                {
                    _current = value;
                    CurrentChanged.RaiseEvent(this);
                }
            }
        }

        private volatile ToolStripMenuItem _current;

        /// <summary>
        /// 当前项改变
        /// </summary>
        public event EventHandler CurrentChanged;

        public void Dispose()
        {
            _RemoveEvents();
        }
    }
}
