using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Package.GlobalTimer
{
    /// <summary>
    /// 全局定时器的定时任务句柄
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    class GlobalTimerTaskHandle<TTask> : IGlobalTimerTaskHandle
        where TTask : class, ITask
    {
        internal GlobalTimerTaskHandle(GlobalTimer<TTask> owner, GlobalTimerTaskItem<TTask> task)
        {
            _Owner = owner;
            Task = task;
        }

        private readonly GlobalTimer<TTask> _Owner;

        internal GlobalTimerTaskItem<TTask> Task { get; private set; }

        /// <summary>
        /// 是否为启用状态
        /// </summary>
        public bool Enable
        {
            get { return Task.Enable; }
            set { Task.Enable = value; }
        }

        /// <summary>
        /// 启用
        /// </summary>
        public void Start()
        {
            Enable = true;
        }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Stop()
        {
            Enable = false;
        }

        /// <summary>
        /// 立即执行
        /// </summary>
        public void ExecuteImmediately()
        {
            if (Enable)
                _Owner.ExecuteImmediately(this);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Stop();
            _Owner.Remove(this);
        }

        #endregion
    }
}
