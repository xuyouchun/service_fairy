using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Windows.Forms;

namespace Common.WinForm
{
    public class CollectionEditDialogContext
    {
        internal CollectionEditDialogContext(IWin32Window window, object[] selectedItems, object focusedItem, object[] allItems, object state, CollectionEditDialogEditType editType)
        {
            Window = window;
            SelectedItems = selectedItems;
            FocusedItem = focusedItem;
            AllItems = allItems;
            State = state;
            EditType = editType;
        }

        /// <summary>
        /// 窗口
        /// </summary>
        public IWin32Window Window { get; private set; }

        /// <summary>
        /// 已选择的项
        /// </summary>
        public object[] SelectedItems { get; private set; }

        /// <summary>
        /// 当前具有焦点的项
        /// </summary>
        public object FocusedItem { get; private set; }

        /// <summary>
        /// 所有项
        /// </summary>
        public object[] AllItems { get; private set; }

        /// <summary>
        /// 附带的数据
        /// </summary>
        public object State { get; private set; }

        /// <summary>
        /// 编辑类型
        /// </summary>
        public CollectionEditDialogEditType EditType { get; private set; }

        /// <summary>
        /// 是否包含指定的项
        /// </summary>
        /// <param name="excludeFocusedItem"></param>
        /// <returns></returns>
        public bool Contains(object item, bool? excludeFocusedItem = null)
        {
            if (excludeFocusedItem == null)
                excludeFocusedItem = (EditType == CollectionEditDialogEditType.Edit);

            return AllItems.Any(item0 => object.Equals(item, item0) && !((bool)excludeFocusedItem && object.Equals(item0, FocusedItem)));
        }

        internal CollectionEditDialogContext<T> To<T>()
        {
            return new CollectionEditDialogContext<T>(Window, SelectedItems, FocusedItem, AllItems, State, EditType);
        }
    }

    public class CollectionEditDialogContext<T> : CollectionEditDialogContext
    {
        internal CollectionEditDialogContext(IWin32Window window, object[] selectedItems, object focusedItem, object[] allItems, object state, CollectionEditDialogEditType editType)
            : base(window, selectedItems, focusedItem, allItems, state, editType)
        {
            SelectedItems = selectedItems.Cast<T>();
            FocusedItem = (T)focusedItem;
            AllItems = allItems.Cast<T>();
        }

        /// <summary>
        /// 已选择的项
        /// </summary>
        public new T[] SelectedItems { get; private set; }

        /// <summary>
        /// 当前具有焦点的项
        /// </summary>
        public new T FocusedItem { get; private set; }

        /// <summary>
        /// 所有项
        /// </summary>
        public new T[] AllItems { get; private set; }

        /// <summary>
        /// 是否包含指定的项目
        /// </summary>
        /// <param name="item"></param>
        /// <param name="excludeFocusedItem"></param>
        /// <returns></returns>
        public bool Contains(T item, bool? excludeFocusedItem = null)
        {
            return base.Contains(item, excludeFocusedItem);
        }
    }

    public enum CollectionEditDialogEditType
    {
        Add,

        Remove,

        Edit
    }
}
