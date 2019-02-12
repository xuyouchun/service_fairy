using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;
using Common.Contracts;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取进程列表－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetProcesses_Reply : ReplyEntity
    {
        /// <summary>
        /// 进程信息列表
        /// </summary>
        [DataMember]
        public ProcessInfo[] ProcessInfos { get; set; }
    }

    /// <summary>
    /// 进程信息
    /// </summary>
    [Serializable, DataContract]
    public class ProcessInfo
    {
        /// <summary>
        /// 进程ID
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// 进程名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 启动命令行
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// 启动时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 线程数
        /// </summary>
        [DataMember]
        public int ThreadCount { get; set; }

        /// <summary>
        /// 当前用户
        /// </summary>
        [DataMember]
        public string User { get; set; }

        /// <summary>
        /// 虚拟内存
        /// </summary>
        [DataMember]
        public long VirtualMemorySize { get; set; }

        [DataMember]
        public long PrivateMemorySize { get; set; }

        [DataMember]
        public long PagedMemorySize { get; set; }

        [DataMember]
        public long PagedSystemMemorySize { get; set; }

        [DataMember]
        public long PeakPagedMemorySize { get; set; }

        [DataMember]
        public long PeakVirtualMemorySize { get; set; }

        [DataMember]
        public long WorkingSet { get; set; }

        [DataMember]
        public long NonpagedSystemMemorySize { get; set; }

        [DataMember]
        public string MainWindowTitle { get; set; }

        [DataMember]
        public long PeakWorkingSet { get; set; }

        [DataMember]
        public TimeSpan TotalProcessorTime { get; set; }

        [DataMember]
        public TimeSpan UserProcessorTime { get; set; }

        [DataMember]
        public int ModuleCount { get; set; }

        [DataMember]
        public ProcessPriorityClass PriorityClass { get; set; }
    }
}
