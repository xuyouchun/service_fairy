using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取线程信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetThreads_Request : RequestEntity
    {
        /// <summary>
        /// 进程ID
        /// </summary>
        [DataMember]
        public int[] ProcessIds { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(ProcessIds, "ProcessIds");
        }
    }

    /// <summary>
    /// 线程信息的集合
    /// </summary>
    [Serializable, DataContract]
    public class ThreadInfoCollection
    {
        /// <summary>
        /// 进程ID
        /// </summary>
        [DataMember]
        public int ProcessId { get; set; }

        /// <summary>
        /// 线程信息
        /// </summary>
        [DataMember]
        public ThreadInfo[] ThreadInfos { get; set; }
    }

    /// <summary>
    /// 线程信息
    /// </summary>
    [Serializable, DataContract]
    public class ThreadInfo
    {
        /// <summary>
        /// 线程ID
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 启动时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 线程状态
        /// </summary>
        [DataMember]
        public ThreadState ThreadState { get; set; }

        /// <summary>
        /// 线程优先级
        /// </summary>
        [DataMember]
        public ThreadPriorityLevel ThreadPriorityLevel { get; set; }

        /// <summary>
        /// 线程等待原因
        /// </summary>
        [DataMember]
        public ThreadWaitReason ThreadWaitReason { get; set; }
    }

    /// <summary>
    /// 获取线程信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetThreads_Reply : ReplyEntity
    {
        /// <summary>
        /// 线程信息集合
        /// </summary>
        [DataMember]
        public ThreadInfoCollection[] ThreadInfoCollections { get; set; }
    }
}
