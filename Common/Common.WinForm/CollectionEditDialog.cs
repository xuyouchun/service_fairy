using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Utility;

namespace Common.WinForm
{
    public partial class CollectionEditDialog : XDialog
    {
        private CollectionEditDialog(object[] items, string title,
            Func<CollectionEditDialogContext, object> onAdd, Func<CollectionEditDialogContext, bool> onRemove,
            Func<CollectionEditDialogContext, object> onEdit, object state)
        {
            InitializeComponent();

            Text = title;
            _onAdd = onAdd;
            _onRemove = onRemove;
            _onEdit = onEdit;
            _state = state;

            if (!items.IsNullOrEmpty())
                _listbox.Items.AddRange(items);

            _SetButtonState();
        }

        private readonly Func<CollectionEditDialogContext, object> _onAdd;
        private readonly Func<CollectionEditDialogContext, bool> _onRemove;
        private readonly Func<CollectionEditDialogContext, object> _onEdit;
        private readonly object _state;

        /// <summary>
        /// 获取当前的对象集合
        /// </summary>
        /// <returns></returns>
        public object[] GetCurrentList()
        {
            return _listbox.Items.OfType<object>().ToArray();
        }

        private void _listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SetButtonState();   
        }

        private void _SetButtonState()
        {
            _ctlAdd.Enabled = (_onAdd != null);
            _ctlRemove.Enabled = (_onRemove != null && _listbox.SelectedItems.Count > 0);
            _ctlEdit.Enabled = (_onEdit != null && _listbox.SelectedItems.Count > 0);
        }

        private CollectionEditDialogContext _GetContext(CollectionEditDialogEditType editType)
        {
            return new CollectionEditDialogContext(this,
                _listbox.SelectedItems.OfType<object>().ToArray(),
                _listbox.SelectedIndex < 0 ? null : _listbox.Items[_listbox.SelectedIndex],
                _listbox.Items.OfType<object>().ToArray(), _state, editType
            );
        }

        private void _ctlAdd_Click(object sender, EventArgs e)
        {
            if (_onAdd == null)
                return;

            object obj = _onAdd(_GetContext(CollectionEditDialogEditType.Add));
            if (obj != null)
            {
                _listbox.Items.Add(obj);
            }
        }

        private void _ctlRemove_Click(object sender, EventArgs e)
        {
            CollectionEditDialogContext ctx = _GetContext(CollectionEditDialogEditType.Remove);
            if (_onRemove == null || ctx.SelectedItems.Length == 0)
                return;

            if (_onRemove(ctx))
            {
                foreach (var item in ctx.SelectedItems)
                {
                    _listbox.Items.Remove(item);
                }
            }
        }

        private void _ctlEdit_Click(object sender, EventArgs e)
        {
            CollectionEditDialogContext ctx = _GetContext(CollectionEditDialogEditType.Edit);
            if (_onEdit == null || ctx.FocusedItem == null)
                return;

            try
            {
                _listbox.BeginUpdate();
                object newObj = _onEdit(ctx);
                if (newObj != null)
                    _listbox.Items[_listbox.SelectedIndex] = newObj;
            }
            finally
            {
                _listbox.EndUpdate();
            }
        }

        private void _listbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Y <= _listbox.ItemHeight * (_listbox.Items.Count - _listbox.TopIndex))
                _ctlEdit_Click(sender, e);
        }

        /// <summary>
        /// 显示集合编辑对话框
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="items"></param>
        /// <param name="title"></param>
        /// <param name="onAdd"></param>
        /// <param name="onRemove"></param>
        /// <param name="onEdit"></param>
        /// <returns></returns>
        public static object[] Show(IWin32Window owner, object[] items, string title = "",
            Func<CollectionEditDialogContext, object> onAdd = null, Func<CollectionEditDialogContext, bool> onRemove = null,
            Func<CollectionEditDialogContext, object> onEdit = null, SelectionMode selectionMode = SelectionMode.One, object state = null)
        {
            return _Show(owner, items, title, onAdd, onRemove, onEdit, selectionMode);
        }

        private static object[] _Show(IWin32Window owner, object[] items, string title = "",
            Func<CollectionEditDialogContext, object> onAdd = null, Func<CollectionEditDialogContext, bool> onRemove = null,
            Func<CollectionEditDialogContext, object> onEdit = null, SelectionMode selectionMode = SelectionMode.One, object state = null)
        {
            CollectionEditDialog dlg = new CollectionEditDialog(items, title, onAdd, onRemove, onEdit, state);
            if (dlg.ShowDialog(owner) == DialogResult.OK)
                return dlg.GetCurrentList();

            return null;
        }

        /// <summary>
        /// 显示集合编辑对话框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="owner"></param>
        /// <param name="items"></param>
        /// <param name="title"></param>
        /// <param name="onAdd"></param>
        /// <param name="onRemove"></param>
        /// <param name="onEdit"></param>
        /// <returns></returns>
        public static T[] Show<T>(IWin32Window owner, T[] items, string title = "",
            Func<CollectionEditDialogContext<T>, T> onAdd = null,
            Func<CollectionEditDialogContext<T>, bool> onRemove = null,
            Func<CollectionEditDialogContext<T>, T> onEdit = null, SelectionMode selectionMode = SelectionMode.One, object state = null)
        {
            object[] list = _Show(owner, items == null ? null : items.Cast<object>(), title,
                onAdd == null ? (Func<CollectionEditDialogContext, object>)null : (ctx) => onAdd(ctx.To<T>()),
                onRemove == null ? (Func<CollectionEditDialogContext, bool>)null : (ctx) => onRemove(ctx.To<T>()),
                onEdit == null ? (Func<CollectionEditDialogContext, object>)null : (ctx) => onEdit(ctx.To<T>()),
                selectionMode, state
            );

            return list == null ? null : list.Cast<T>();
        }
    }
}
