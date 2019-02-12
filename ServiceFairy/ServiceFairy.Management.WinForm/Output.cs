using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using WeifenLuo.WinFormsUI.Docking;
using Common.WinForm.Docking.DockingWindows;
using Common.Utility;
using System.Threading;

namespace ServiceFairy.Management.WinForm
{
    class Output : IOutput, IDisposable
    {
        public Output(OutputDockingWindow outputWindow)
        {
            _outputWindow = outputWindow;
            _thread = new Thread(_OutputFunc);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private readonly OutputDockingWindow _outputWindow;

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

        private readonly Thread _thread;
        private readonly AutoResetEvent _waitForWriteEvent = new AutoResetEvent(false);
        private readonly ManualResetEvent _waitForExit = new ManualResetEvent(false);
        private volatile bool _running = true;

        private void _OutputFunc()
        {
            while (_running)
            {
                WaitHandle.WaitAny(new WaitHandle[] { _waitForExit, _waitForWriteEvent }, TimeSpan.FromSeconds(5));
                IOutput output = _GetOutputWindow();
                while (_running && output != null && _queue.Count > 0)
                {
                    Item[] items;
                    lock (_queue) items = _queue.DequeueAll();
                    for (int k = 0, length = items.Length; k < length; k++)
                    {
                        Item item = items[k];
                        output.Write(item.Message, item.MessageType);
                    }
                }

                if (!_running)
                    break;
            }
        }

        private IOutput _GetOutputWindow()
        {
            if (_outputWindow.Created && !_outputWindow.IsDisposed)
                return _outputWindow;

            return null;
        }

        public void Dispose()
        {
            _running = false;
            _waitForExit.Set();
        }
    }
}
