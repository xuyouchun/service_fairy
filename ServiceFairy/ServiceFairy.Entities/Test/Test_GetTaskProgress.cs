using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Test
{
    /// <summary>
    /// 获取测试任务的进度
    /// </summary>
    [Serializable, DataContract]
    public class Test_GetTaskProgress_Request : RequestEntity
    {
        /// <summary>
        /// 任务ID，如果为空，则获取全部任务的进度
        /// </summary>
        [DataMember]
        public Guid[] TaskIds { get; set; }
    }

    /// <summary>
    /// 获取测试任务的进度
    /// </summary>
    [Serializable, DataContract]
    public class Test_GetTaskProgress_Reply : ReplyEntity
    {
        /// <summary>
        /// 任务的进度
        /// </summary>
        [DataMember]
        public TaskProgress[] Progresses { get; set; }
    }

    /// <summary>
    /// 任务的进度
    /// </summary>
    [Serializable, DataContract]
    public class TaskProgress
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [DataMember]
        public Guid TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [DataMember]
        public string Args { get; set; }

        /// <summary>
        /// 开始时间（如果尚未开始则留空）
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间（如果尚未结束则留空）
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 运行次数
        /// </summary>
        [DataMember]
        public int RunTimes { get; set; }

        /// <summary>
        /// 线程数
        /// </summary>
        [DataMember]
        public int ThreadCount { get; set; }

        /// <summary>
        /// 错误
        /// </summary>
        [DataMember]
        public string Error { get; set; }
    }
}
