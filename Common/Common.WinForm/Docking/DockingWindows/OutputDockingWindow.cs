using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts;
using Common.Contracts.UIObject;
using System.Threading;
using Common.Utility;

namespace Common.WinForm.Docking.DockingWindows
{
    /// <summary>
    /// 输出窗口
    /// </summary>
    public partial class OutputDockingWindow : DockContentEx, IOutput, IOutputWindow
    {
        public OutputDockingWindow()
        {
            InitializeComponent();
        }

        #region Class Item ...

        class Item
        {
            public Item(string message, MessageType messageType)
            {
                Message = message;
                MessageType = messageType;
            }

            public string Message { get; private set; }

            public MessageType MessageType { get; private set; }
        }

        #endregion

        private readonly Queue<Item> _queue = new Queue<Item>();

        public void Write(string s, MessageType type = MessageType.Message)
        {
            lock (_queue)
            {
                _queue.Enqueue(new Item(s, type));
            }

            _waitForExit.Set();
        }

        private Thread _thread;
        private readonly AutoResetEvent _waitForWriteEvent = new AutoResetEvent(false);
        private readonly ManualResetEvent _waitForExit = new ManualResetEvent(false);
        private volatile bool _running = true;

        private void _OutputFunc()
        {
            while (_running)
            {
                WaitHandle.WaitAny(new WaitHandle[] { _waitForExit, _waitForWriteEvent }, TimeSpan.FromSeconds(5));
                while (_running && _queue.Count > 0)
                {
                    Item[] items;
                    lock (_queue) items = _queue.DequeueAll();

                    this.BeginInvoke(delegate {
                        for (int k = 0, length = items.Length; k < length; k++)
                        {
                            Item item = items[k];
                            _Write(item.Message, item.MessageType);
                        }
                    });
                }
            }
        }

        private void _Write(string s, MessageType messageType)
        {
            _txtOutput.AppendText(s);
        }

        private void OutputDockingWindow_Load(object sender, EventArgs e)
        {
            _thread = new Thread(_OutputFunc);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void OutputDockingWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClosingNotify();
        }

        public override bool ClosingNotify()
        {
            _running = false;
            _waitForExit.Set();

            return base.ClosingNotify();
        }
    }
}
