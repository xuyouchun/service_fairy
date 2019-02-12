using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar.Controls;
using System.Windows.Forms;
using System.Drawing;

namespace Common.WinForm.Docking.DockingWindows
{
    class MyListView : ListView
    {
        public MyListView()
        {
            OwnerDraw = true;
            DoubleBuffered = true;
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            if (View != System.Windows.Forms.View.Details)
            {
                e.DrawDefault = true;
                base.OnDrawSubItem(e);
                return;
            }

            int offset = ((SmallImageList == null || e.ColumnIndex > 0) ? 2 : SmallImageList.ImageSize.Width + 5);
            Rectangle rect = e.Bounds;
            rect = new Rectangle() { X = rect.X + offset, Y = rect.Y, Width = rect.Width - offset, Height = rect.Height };
            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, Font, rect, _GetColor(e.ItemState), flags);
        }

        private Color _GetColor(ListViewItemStates states)
        {
            return (states & ListViewItemStates.Selected) != 0 ? Color.Black : Color.Black;
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            if (View != System.Windows.Forms.View.Details)
            {
                e.DrawDefault = true;
                base.OnDrawItem(e);
                return;
            }

            if ((e.State & ListViewItemStates.Selected) != 0)
                e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds);

            /*
            if ((e.State & ListViewItemStates.Focused) != 0)
                e.DrawFocusRectangle();*/

            Image img = _GetImage(e);
            if (img != null)
                e.Graphics.DrawImage(img, e.Bounds.X + 3, e.Bounds.Y);
        }

        private Image _GetImage(DrawListViewItemEventArgs e)
        {
            if (SmallImageList.Images == null || e.Item.ImageIndex >= SmallImageList.Images.Count)
                return null;

            return SmallImageList.Images[e.Item.ImageIndex];
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            base.OnGiveFeedback(gfbevent);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            /*
            ListViewItem item = this.GetItemAt(e.X, e.Y);
            if (item != null)
                _dragContext = new DragContext() { Item = item };*/
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _dragContext = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_dragContext != null)
            {
                this.DoDragDrop(_dragContext, DragDropEffects.Move);
            }
        }

        class DragContext
        {
            public ListViewItem Item { get; set; }
        }

        private volatile DragContext _dragContext;
    }
}
