using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Threading;
using System.Collections.Concurrent;

namespace Common.Framework.TrayPlatform
{
	partial class TrayAppServiceManager
	{
        /// <summary>
        /// 平台线程同步器
        /// </summary>
        class TraySynchronizer : MarshalByRefObjectEx, ITraySynchronizer, IDisposable
        {
            public TraySynchronizer(TrayAppServiceManager owner, TrayAppServiceInfo info)
            {
                _info = info;
                _owner = owner;
            }

            private readonly TrayAppServiceInfo _info;
            private readonly TrayAppServiceManager _owner;

            class LockContext
            {
                public int ThreadId;
                public AutoResetEvent Event;
                public TraySynchronizer Owner;
                public int WaitCount;

                public static readonly ConcurrentDictionary<string, LockContext> Dict = new ConcurrentDictionary<string, LockContext>();
            }

            /// <summary>
            /// 进入临界区
            /// </summary>
            /// <param name="name"></param>
            /// <param name="timeout"></param>
            public bool Enter(string name, TimeSpan timeout)
            {
                while (!_disposed)
                {
                    LockContext lc;
                    if (!_GetOrAddLock(name, out lc))
                    {
                        int index = WaitHandle.WaitAny(new WaitHandle[] { lc.Event, _waitForExit },
                            timeout == default(TimeSpan) ? -1 : (int)timeout.TotalMilliseconds);

                        if (index != 0)
                            return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                return !_disposed;
            }

            /// <summary>
            /// 尝试进入临界区
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool TryEnter(string name)
            {
                LockContext lc;
                return _GetOrAddLock(name, out lc);
            }

            // 获取指定名称的锁，返回值表示该锁是否为当前线程所拥有
            private bool _GetOrAddLock(string name, out LockContext lc)
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                lc = LockContext.Dict.AddOrUpdate(name ?? _DEFAULT_LOCKER_NAME, (key) =>
                    new LockContext { ThreadId = threadId, Event = new AutoResetEvent(false), Owner = this },
                    (key, old) => old
                );

                return lc.ThreadId == threadId;
            }

            private const string _DEFAULT_LOCKER_NAME = "<NO NAME>";

            /// <summary>
            /// 退出临界区
            /// </summary>
            /// <param name="name"></param>
            public void Exit(string name)
            {
                LockContext lc;
                if (LockContext.Dict.TryRemove(name ?? _DEFAULT_LOCKER_NAME, out lc))
                {
                    if (lc.ThreadId != Thread.CurrentThread.ManagedThreadId)
                    {
                        throw new SynchronizationLockException(
                            string.Format("退出临界区“{0}”的线程并非该临界区的拥有者", name)
                        );
                    }

                    lc.Event.Set();
                }
            }

            private volatile bool _disposed;
            private readonly ManualResetEvent _waitForExit = new ManualResetEvent(false);

            public void Dispose()
            {
                _disposed = true;
                _waitForExit.Set();
            }
        }
	}
}
