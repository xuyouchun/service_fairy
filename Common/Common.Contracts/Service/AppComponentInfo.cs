using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 组件列表
    /// </summary>
    [Serializable, DataContract]
    public class AppComponentInfo
    {
        public AppComponentInfo(string name, string title = "", string desc = "", AppComponentCategory componentType = AppComponentCategory.Application)
        {
            Name = name;
            Title = title;
            Desc = desc;
            Category = componentType;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; private set; }

        /// <summary>
        /// 类别
        /// </summary>
        [DataMember]
        public AppComponentCategory Category { get; private set; }
    }
}
