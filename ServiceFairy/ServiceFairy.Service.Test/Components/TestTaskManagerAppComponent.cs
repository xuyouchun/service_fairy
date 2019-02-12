using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Collection;
using Common.Utility;
using System.Diagnostics;
using ServiceFairy.Entities.Test;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Test.Components
{
    /// <summary>
    /// 测试管理器
    /// </summary>
    [AppComponent("测试管理器", "管理测试任务")]
    class TestTaskManagerAppComponent : AppComponent
    {
        public TestTaskManagerAppComponent(Service service)
            : base(service)
        {
            
        }

        private readonly ThreadSafeDictionaryWrapper<Guid, TaskWrapper> _taskDict = new ThreadSafeDictionaryWrapper<Guid, TaskWrapper>();

        class TaskWrapper
        {
            public ITestTask Task;
            public TestTaskInfo Info;
            public TaskProgress TaskProgress;
        }

        /// <summary>
        /// 启动测试服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Guid StartTest(string name, string title, string args)
        {
            ITestTask task = TestTaskFactory.CreateTestTask(name);
            task.Validate(args);

            Guid id = Guid.NewGuid();
            TaskWrapper w = new TaskWrapper() {
                Task = task,
                Info = new TestTaskInfo() { Name = name, Title = title, Args = args },
                TaskProgress = new TaskProgress() { Name = name, Title = title, Args = args, TaskId = id },
            };

            _taskDict.Add(id, w);

            ThreadUtility.StartNew(delegate {
                _StartTest(w);
            });

            return id;
        }

        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="w"></param>
        private void _StartTest(TaskWrapper w)
        {
            DateTime startTime = DateTime.UtcNow;

            try
            {
                w.Task.Start(w.Info.Args);
            }
            catch (Exception ex)
            {
                w.TaskProgress.Error = ex.ToString();
            }
            finally
            {
                w.TaskProgress.EndTime = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// 获取任务的进度
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public TaskProgress[] GetTaskProgresses(Guid[] taskIds)
        {
            Contract.Requires(taskIds != null);

            return taskIds.Select(id => _taskDict.GetOrDefault(id)).ToArrayNotNull(td => td.TaskProgress);
        }

        /// <summary>
        /// 获取所有任务的进度
        /// </summary>
        /// <returns></returns>
        public TaskProgress[] GetAllTestProgresses()
        {
            return _taskDict.Values.ToArray(w => w.TaskProgress);
        }

        /// <summary>
        /// 停止测试
        /// </summary>
        /// <param name="testIds"></param>
        public void StopTest(Guid[] testIds)
        {
            Contract.Requires(testIds != null);

            testIds.Select(id => _taskDict.GetOrDefault(id)).ToArrayNotNull(td => td.Task)
                .ForEach(task => task.Stop());
        }
    }
}
