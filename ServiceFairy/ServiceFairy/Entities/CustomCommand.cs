using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 自定义命令信息
    /// </summary>
    [Serializable, DataContract]
    public class CustomCommandInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        /// <summary>
        /// 输入参数信息
        /// </summary>
        [DataMember]
        public CustomCommandParameterInfo[] Input { get; set; }

        /// <summary>
        /// 输出参数信息
        /// </summary>
        [DataMember]
        public CustomCommandParameterInfo[] Output { get; set; }
    }

    /// <summary>
    /// 命令参数信息
    /// </summary>
    [Serializable, DataContract]
    public class CustomCommandParameterInfo
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
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        /// <summary>
        /// 缺少值
        /// </summary>
        [DataMember]
        public string DefaultValue { get; set; }
    }
}
