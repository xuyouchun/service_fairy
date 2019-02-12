using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Common.Script.VbScript
{
    class StatementContext : IStatementContext, IScriptExecutorAsyncResult
    {
        public StatementContext(ScriptExecutor owner, Statement statement, bool runAtOnce, Thread thread)
        {
            _Owner = owner;
            _Event = new ManualResetEvent(runAtOnce);
            _Running = runAtOnce;
            Statement = statement;
            _Thread = thread;

            AsyncWaitEvent = new AutoResetEvent(false);
        }

        private readonly ScriptExecutor _Owner;
        private readonly ManualResetEvent _Event;
        private readonly object _SyncLocker = new object();
        private readonly Thread _Thread;

        private volatile bool _Running, _HasStop = false;

        public Statement Statement { get; private set; }
        public RunningContext RunningContext { get; internal set; }

        /// <summary>
        /// 暂停运行
        /// </summary>
        public void Pause()
        {
            if (_HasStop)
                return;

            try
            {
                if (!Monitor.TryEnter(_SyncLocker))
                    return;

                if (!_HasStop)
                {
                    _Event.Reset();
                    _Running = false;

                    //if (Thread.CurrentThread.ManagedThreadId != _Thread.ManagedThreadId && _Thread.ThreadState == ThreadState.Running)
                    //    _Thread.Suspend();
                }
            }
            finally
            {
                Monitor.Exit(_SyncLocker);
            }
        }

        /// <summary>
        /// 恢复运行
        /// </summary>
        public void Resume()
        {
            if (_HasStop)
                return;

            try
            {
                if (!Monitor.TryEnter(_SyncLocker))
                    return;

                if (!_HasStop)
                {
                    _Event.Set();
                    _Running = true;

                    //if (Thread.CurrentThread.ManagedThreadId != _Thread.ManagedThreadId && _Thread.ThreadState == ThreadState.Suspended)
                    //    _Thread.Resume();
                }
            }
            finally
            {
                Monitor.Exit(_SyncLocker);
            }
        }

        /// <summary>
        /// 是否在运行状态
        /// </summary>
        public bool Running { get { return _Running; } }

        /// <summary>
        /// 停止运行
        /// </summary>
        public void Stop()
        {
            if (_HasStop)
                return;

            try
            {
                if (!Monitor.TryEnter(_SyncLocker))
                    return;

                if (!_HasStop)
                {
                    _HasStop = true;
                    _Running = false;
                    _Event.Set();

                    if (_Thread != null)
                        _Thread.Abort();
                }
            }
            finally
            {
                Monitor.Exit(_SyncLocker);
            }
        }

        public bool GetValue(string exp, out Value result, out ExpressionType expType)
        {
            return RunningContext.GetValue(exp, out result, out expType);
        }

        #region IStatementContext 成员

        public bool BeforeExecuteStatement(RunningContext context, Statement statement)
        {
            if (_HasStop)
                return false;

            _Event.WaitOne();
            return _Owner.OnBeforeExecuteStatement(statement);
        }

        public void EndExecuteStatement(RunningContext context, Statement statement, StatementExecuteResult result)
        {
            _Owner.OnEndExecuteStatement(statement, result);
        }

        #endregion

        public AutoResetEvent AsyncWaitEvent { get; private set; }

        #region IAsyncResult 成员

        public object AsyncState { get; internal set; }

        public WaitHandle AsyncWaitHandle
        {
            get { return AsyncWaitEvent; }
        }

        public bool CompletedSynchronously { get; internal set; }

        public bool IsCompleted
        {
            get { return _HasStop; }
            internal set { _HasStop = value; }
        }

        #endregion

        /// <summary>
        /// 记录异步执行过程中出现的错误
        /// </summary>
        public Exception Error { get; internal set; }
    }
}
