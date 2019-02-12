using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.Queue.Components
{
    /// <summary>
    /// 基于哈希表的任务集合
    /// </summary>
    class HashSetTaskCollection<TTask> : ITaskCollection<TTask>
        where TTask : class
    {
        private readonly SortedSet<Wrapper> _tasks = new SortedSet<Wrapper>();

        class Wrapper : IComparable<Wrapper>
        {
            public Wrapper(TTask task)
            {
                Task = task;
                _priority = _random.Next(int.MaxValue);
            }

            private static Random _random = new Random();

            public TTask Task { get; private set; }

            private int _priority;

            public int CompareTo(Wrapper other)
            {
                return _priority.CompareTo(other._priority);
            }
        }

        public void Add(TTask task)
        {
            Contract.Requires(task != null);

            lock (_tasks)
            {
                _tasks.Add(new Wrapper(task));
            }
        }

        public TTask Get()
        {
            if (_tasks.Count == 0)
                return null;

            lock (_tasks)
            {
                Wrapper w = _tasks.First();
                TTask task = w.Task;
                _tasks.Remove(w);
                return task;
            }
        }

        public int Count
        {
            get { return _tasks.Count; }
        }
    }
}
