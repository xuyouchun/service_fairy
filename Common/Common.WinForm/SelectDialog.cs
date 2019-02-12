using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using Common.Contracts.UIObject;

namespace Common.WinForm
{
    public partial class SelectDialog : XDialog
    {
        public SelectDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 批量添加项
        /// </summary>
        /// <param name="items"></param>
        public void AddItems(object[] items)
        {
            Contract.Requires(items != null);
            _lbList.Items.AddRange(items);
            _ctlSelectAll.UpdateState();
        }

        /// <summary>
        /// 批量删除项
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(object item)
        {
            if (item != null)
            {
                _lbList.Items.Remove(item);
                _ctlSelectAll.UpdateState();
            }
        }

        /// <summary>
        /// 选择项
        /// </summary>
        /// <param name="item"></param>
        public void Select(object item)
        {
            if (item != null)
                _lbList.SelectedItems.Add(item);
        }

        /// <summary>
        /// 选择全部
        /// </summary>
        public void SelectAll()
        {
            foreach (object item in _lbList.Items)
            {
                _lbList.SelectedItems.Add(item);
            }
        }

        /// <summary>
        /// 获取所有选中的项
        /// </summary>
        /// <returns></returns>
        private object[] GetSelectedItems()
        {
            return _lbList.SelectedItems.Cast<object>().ToArray();
        }

        /// <summary>
        /// 选择方式
        /// </summary>
        public SelectionMode SelectionMode
        {
            get { return _lbList.SelectionMode; }
            set { _lbList.SelectionMode = value; }
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="owner"></param>
        /// <param name="items"></param>
        /// <param name="caption"></param>
        /// <param name="selectionMode"></param>
        /// <returns></returns>
        public static T[] Show<T>(IWin32Window owner, T[] items, string caption = "", SelectionMode selectionMode = SelectionMode.One)
        {
            SelectDialog dlg = new SelectDialog();
            dlg.Text = caption ?? "";
            dlg.SelectionMode = selectionMode;
            dlg.AddItems((items ?? new T[0]).Cast<object>().ToArray());

            if (dlg.ShowDialog(owner) == DialogResult.OK)
                return dlg.GetSelectedItems().OfType<T>().ToArray();

            return null;
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="owner"></param>
        /// <param name="items"></param>
        /// <param name="selectionMode"></param>
        /// <returns></returns>
        public static T[] Show<T>(IWin32Window owner, T[] items, SelectionMode selectionMode)
        {
            return Show<T>(owner, items, "", selectionMode);
        }

        private void _lbList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.Items.Count * lb.ItemHeight >= e.Y)  // 在条目上双击时，会关闭窗口
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void _lbList_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (e.Index < 0 || e.Index >= lb.Items.Count)
                return;

            object obj = lb.Items[e.Index];

            if ((e.State & DrawItemState.Selected) != 0)
                e.DrawBackground();
            else
                e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);

            if ((e.State & DrawItemState.Focus) != 0)
                e.DrawFocusRectangle();

            int width = lb.ItemHeight, height = lb.ItemHeight;
            IUIObjectImageLoader uiObj = obj as IUIObjectImageLoader;
            if (uiObj != null)
            {
                e.Graphics.DrawImage(uiObj.GetImage(new Size(width, height)).ToImage(), e.Bounds.Location);
            }

            Color color = (e.State & DrawItemState.Selected) != 0 ? Color.White : e.ForeColor;
            Rectangle rect = new Rectangle(e.Bounds.Left + width + 2, e.Bounds.Top, e.Bounds.Width - width - 2, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, obj.ToString(), e.Font, rect, color, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }
    }
}
