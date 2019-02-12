using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics.Contracts;

namespace Common.Package.TaskDispatcher
{
    /// <summary>
    /// 用于任务调度的信号量
    /// </summary>
    public class TaskSemaphore : ITaskSemaphore
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="max">信号个数</param>
        public TaskSemaphore(int max)
        {
            Contract.Requires(max > 0);

            _Semaphore = new Semaphore(max, max);
        }

        private readonly Semaphore _Semaphore;

        #region ITaskSemaphore Members

        /// <summary>
        /// 信号的句柄
        /// </summary>
        public WaitHandle Handler
        {
            get { return _Semaphore; }
        }

        /// <summary>
        /// 释放一个信号
        /// </summary>
        public void Release()
        {
            _Semaphore.Release();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _Semaphore.Dispose();
        }

        #endregion
    }
}
