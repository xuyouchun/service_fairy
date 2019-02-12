﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;

namespace Common.Package.GlobalTimer
{
    [System.Diagnostics.DebuggerStepThrough]
    class GlobalTimerTaskItem<TTask>
        where TTask : class, ITask
    {
        public GlobalTimerTaskItem(ITimerStrategy timerStrategy, TTask task, ITaskExecutor<TTask> taskExecutor, bool enableReenter, bool enable)
        {
            TimerStrategy = timerStrategy;
            TaskExecutor = taskExecutor;
            EnableReenter = enableReenter;
            Task = task;
            Enable = enable;
        }

        // 定时策略
        public ITimerStrategy TimerStrategy { get; private set; }

        /// <summary>
        /// 任务执行策略
        /// </summary>
        public ITaskExecutor<TTask> TaskExecutor { get; private set; }

        // 是否等待完成之后才重新开始计时
        public bool EnableReenter { get; private set; }

        /// <summary>
        /// 是否接到立即执行的命令
        /// </summary>
        public bool IsExecuteImmediately { get; set; }

        /// <summary>
        /// 任务
        /// </summary>
        public TTask Task { get; private set; }

        private volatile bool _enable = false;

        /// <summary>
        /// 是否为启用状态
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        private volatile int _runningCount = 0;

        /// <summary>
        /// 记录当前有多少个处于运行状态的任务
        /// </summary>
        public int RunningCount
        {
            get { return _runningCount; }
            set { _runningCount = value; }
        }
    }
}
