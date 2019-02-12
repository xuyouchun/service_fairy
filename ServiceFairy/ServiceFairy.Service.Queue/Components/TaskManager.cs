using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities.Queue;
using Common.Utility;
using Common.Contracts.Service;
using Common.Package.TaskDispatcher;
using Common;
using System.Diagnostics.Contracts;
using Common.Package;

namespace ServiceFairy.Service.Queue.Components
{
    /// <summary>
    /// 任务管理器
    /// </summary>
    class TaskManager : AppComponent
    {
        public TaskManager(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly int _maxRunningCount = 5;

        private readonly Dictionary<Tuple<string, TaskCollectionType>, TaskCollectionWrapper> _taskCollections
            = new Dictionary<Tuple<string, TaskCollectionType>, TaskCollectionWrapper>();

        private readonly Service _service;

        class TaskCollectionWrapper
        {
            public string Name;

            public TaskCollectionType Type;

            public TaskDispatcher<InvokeTask> Dispatcher;
        }

        private TaskCollectionWrapper _GetWrapper(string collectionName, TaskCollectionType collectionType)
        {
            var key = new Tuple<string, TaskCollectionType>(collectionName, collectionType);
            return _taskCollections.GetOrSet(key, delegate {
                TaskDispatcherStrategy<InvokeTask> strategy = new TaskDispatcherStrategy<InvokeTask>(_maxRunningCount,
                    taskQueue: new TaskQueueAdapter(TaskCollectionFactory.CreateTaskCollection<InvokeTask>(collectionType))
                );

                return new TaskCollectionWrapper() {
                    Dispatcher = new TaskDispatcher<InvokeTask>(strategy),
                    Type = collectionType, Name = collectionName
                };
            });
        }

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(QueueTask task)
        {
            Contract.Requires(task != null);

            if (task.TaskId == Guid.Empty)
                task.TaskId = Guid.NewGuid();

            TaskCollectionWrapper w = _GetWrapper(task.CollectionName ?? string.Empty, task.CollectionType);
            w.Dispatcher.Add(new InvokeTask(this, task));
        }

        /// <summary>
        /// 批量添加任务
        /// </summary>
        /// <param name="tasks"></param>
        public void AddTasks(QueueTask[] tasks)
        {
            Contract.Requires(tasks != null);

            foreach (QueueTask task in tasks)
            {
                AddTask(task);
            }
        }

        class TaskQueueAdapter : ITaskQueue<InvokeTask>
        {
            public TaskQueueAdapter(ITaskCollection<InvokeTask> tasks)
            {
                _tasks = tasks;
            }

            private readonly ITaskCollection<InvokeTask> _tasks;

            public void Enqueue(InvokeTask task)
            {
                _tasks.Add(task);
            }

            public InvokeTask Dequeue()
            {
                return _tasks.Get();
            }
        }

        class InvokeTask : ITask, IPriority<int>
        {
            public InvokeTask(TaskManager owner, QueueTask task)
            {
                _owner = owner;
                _task = task;
            }

            private readonly QueueTask _task;
            private readonly TaskManager _owner;

            public void Execute()
            {
                try
                {
                    Service srv = _owner._service;
                    srv.Context.Communicate.Call(null, _task.Method, new CommunicateData(_task.Data, _task.DataFormat));
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }

            int IPriority<int>.GetPriority()
            {
                return _task.GetPriority();
            }
        }

        protected override void OnDispose()
        {
            lock (_taskCollections)
            {
                foreach (TaskCollectionWrapper w in _taskCollections.Values.ToArray())
                {
                    w.Dispatcher.Dispose();
                }
            }
        }
    }
}
