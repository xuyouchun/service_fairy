using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics.Contracts;
using Common.Package.GlobalTimer.TimerStrategies;
using Common.Package.GlobalTimer;
using Common.Contracts.Service;
using Common.Contracts;

namespace Common.Package
{
    /// <summary>
    /// 全局定时器
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
	public class GlobalTimer<TTask> : IDisposable
        where TTask : class, ITask
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interval">轮询周期</param>
        /// <param name="defaultTaskExecutor">任务执行策略</param>
        public GlobalTimer(TimeSpan interval, ITaskExecutor<TTask> defaultTaskExecutor = null)
            : this((int)interval.TotalMilliseconds, defaultTaskExecutor)
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interval">轮询周期（毫秒数）</param>
        /// <param name="defaultTaskExecutor">任务执行策略</param>
        public GlobalTimer(int interval, ITaskExecutor<TTask> defaultTaskExecutor = null)
        {
            Contract.Requires(interval > 0);

            _Interval = interval;
            _DefaultTaskExecutor = defaultTaskExecutor ?? TaskExecutor<TTask>.Instance;
            _Timer = new Timer(_TimerCallback);
            _ChangeTimer();
        }

        private readonly Timer _Timer;
        private readonly int _Interval;
        private readonly HashSet<GlobalTimerTaskItem<TTask>> _Tasks = new HashSet<GlobalTimerTaskItem<TTask>>();
        private readonly ITaskExecutor<TTask> _DefaultTaskExecutor;

        private void _ChangeTimer()
        {
            try
            {
                _Timer.Change(_Interval, Timeout.Infinite);
            }
            catch(ObjectDisposedException) { }
        }

        private void _TimerCallback(object state)
        {
            try
            {
                lock (_Tasks)
                {
                    _Dispatch();
                }
            }
            finally
            {
                _ChangeTimer();
            }
        }

        // 调度任务
        private void _Dispatch()
        {
            foreach (GlobalTimerTaskItem<TTask> taskItem in _Tasks)
            {
                if (!taskItem.Enable)
                    continue;

                if (taskItem.EnableReenter)
                {
                    if (taskItem.IsExecuteImmediately || taskItem.TimerStrategy.IsTimeUp())
                    {
                        taskItem.IsExecuteImmediately = false;
                        taskItem.RunningCount++;
                        _RunTask(taskItem);
                    }
                }
                else
                {
                    if (!taskItem.IsExecuteImmediately && (taskItem.RunningCount > 0 || !taskItem.TimerStrategy.IsTimeUp()))
                        continue;

                    taskItem.IsExecuteImmediately = false;
                    lock (taskItem)
                    {
                        if (taskItem.RunningCount <= 0)
                        {
                            taskItem.RunningCount++;
                            _RunTask(taskItem);
                        }
                    }
                }
            }
        }

        // 运行任务
        private void _RunTask(GlobalTimerTaskItem<TTask> taskItem)
        {
            taskItem.TimerStrategy.RunNotify();
            taskItem.TaskExecutor.Execute(taskItem.Task, _TaskCompletedCallback, taskItem);
        }

        // 任务运行完毕的回调
        private void _TaskCompletedCallback(TTask task, Exception error, object state)
        {
            var taskItem = (GlobalTimerTaskItem<TTask>)state;

            taskItem.TimerStrategy.CompletedNotify();
            taskItem.RunningCount--;
        }

        /// <summary>
        /// 添加一个定时任务
        /// </summary>
        /// <param name="timerStrategy">定时方式</param>
        /// <param name="task">任务</param>
        /// <param name="taskExecutor">任务执行策略</param>
        /// <param name="enableReenter">是否允许重入</param>
        /// <param name="enable">是否为启用状态</param>
        /// <returns>控制句柄</returns>
        public IGlobalTimerTaskHandle Add(ITimerStrategy timerStrategy, TTask task, ITaskExecutor<TTask> taskExecutor, bool enableReenter = true, bool enable = true)
        {
            Contract.Requires(timerStrategy != null && task != null);

            lock (_Tasks)
            {
                var taskItem = new GlobalTimerTaskItem<TTask>(timerStrategy, task, taskExecutor ?? _DefaultTaskExecutor, enableReenter, enable);
                _Tasks.Add(taskItem);
                return new GlobalTimerTaskHandle<TTask>(this, taskItem);
            }
        }

