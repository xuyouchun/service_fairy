using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading;
using Common.Contracts;

namespace Common.Package
{
    /// <summary>
    /// 默认的对象池策略
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    class ObjectPoolStrategy : IObjectPoolStrategy
    {
        public ObjectPoolStrategy(int maxCount)
        {
            Contract.Requires(maxCount > 0);
            _MaxCount = maxCount;
        }

        private readonly int _MaxCount;
        private volatile int _TotalCount, _ReadyCount;
        private readonly AutoResetEvent _SyncEvent = new AutoResetEvent(true);
        private readonly AutoResetEvent _WaitForResourceEvent = new AutoResetEvent(false);

        #region IObjectPoolStrategy Members

        public bool Wait(int count, int timeoutMillsecounds, WaitHandle waitForExit)
        {
            if (!_Wait(timeoutMillsecounds, waitForExit))
                return false;

            while (_TotalCount + (count - _ReadyCount) > _MaxCount)
            {
                _WaitForResourceEvent.WaitOne();
            }

            return true;
        }

        private bool _Wait(int timeoutMillsecounds, WaitHandle waitForExit)
        {
            if (waitForExit == null)
                return _SyncEvent.WaitOne(timeoutMillsecounds);

            return WaitHandle.WaitAny(new WaitHandle[] { _SyncEvent, waitForExit }, timeoutMillsecounds) == 0;
        }

        public void AccquireNotify(int count, int newCount)
        {
            _TotalCount += newCount;
            _ReadyCount -= count - newCount;

            _SyncEvent.Set();
        }

        public void ReleaseNotify(int count)
        {
            _ReadyCount += count;
            _WaitForResourceEvent.Set();
        }

        public int TrimNotify()
        {
            return _MaxCount;
        }

        #endregion
    }
}
