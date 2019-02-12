using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WinForm;
using System.Windows.Forms;
using System.Data;
using WeifenLuo.WinFormsUI.Docking;
using Common.Package.Storage;
using Common.Contracts;
using System.ComponentModel;
using Common.Utility;
using System.Drawing;

namespace StreamTableViewer.Actions
{
    class StTableNodeUIAction : NodeActionUIBase
    {
        public StTableNodeUIAction(ViewerContext context, TreeViewUIActionManager uiActionManager, TreeNode node, StreamTableReader reader, StreamTable table)
            : base(context, uiActionManager, node)
        {
            _table = table;
            _reader = reader;

            node.ContextMenuStrip = _CreateContextMenuStrip();
        }

        private readonly StreamTable _table;
        private readonly StreamTableReader _reader;

        public StreamTable StreamTable
        {
            get { return _table; }
        }

        public override void OnAction(UIActionType actionType, object sender, EventArgs e)
        {
            switch (actionType)
            {
                case UIActionType.TreeNodeMouseDoubleClick:
                    _ShowTable(true);
                    break;

                case UIActionType.TreeNodeMouseClick:
                    _ShowTable(false);
                    break;

                case UIActionType.Close:
                    _Close();
                    break;
            }

            base.OnAction(actionType, sender, e);
        }

        private TableViewerDockingWindow _window;

        public bool Opened
        {
            get { return _window != null; }
        }

        /// <summary>
        /// 状态变化
        /// </summary>
        public event EventHandler StateChanged;

        private void _RaiseStateChangedEvent()
        {
            var eh = StateChanged;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        private void _ShowTable(bool autoCreate)
        {
            if (_window == null)
            {
                if (!autoCreate)
                    return;

                _window = new TableViewerDockingWindow(Context);
                _window.Show(Context.MainWindow.DockPanel, DockState.Document);
                _window.FormClosed += new FormClosedEventHandler(_window_FormClosed);
                _window.Text = _table.Name;
                _window.SetData(_table);

                Node.NodeFont = new Font(Node.TreeView.Font, FontStyle.Bold);
                _window.Tag = this;

                _RaiseStateChangedEvent();
            }
            else
            {
                _window.Activate();
            }
        }

        void _window_FormClosed(object sender, FormClosedEventArgs e)
        {
            _window = null;
            Node.NodeFont = Node.TreeView.Font;
            _RaiseStateChangedEvent();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        private void _Close()
        {
            if (_window != null)
                _window.Close();
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            _ShowTable(true);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            _Close();
        }

        protected override object OnGetPropertyObject()
        {
            return new Property(_reader, _table);
        }

        #region Class Property ...

        class Property
        {
            public Property(StreamTableReader reader, StreamTable table)
            {
                _reader = reader;
                _table = table;

                Columns = table.Columns.SelectFromArray(c => new ColumnProperty(c));
            }

            private static string _GetElementCount(StreamTableColumn c)
            {
                switch (c.StorageModel)
                {
                    case StreamTableColumnStorageModel.Element:
                        return "单个元素";

                    case StreamTableColumnStorageModel.Array:
                        return c.ElementCount.ToString();

                    case StreamTableColumnStorageModel.DynamicArray:
                        return "变长数组";
                }

                return "";
            }

            private readonly StreamTableReader _reader;
            private readonly StreamTable _table;

            [Browsable(true), DisplayName("名称")]
            public string Name { get { return _table.Name; } }

            [Browsable(true), DisplayName("行数")]
            public int RowCount { get { return _table.RowCount; } }

            [Browsable(true), DisplayName("列数")]
            public int ColumnCount { get { return _table.Columns.Length; } }

            [Browsable(true), DisplayName("列信息"), EditorBrowsable(EditorBrowsableState.Advanced)]
            public ColumnProperty[] Columns { get; private set; }

            [Browsable(true), DisplayName("备注")]
            public string Desc { get { return _table.Desc; } }
        }

        #endregion

        #region Class ColumnProperty ...

        [EditorBrowsable]
        class ColumnProperty
        {
            public ColumnProperty(StreamTableColumn column)
            {
                _column = column;

                Name = column.Name;
                Type = column.ColumnType.GetDesc();
                StorageModel = column.StorageModel.GetDesc();
                ElementCount = column.ElementCount;
                Desc = column.Desc;
            }

            private readonly StreamTableColumn _column;

            [Browsable(true), DisplayName("列名")]
            public string Name { get; private set; }

            [Browsable(true), DisplayName("数据类型")]
            public string Type { get; private set; }

            [Browsable(true), DisplayName("数据组织类型")]
            public string StorageModel { get; private set; }

            [Browsable(true), DisplayName("数组长度")]
            public int ElementCount { get; private set; }

            [Browsable(true), DisplayName("备注")]
            public string Desc { get; private set; }

            public override string ToString()
            {
                return _column.ToString();
            }
        }

        #endregion

        private ContextMenuStrip _CreateContextMenuStrip()
        {
            ContextMenuStrip ms = new ContextMenuStrip();
            ToolStripMenuItem ts = new ToolStripMenuItem("关闭", null, delegate { _Close(); });
            ms.Items.Add(ts);

            return ms;
        }
    }
}
