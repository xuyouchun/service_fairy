using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Threading;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using System.Runtime.CompilerServices;

namespace Common.Package.TaskDispatcher
{
    /// <summary>
    /// 任务调度器
    /// </summary>
    /// <typeparam name="TTask">任务类型</typeparam>
    public class TaskDispatcher<TTask> : IDisposable, _ITaskDispatcher
        where TTask : class, ITask
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dispatcherStrategy">任务调度策略</param>
        /// <param name="dispatcherExecutor">任务执行策略</param>
        public TaskDispatcher(
            ITaskDispatcherStrategy<TTask> dispatcherStrategy = null,
            ITaskExecutor<TTask> dispatcherExecutor = null
         )
	    {
            _DispatcherExecutor = dispatcherExecutor ?? TaskExecutor<TTask>.Instance;
            _DispatcherStrategy = dispatcherStrategy ?? new TaskDispatcherStrategy<TTask>();

            _DispatcherStrategy.RunDispatch(_TaskRunCallback);

            _TaskDispatcherManager.Register(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxRunningCount">最大运行数量</param>
        /// <param name="dispatcherExecutor">任务执行策略</param>
        public TaskDispatcher(int maxRunningCount, ITaskExecutor<TTask> dispatcherExecutor = null)
            : this(maxRunningCount, ThreadPriority.Normal, dispatcherExecutor)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxRunningCount">最大运行数量</param>
        /// <param name="threadPriority">线程优先级</param>
        /// <param name="dispatcherExecutor">任务执行策略</param>
        public TaskDispatcher(int maxRunningCount, ThreadPriority threadPriority, ITaskExecutor<TTask> dispatcherExecutor = null)
            : this(new TaskDispatcherStrategy<TTask>(new TaskSemaphore(maxRunningCount), threadPriority: threadPriority),
            dispatcherExecutor ?? new TaskExecutor<TTask>(threadPriority))
        {

        }

        private readonly ITaskDispatcherStrategy<TTask> _DispatcherStrategy;
        private readonly ITaskExecutor<TTask> _DispatcherExecutor;
        private object _syncLocker = new object();

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="task">任务</param>
        public void Add(TTask task)
        {
            Contract.Requires(task != null);

            AddRange(new[] { task });
        }

        /// <summary>
        /// 批量添加任务
        /// </summary>
        /// <param name="tasks"></param>
        public void AddRange(IEnumerable<TTask> tasks)
        {
            Contract.Requires(tasks != null);

            _WaitForAllCompletedEvent.Reset();
            _DispatcherStrategy.AddTasks(tasks);
        }

        private readonly HashSet<TTask> _runningTasks = new HashSet<TTask>();

        // 当任务需要执行时的回调
        private void _TaskRunCallback(TTask task)
        {
            lock (_syncLocker)
            {
                _runningTasks.Add(task);
                _DispatcherExecutor.Execute(task, _TaskCompletedCallback, null);
            }
        }

        /// <summary>
        /// 当前正在执行的任务数
        /// </summary>
        public int RunningCount
        {
            get { return _runningTasks.Count; }
        }

        /// <summary>
        /// 当前正在运行的任务
        /// </summary>
        /// <returns></returns>
        public TTask[] GetRunningTasks()
        {
            lock (_syncLocker)
            {
                return _runningTasks.ToArray();
            }
        }

        private int _completedCount = 0;

        // 任务完成时的回调
        private void _TaskCompletedCallback(TTask task, Exception error, object state)
        {
            lock (_syncLocker)
            {
                _runningTasks.Remove(task);

                _DispatcherStrategy.TaskCompletedNotify(task);
                _RaiseTaskCompletedEvent(task, error);

                if (_runningTasks.Count == 0)
                {
                    _WaitForAllCompletedEvent.Set();
                    _RaiseAllTaskCompletedEvent();
                }

                if (++_completedCount % 100 == 0)
                    _runningTasks.TrimExcess();
            }
        }

        // 触发任务完成事件
        private void _RaiseTaskCompletedEvent(TTask task, Exception error)
        {
            var eh = TaskCompleted;
            if (eh != null)
                eh(this, new TaskCompletedEventArgs<TTask>(task, error));
        }

        // 触发所有任务完成事件
        private void _RaiseAllTaskCompletedEvent()
        {
            var eh = AllTaskCompleted;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        /// <summary>
        /// 任务完成事件
        /// </summary>
        public event TaskCompletedEventHandler<TTask> TaskCompleted;

        /// <summary>
        /// 所有任务完成事件
        /// </summary>
        public event EventHandler AllTaskCompleted;

        private readonly AutoResetEvent _WaitForAllCompletedEvent = new AutoResetEvent(false);

        /// <summary>
        /// 等待所有的任务执行完毕
        /// </summary>
        public void WaitForAllCompleted()
        {
            _WaitForAllCompletedEvent.WaitOne();
        }

        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            _TaskDispatcherManager.Unregister(this);

            GC.SuppressFinalize(this);
 	        _DispatcherStrategy.Dispose();
        }

        ~TaskDispatcher()
        {
            Dispose();
        }

        #endregion

        #region Class DefaultDispatcher ...

        class DefaultDispatcher : TaskDispatcher<TTask>, _ITaskDispatcher
        {
            public DefaultDispatcher()
                : base(10)
            {

            }

            public override void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            void _ITaskDispatcher.Close()
            {
                base.Dispose();
            }

            private static volatile DefaultDispatcher _instance;

            public static DefaultDispatcher Instance
            {
                get
                {
                    if (_instance != null)
                        return _instance;

                    lock (typeof(DefaultDispatcher))
                    {
                        return _instance ?? (_instance = new DefaultDispatcher());
                    }
                }
            }
        }

        #endregion

        public static TaskDispatcher<TTask> Default
        {
            get { return DefaultDispatcher.Instance; }
        }

        void _ITaskDispatcher.Close()
        {
            Dispose();
        }
    }

    #region Interface _ITaskDispatcher ...

    internal interface _ITaskDispatcher
    {
        void Close();
    }

    #endregion

    #region Class _TaskDispatcherManager ...

    static class _TaskDispatcherManager
    {
        static _TaskDispatcherManager()
        {
            SystemUtility.ApplicationExit += SystemUtility_ApplicationExit;
        }

        private static readonly ObjectManager<_ITaskDispatcher> _objMgr = new ObjectManager<_ITaskDispatcher>(TimeSpan.FromSeconds(10), _Check);

        public static void Register(_ITaskDispatcher dispatcher)
        {
            _objMgr.Register(dispatcher);
        }

        public static void Unregister(_ITaskDispatcher dispatcher)
        {
            _objMgr.Unregister(dispatcher);
        }

        private static void _Check(_ITaskDispatcher dispatcher)
        {
            
        }

        private static void SystemUtility_ApplicationExit(object sender, EventArgs e)
        {
            foreach (_ITaskDispatcher dispatcher in _objMgr.GetAll())
            {
                dispatcher.Close();
            }
        }
    }

    #endregion

    /// <summary>
    /// 任务完成事件的参数
    /// </summary>
    /// <typeparam name="TTask"></typeparam>
    public class TaskCompletedEventArgs<TTask> : EventArgs
        where TTask : class, ITask
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="task">任务</param>
        internal TaskCompletedEventArgs(TTask task, Exception error)
	    {
            Task = task;
            Error = error;
    	}

        /// <summary>
        /// 任务
        /// </summary>
        public TTask Task { get; private set; }

        /// <summary>
        /// 错误
        /// </summary>
        /// <remarks>如果执行正确，则为空引用</remarks>
        public Exception Error { get; private set; }

        /// <summary>
        /// 是否已经执行成功
        /// </summary>
        public bool Success
        {
            get { return Error == null; }
        }
    }

    /// <summary>
    /// 任务完成事件
    /// </summary>
    /// <typeparam name="TTask">任务类型</typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TaskCompletedEventHandler<TTask>(object sender, TaskCompletedEventArgs<TTask> e)
        where TTask : class, ITask;
}
