using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Test
{
    /// <summary>
    /// 启动测试任务－请求
    /// </summary>
    [Serializable, DataContract]
    public class Test_StartTest_Request : RequestEntity
    {
        /// <summary>
        /// 名称
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
    }

    /// <summary>
    /// 启动测试任务－应答
    /// </summary>
    [Serializable, DataContract]
    public class Test_StartTest_Reply : ReplyEntity
    {
        /// <summary>
        /// 分配的任务ID
        /// </summary>
        [DataMember]
        public Guid TaskId { get; set; }
    }
}
