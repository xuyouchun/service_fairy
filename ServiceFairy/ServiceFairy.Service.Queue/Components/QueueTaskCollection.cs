using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.Queue.Components
{
    /// <summary>
    /// 基于队列的任务集合
    /// </summary>
    class QueueTaskCollection<TTask> : ITaskCollection<TTask>
        where TTask : class
    {
        private readonly Queue<TTask> _tasks = new Queue<TTask>();

        public void Add(TTask task)
        {
            Contract.Requires(task != null);

            lock (_tasks)
            {
                _tasks.Enqueue(task);
            }
        }

        public TTask Get()
        {
            if (_tasks.Count == 0)
                return null;

            lock (_tasks)
            {
                return _tasks.Dequeue();
            }
        }

        public int Count
        {
            get { return _tasks.Count; }
        }
    }
}
