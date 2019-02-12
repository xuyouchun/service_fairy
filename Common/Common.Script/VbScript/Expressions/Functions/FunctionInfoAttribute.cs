using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class FunctionInfoAttribute : Attribute
    {
        public FunctionInfoAttribute()
            : this(string.Empty, string.Empty)
        {

        }

        public FunctionInfoAttribute(string description)
            : this(description, string.Empty)
        {

        }

        public FunctionInfoAttribute(string description, string paramInfo)
        {
            Description = description;
            ParamInfo = paramInfo;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        public string ParamInfo { get; private set; }
    }
}
