using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common;
using ServiceFairy.Entities.Queue;
using Common.Contracts;

namespace ServiceFairy.Service.Queue.Components
{
    /// <summary>
    /// 任务集合
    /// </summary>
    interface ITaskCollection<TTask> where TTask : class
    {
        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="task"></param>
        void Add(TTask task);

        /// <summary>
        /// 获取下一个任务
        /// </summary>
        /// <returns></returns>
        TTask Get();

        /// <summary>
        /// 任务数量
        /// </summary>
        int Count { get; }
    }

    static class TaskCollectionFactory
    {
        public static ITaskCollection<TTask> CreateTaskCollection<TTask>(TaskCollectionType type)
            where TTask : class, IPriority<int>
        {
            switch (type)
            {
                case TaskCollectionType.Queue:
                    return new QueueTaskCollection<TTask>();

                case TaskCollectionType.Stack:
                    return new StackTaskCollection<TTask>();

                case TaskCollectionType.HashSet:
                    return new HashSetTaskCollection<TTask>();

                case TaskCollectionType.PriorityQueue:
                    return new PriorityQueueTaskCollection<TTask>();

                default:
                    throw new NotSupportedException("不支持该类型的集合");
            }
        }
    }
}