        /// <summary>
        /// 添加一个定时任务
        /// </summary>
        /// <param name="timerStrategy">定时方式</param>
        /// <param name="task">任务</param>
        /// <param name="enableReenter">是否允许重入</param>
        /// <returns>控制句柄</returns>
        public IGlobalTimerTaskHandle Add(ITimerStrategy timerStrategy, TTask task, bool enableReenter = true)
        {
            return Add(timerStrategy, task, null, enableReenter, true);
        }

        /// <summary>
        /// 添加一个定时任务
        /// </summary>
        /// <param name="interval">周期</param>
        /// <param name="dueTime">第一次启动时间</param>
        /// <param name="task">任务</param>
        /// <param name="enableReenter">是否允许重入</param>
        /// <returns>控制句柄</returns>
        public IGlobalTimerTaskHandle Add(TimeSpan interval, TimeSpan dueTime, TTask task, bool enableReenter = true)
        {
            return Add(new StaticIntervalTimerStrategy(interval, dueTime), task, null, enableReenter, true);
        }

        /// <summary>
        /// 添加一个定时任务
        /// </summary>
        /// <param name="interval">周期</param>
        /// <param name="task">任务</param>
        /// <param name="enableReenter">是否允许重入</param>
        /// <returns>控制句柄</returns>
        public IGlobalTimerTaskHandle Add(TimeSpan interval, TTask task, bool enableReenter = true)
        {
            return Add(new StaticIntervalTimerStrategy(interval, interval), task, null, enableReenter, true);
        }

        /// <summary>
        /// 添加一个定时任务
        /// </summary>
        /// <param name="interval">周期</param>
        /// <param name="dueTime">第一次启动时间</param>
        /// <param name="task">任务</param>
        /// <param name="enableReenter">是否允许重入</param>
        /// <param name="enable">是否为启用状态</param>
        /// <returns></returns>
        public IGlobalTimerTaskHandle Add(TimeSpan interval, TimeSpan dueTime, TTask task, bool enableReenter, bool enable = true)
        {
            return Add(new StaticIntervalTimerStrategy(interval, dueTime), task, null, enableReenter, enable);
        }

        /// <summary>
        /// 添加一个定时任务
        /// </summary>
        /// <param name="interval">周期</param>
        /// <param name="task">任务</param>
        /// <param name="enableReenter">是否允许重入</param>
        /// <param name="enable">是否为启用状态</param>
        /// <returns></returns>
        public IGlobalTimerTaskHandle Add(TimeSpan interval, TTask task, bool enableReenter, bool enable = true)
        {
            return Add(new StaticIntervalTimerStrategy(interval, interval), task, null, enableReenter, enable);
        }

        /// <summary>
        /// 删除一项任务
        /// </summary>
        /// <param name="handle"></param>
        internal void Remove(GlobalTimerTaskHandle<TTask> handle)
        {
            lock (_Tasks)
            {
                _Tasks.Remove(handle.Task);
            }
        }

        /// <summary>
        /// 立即执行指定的任务
        /// </summary>
        /// <param name="handle"></param>
        internal void ExecuteImmediately(GlobalTimerTaskHandle<TTask> handle)
        {
            handle.Task.IsExecuteImmediately = true;
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _Timer.Dispose();
        }

        ~GlobalTimer()
        {
            Dispose();
        }

        #endregion

        /// <summary>
        /// 默认实体
        /// </summary>
        public static readonly GlobalTimer<ITask> Default = new GlobalTimer<ITask>(TimeSpan.FromSeconds(1));
    }
}
