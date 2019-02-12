using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 标识类型或成员的当前可用状态
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class UsableAttribute : Attribute
    {
        public UsableAttribute(UsableType usable, string message)
        {
            Usable = usable;
            Message = message;
        }

        public UsableAttribute(UsableType usable)
            : this(usable, "")
        {

        }

        /// <summary>
        /// 类型
        /// </summary>
        public UsableType Usable { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
    }

    public enum UsableType
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Desc("正常")]
        Normal,

        /// <summary>
        /// 不推荐使用
        /// </summary>
        [Desc("不推荐使用")]
        Obsolete,

        /// <summary>
        /// 已禁用
        /// </summary>
        [Desc("已禁用")]
        Disabled,

        /// <summary>
        /// 新功能
        /// </summary>
        [Desc("新功能")]
        New,

        /// <summary>
        /// 已修改
        /// </summary>
        [Desc("已修改")]
        Nodified,
    }
}
