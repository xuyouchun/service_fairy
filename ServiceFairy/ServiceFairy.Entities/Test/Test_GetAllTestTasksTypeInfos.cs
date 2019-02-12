using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Test
{
    /// <summary>
    /// 获取所有测试任务类型的信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Test_GetAllTestTasksTypeInfos_Request : RequestEntity
    {

    }

    /// <summary>
    /// 获取所有测试任务类型的信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Test_GetAllTestTasksTypeInfos_Reply : ReplyEntity
    {
        [DataMember]
        public TestTaskTypeInfo[] Infos { get; set; }
    }

    /// <summary>
    /// 测试任务类型信息
    /// </summary>
    [Serializable, DataContract]
    public class TestTaskTypeInfo
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        /// <summary>
        /// 参数格式
        /// </summary>
        [DataMember]
        public string ArgsFormat { get; set; }
    }
}
