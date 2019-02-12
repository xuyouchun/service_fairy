using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Ats.Client.Script.ScriptExecutors
{
    class StatementContext : IStatementContext, IScriptExecutorAsyncResult
    {
        public StatementContext(ScriptExecutor owner, Statement statement, IExpressionContext expressionContext, bool runAtOnce)
        {
            _Owner = owner;
            _Event = new ManualResetEvent(runAtOnce);
            _Running = runAtOnce;
            _ExpressionContext = expressionContext;
            Statement = statement;

            AsyncWaitEvent = new AutoResetEvent(false);
        }

        private readonly ScriptExecutor _Owner;
        private readonly IExpressionContext _ExpressionContext;
        private readonly ManualResetEvent _Event;
        private readonly object _SyncLocker = new object();

        private volatile bool _Running, _HasStop = false;

        public Statement Statement { get; private set; }

        /// <summary>
        /// 暂停运行
        /// </summary>
        public void Pause()
        {
            lock (_SyncLocker)
            {
                if (!_HasStop)
                {
                    _Event.Reset();
                    _Running = false;
                }
            }
        }

        /// <summary>
        /// 恢复运行
        /// </summary>
        public void Resume()
        {
            lock (_SyncLocker)
            {
                if (!_HasStop)
                {
                    _Event.Set();
                    _Running = true;
                }
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
            lock (_SyncLocker)
            {
                if (!_HasStop)
                {
                    _HasStop = true;
                    _Running = false;
                    _Event.Set();
                }
            }
        }

        #region IStatementContext 成员

        public IExpressionContext ExpressionContext
        {
            get { return _ExpressionContext; }
        }

        public bool BeforeExecuteStatement(Statement statement)
        {
            if (_HasStop)
                return false;

            _Event.WaitOne();
            return _Owner.BeforeExecuteStatement(statement);
        }

        public void EndExecuteStatement(Statement statement, Exception error)
        {
            _Owner.EndExecuteStatement(statement, error);
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
