using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Package
{
    /// <summary>
    /// 默认的任务执行策略
    /// </summary>
    /// <typeparam name="TTask">任务类型</typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    class TaskExecutor<TTask> : ITaskExecutor<TTask>
        where TTask : class, ITask
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="threadPriority">线程优先级</param>
        /// <param name="useThreadPool">是否使用连接池</param>
        public TaskExecutor(ThreadPriority threadPriority = ThreadPriority.Normal, bool useThreadPool = true)
        {
            _threadPriority = threadPriority;
            _useThreadPool = useThreadPool;
        }

        private readonly ThreadPriority _threadPriority;
        private readonly bool _useThreadPool;

        #region ITaskExecutor<TTask> Members

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="completedCallback">任务完成的回调</param>
        /// <param name="state">其它参数，在completedCallback中被传回</param>
        public void Execute(TTask task, TaskCompletedCallback<TTask> completedCallback, object state)
        {
            Contract.Requires(task != null && completedCallback != null);

            object args = new object[] { task, completedCallback, state };
            if (_useThreadPool)
                ThreadPool.QueueUserWorkItem(_ExecuteByThreadPool, args);
            else
                ThreadUtility.StartNew<object>(_Execute, args, _threadPriority);
        }

        private void _ExecuteByThreadPool(object state)
        {
            Thread.CurrentThread.Priority = _threadPriority;
            _Execute(state);
        }

        private void _Execute(object state)
        {
            object[] states = (object[])state;
            TTask task = (TTask)states[0];
            var completedCallback = (TaskCompletedCallback<TTask>)states[1];

            try
            {
                task.Execute();
                completedCallback(task, null, states[2]);
            }
            catch (Exception ex)
            {
                completedCallback(task, ex, states[2]);
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            
        }

        #endregion

        internal static readonly TaskExecutor<TTask> Instance = new TaskExecutor<TTask>();
    }
}
