using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Test
{
    /// <summary>
    /// 停止测试－请求
    /// </summary>
    [Serializable, DataContract]
    public class Test_StopTest_Request : RequestEntity
    {
        /// <summary>
        /// 测试ID
        /// </summary>
        [DataMember]
        public Guid[] TestIds { get; set; }
    }

    /// <summary>
    /// 停止测试－应答
    /// </summary>
    [Serializable, DataContract]
    public class Test_StopTest_Reply : ReplyEntity
    {
        
    }
}
