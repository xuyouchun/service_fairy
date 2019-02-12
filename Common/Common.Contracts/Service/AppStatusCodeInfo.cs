using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 状态码信息
    /// </summary>
    [Serializable, DataContract]
    public class AppStatusCodeInfo
    {
        public AppStatusCodeInfo(int code, string name, string title, string desc)
        {
            Code = code;
            Name = name ?? string.Empty;
            Title = title ?? string.Empty;
            Desc = desc ?? string.Empty;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public int Code { get; private set; }

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

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Title))
                return Code.ToString();

            return Code + " " + Title;
        }
    }
}
