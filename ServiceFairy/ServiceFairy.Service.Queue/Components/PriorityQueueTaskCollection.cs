using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Collection;
using System.Diagnostics.Contracts;
using Common;

namespace ServiceFairy.Service.Queue.Components
{
    /// <summary>
    /// 基于优先级队列的任务集合
    /// </summary>
    class PriorityQueueTaskCollection<TTask> : ITaskCollection<TTask>
        where TTask : class, IPriority<int>
    {
        private readonly PriorityQueue<TTask, int> _tasks = new PriorityQueue<TTask, int>();

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
