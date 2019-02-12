using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.Queue.Components
{
    /// <summary>
    /// 基于栈的任务集合
    /// </summary>
    class StackTaskCollection<TTask> : ITaskCollection<TTask>
        where TTask : class
    {
        private readonly Stack<TTask> _tasks = new Stack<TTask>();

        public void Add(TTask task)
        {
            Contract.Requires(task != null);

            lock (_tasks)
            {
                _tasks.Push(task);
            }
        }

        public TTask Get()
        {
            if (_tasks.Count == 0)
                return null;

            lock (_tasks)
            {
                return _tasks.Pop();
            }
        }

        public int Count
        {
            get { return _tasks.Count; }
        }
    }
}
