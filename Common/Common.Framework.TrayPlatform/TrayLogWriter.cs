using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.GlobalTimer;
using Common.Utility;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    class TrayLogWriter : ILogWriter<LogItem>, IDisposable
    {
        public TrayLogWriter(ITrayLogManager logManager)
        {
            _logManager = logManager;
            _handle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(1), new TaskFuncAdapter(_WriteLogFunc), false);
        }

        private readonly ITrayLogManager _logManager;
        private readonly IGlobalTimerTaskHandle _handle;

        private readonly Queue<LogItem> _items = new Queue<LogItem>();

        public void Write(IEnumerable<LogItem> items)
        {
            lock (_items)
            {
                _items.EnqueueRange(items);
            }
        }

        private void _WriteLogFunc()
        {
            if (_items.Count == 0)
                return;

            LogItem[] items;
            lock (_items)
            {
                items = _items.ToArray();
                _items.Clear();
            }

            if (_logManager != null && _logManager.Writer != null)
                _logManager.Writer.Write(items);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}
