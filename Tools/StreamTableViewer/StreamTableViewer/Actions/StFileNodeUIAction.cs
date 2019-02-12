using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WinForm;
using System.Windows.Forms;
using Common.Package.Storage;
using System.Data;
using System.ComponentModel;
using Common.Contracts;
using System.Drawing;

namespace StreamTableViewer.Actions
{
    /// <summary>
    /// 文件节点
    /// </summary>
    class StFileNodeUIAction : NodeActionUIBase
    {
        public StFileNodeUIAction(ViewerContext context, TreeViewUIActionManager treeviewUIActionManager, TreeNode node, string file)
            : base(context, treeviewUIActionManager, node)
        {
            _context = context;
            File = file;
            node.ContextMenuStrip = _CreateContextMenuStrip();

            node.Nodes.Add(new TreeNode("正在打开 ..."));
        }

        private readonly ViewerContext _context;
        private bool _opened = false;
        private Property _property;

        /// <summary>
        /// 打开的文件
        /// </summary>
        public string File { get; private set; }

        public override void OnAction(UIActionType actionType, object sender, EventArgs e)
        {
            switch (actionType)
            {
                case UIActionType.TreeNodeAfterExpand:
                    _AfterExpand();
                    break;

                case UIActionType.Close:
                    _Close();
                    break;
            }

            base.OnAction(actionType, sender, e);
        }

        private void _AfterExpand()
        {
            if (_opened)
                return;

            Node.Nodes.Clear();

            try
            {
                _OpenFile();
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(_context.MainWindow, ex);
            }
            finally
            {
                _opened = true;
            }
        }

        private void _OpenFile()
        {
            StreamTableReader reader = new StreamTableReader(File);
            foreach (StreamTable table in reader.GetAllTables())
            {
                TreeNode node = new TreeNode(table.Name);
                node.StateImageIndex = 2;
                StTableNodeUIAction uiAction = new StTableNodeUIAction(_context, TreeViewUIActionManager, node, reader, table);
                node.Tag = uiAction;
                _uiActions.Add(uiAction);
                Node.Nodes.Add(node);

                uiAction.StateChanged += new EventHandler(uiAction_StateChanged);
            }

            _property = new Property(reader, File);
        }

        private void uiAction_StateChanged(object sender, EventArgs e)
        {
            bool anyOpened = _uiActions.Any(a => a.Opened);
            if (anyOpened)
                Node.NodeFont = new Font(Node.TreeView.Font, FontStyle.Bold);
            else
                Node.NodeFont = Node.TreeView.Font;
        }

        private readonly List<StTableNodeUIAction> _uiActions = new List<StTableNodeUIAction>();

        private void _Close()
        {
            TreeViewUIActionManager.Raise(UIActionType.Close, Node, raiseAllSubNodes: true, includeCurrent: false);
            Node.Remove();
        }

        protected override object OnGetPropertyObject()
        {
            try
            {
                if (!_opened)
                    _OpenFile();

                return _property;
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(_context.MainWindow, ex);
                return null;
            }
        }

        #region Class Property ...

        class Property
        {
            public Property(StreamTableReader reader, string file)
            {
                _reader = reader;
                File = file;
            }

            private readonly StreamTableReader _reader;

            [Browsable(true), DisplayName("文件")]
            public string File { get; private set; }

            [Browsable(true), DisplayName("名称")]
            public string Name { get { return _reader.HeaderInfo.Name; } }

            [Browsable(true), DisplayName("版本号")]
            public SVersion Version { get { return _reader.HeaderInfo.Version; } }

            [Browsable(true), DisplayName("创建时间")]
            public DateTime CreationTime { get { return _reader.HeaderInfo.CreationTime.ToLocalTime(); } }

            [Browsable(true), DisplayName("表格数量")]
            public int TableCount { get { return _reader.HeaderInfo.TableCount; } }

            [Browsable(true), DisplayName("备注")]
            public string Desc { get { return _reader.HeaderInfo.Desc; } }
        }

        #endregion

        private ContextMenuStrip _CreateContextMenuStrip()
        {
            ContextMenuStrip ms = new ContextMenuStrip();
            ToolStripMenuItem tsRefresh = new ToolStripMenuItem("刷新", null, delegate { _Refresh(); });
            ms.Items.Add(tsRefresh);

            ms.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem tsClose = new ToolStripMenuItem("关闭", null, delegate { _Close(); });
            ms.Items.Add(tsClose);

            return ms;
        }

        public void Refresh()
        {
            _Refresh();
        }

        private void _Refresh()
        {
            try
            {
                Node.TreeView.SuspendLayout();

                string[] openedTables = _uiActions.Where(a => a.Opened).Select(a => a.StreamTable.Name).ToArray();
                TreeViewUIActionManager.Raise(UIActionType.Close, Node, raiseAllSubNodes: true, includeCurrent: false);

                _uiActions.Clear();
                Node.Nodes.Clear();

                _OpenFile();
                foreach (string name in openedTables)
                {
                    StTableNodeUIAction uiAction = _uiActions.FirstOrDefault(ua => ua.StreamTable.Name == name);
                    if (uiAction != null)
                        uiAction.Open();
                }
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(_context.MainWindow, ex);
            }
            finally
            {
                Node.TreeView.ResumeLayout();
            }
        }
    }
}
