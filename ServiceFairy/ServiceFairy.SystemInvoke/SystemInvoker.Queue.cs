using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities.Queue;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities;
using Common.Contracts;
using Common.Package.Serializer;
using Common.Contracts.Service;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private QueueInvoker _queue;

        /// <summary>
        /// Queue Service
        /// </summary>
        public QueueInvoker Queue
        {
            get { return _queue ?? (_queue = new QueueInvoker(this)); }
        }

        /// <summary>
        /// 队列服务
        /// </summary>
        public class QueueInvoker : Invoker
        {
            public QueueInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 批量添加任务
            /// </summary>
            /// <param name="tasks">任务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult AddTasksSr(QueueTask[] tasks, CallingSettings settings = null)
            {
                Contract.Requires(tasks != null);

                return QueueService.AddTask(Sc, new Queue_AddTask_Request() { Tasks = tasks }, settings);
            }

            /// <summary>
            /// 批量添加任务
            /// </summary>
            /// <param name="tasks">任务</param>
            /// <param name="settings">调用设置</param>
            public void AddTasks(QueueTask[] tasks, CallingSettings settings = null)
            {
                InvokeWithCheck(AddTasksSr(tasks, settings));
            }


            /// <summary>
            /// 添加一个任务
            /// </summary>
            /// <param name="method">服务方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="collectionType">集合类型</param>
            /// <param name="priority">优先级</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Guid> AddTaskSr(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Binary, string collectionName = null, TaskCollectionType collectionType = TaskCollectionType.PriorityQueue, int priority = 0, CallingSettings settings = null)
            {
                QueueTask qt = _CreateTask(method, requestEntity, format, collectionName, collectionType, priority);
                var sr = AddTasksSr(new[] { qt }, settings);
                return CreateSr(sr, qt.TaskId);
            }

            /// <summary>
            /// 添加一个任务
            /// </summary>
            /// <param name="method">服务方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="format">参数序列化格式</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="collectionType">集合类型</param>
            /// <param name="priority">优先级</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Guid AddTask(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Unknown, string collectionName = null, TaskCollectionType collectionType = TaskCollectionType.PriorityQueue, int priority = 0, CallingSettings settings = null)
            {
                return InvokeWithCheck(AddTaskSr(method, requestEntity, format, collectionName, collectionType, priority, settings));
            }

            private QueueTask _CreateTask(string method, RequestEntity requestEntity, DataFormat format, string collectionName, TaskCollectionType collectionType, int priority)
            {
                Contract.Requires(method != null);

                if (format == DataFormat.Unknown || format == DataFormat.Unknown)
                    format = DataFormat.Binary;

                byte[] reqData = (requestEntity == null) ? null : SerializerUtility.SerializeToBytes(format, requestEntity);
                return new QueueTask() {
                    CollectionName = collectionName, CollectionType = collectionType, Data = reqData, DataFormat = format,
                    Method = method, Priority = priority, TaskId = Guid.NewGuid(),
                };
            }

            /// <summary>
            /// 将任务进队
            /// </summary>
            /// <param name="method">服务方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="format">参数序列化格式</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Guid> EnqueueSr(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Unknown, string collectionName = null, CallingSettings settings = null)
            {
                return AddTaskSr(method, requestEntity, format, collectionName, TaskCollectionType.Queue, 0, settings);
            }

            /// <summary>
            /// 将任务进队
            /// </summary>
            /// <param name="method">服务方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="format">参数序列化格式</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Guid Enqueue(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Unknown, string collectionName = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(EnqueueSr(method, requestEntity, format, collectionName, settings));
            }

            /// <summary>
            /// 将任务进优先级队列
            /// </summary>
            /// <param name="method">请求方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="format">参数序列化格式</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="priority">优先级</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Guid> EnqueuePrioritySr(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Unknown, string collectionName = null, int priority = 0, CallingSettings settings = null)
            {
                return AddTaskSr(method, requestEntity, format, collectionName, TaskCollectionType.PriorityQueue, priority, settings);
            }

            /// <summary>
            /// 将任务进优先级队列
            /// </summary>
            /// <param name="method">请求方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="format">参数序列化格式</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="priority">优先级</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Guid EnqueuePriority(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Unknown, string collectionName = null, int priority = 0, CallingSettings settings = null)
            {
                return InvokeWithCheck(EnqueuePrioritySr(method, requestEntity, format, collectionName, priority, settings));
            }

            /// <summary>
            /// 将任务进栈
            /// </summary>
            /// <param name="method">请求方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="format">参数序列化格式</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Guid> PushSr(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Unknown, string collectionName = null, CallingSettings settings = null)
            {
                return AddTaskSr(method, requestEntity, format, collectionName, TaskCollectionType.Stack, 0, settings);
            }

            /// <summary>
            /// 将任务进栈
            /// </summary>
            /// <param name="method">请求方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="format">参数序列化格式</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Guid Push(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Unknown, string collectionName = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(PushSr(method, requestEntity, format, collectionName, settings));
            }

            /// <summary>
            /// 将任务添加进哈希表，乱序执行
            /// </summary>
            /// <param name="method">请求方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="format">参数格式</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Guid> AddToHashSetSr(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Unknown, string collectionName = null, CallingSettings settings = null)
            {
                return AddTaskSr(method, requestEntity, format, collectionName, TaskCollectionType.HashSet, 0, settings);
            }

            /// <summary>
            /// 将任务添加进哈希表，乱序执行
            /// </summary>
            /// <param name="method">请求方法</param>
            /// <param name="requestEntity">请求参数</param>
            /// <param name="format">参数格式</param>
            /// <param name="collectionName">集合名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Guid AddToHashSet(string method, RequestEntity requestEntity, DataFormat format = DataFormat.Unknown, string collectionName = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(AddToHashSetSr(method, requestEntity, format, collectionName, settings));
            }
        }
    }
}
