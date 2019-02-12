using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ServiceFairy
{
    /// <summary>
    /// 用于标注已禁用的接口
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class DisabledCommandAttribute : UsableAttribute
    {
        public DisabledCommandAttribute()
            : this(null)
        {

        }

        public DisabledCommandAttribute(string message)
            : base(UsableType.Disabled, message ?? "该接口已停止使用")
        {

        }
    }

    /// <summary>
    /// 用于标注不推荐使用的接口
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ObsoleteCommandAttribute : UsableAttribute
    {
        public ObsoleteCommandAttribute()
            : this(null)
        {

        }

        public ObsoleteCommandAttribute(string message)
            : base(UsableType.Obsolete, message ?? "该接口已不推荐使用")
        {

        }
    }

    /// <summary>
    /// 用于标注该接口是新添加的
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class NewCommandAttribute : UsableAttribute
    {
        public NewCommandAttribute()
            : this(null)
        {

        }

        public NewCommandAttribute(string message)
            : base(UsableType.New, message ?? "该接口是新添加的")
        {

        }
    }

    /// <summary>
    /// 用于标注该接口已被修改
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ModifiedCommandAttribute : UsableAttribute
    {
        public ModifiedCommandAttribute()
            : this(null)
        {

        }

        public ModifiedCommandAttribute(string message)
            : base(UsableType.Nodified, message ?? "该接口已被修改")
        {

        }
    }
}
