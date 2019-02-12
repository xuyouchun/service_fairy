using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;
using Common;

namespace ServiceFairy.Entities.Queue
{
    /// <summary>
    /// 添加一个任务－请求
    /// </summary>
    [Serializable, DataContract]
    public class Queue_AddTask_Request : RequestEntity
    {
        /// <summary>
        /// 任务
        /// </summary>
        [DataMember]
        public QueueTask[] Tasks { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (Tasks != null && Tasks.Any(t => string.IsNullOrWhiteSpace(t.Method)))
                throw new ServiceException(ServerErrorCode.ArgumentError, "未指定调用方法");
        }
    }


    /// <summary>
    /// 任务
    /// </summary>
    [Serializable, DataContract]
    public class QueueTask : IPriority<int>
    {
        /// <summary>
        /// 集合名称
        /// </summary>
        [DataMember]
        public string CollectionName { get; set; }

        /// <summary>
        /// 集合类型
        /// </summary>
        [DataMember]
        public TaskCollectionType CollectionType { get; set; }

        /// <summary>
        /// 调用方法
        /// </summary>
        [DataMember]
        public string Method { get; set; }

        /// <summary>
        /// 任务的数据（将被的反序列化为请求参数实体）
        /// </summary>
        [DataMember]
        public byte[] Data { get; set; }

        /// <summary>
        /// 数据格式
        /// </summary>
        [DataMember]
        public DataFormat DataFormat { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        [DataMember]
        public Guid TaskId { get; set; }

        /// <summary>
        /// 任务的优先级
        /// </summary>
        [DataMember]
        public int Priority { get; set; }

        public int GetPriority()
        {
            return Priority;
        }
    }

    /// <summary>
    /// 集合类型
    /// </summary>
    public enum TaskCollectionType
    {
        /// <summary>
        /// 队列
        /// </summary>
        Queue,

        /// <summary>
        /// 栈
        /// </summary>
        Stack,

        /// <summary>
        /// 哈希表
        /// </summary>
        HashSet,

        /// <summary>
        /// 优先级队列
        /// </summary>
        PriorityQueue,
    }
}
